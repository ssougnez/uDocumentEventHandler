namespace AreaProg.uDocumentEventHandler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Umbraco.Core;
    using Umbraco.Core.Models;
    using Umbraco.Core.Services;

    /// <summary>
    /// Used to bind document events to classes
    /// </summary>
    public static class DocumentEventHandlerBinder
    {
        private static readonly List<DocumentEventHandlerInfo> data = new List<DocumentEventHandlerInfo>();

        /// <summary>
        /// Bind the document event handlers
        /// </summary>
        /// <param name="assembly">Assembly that contains the document handlers</param>
        public static void Bind(Assembly assembly)
        {
            Init(assembly);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="action"></param>
        private static void HandleEvents(IEnumerable<IContent> entities, Action<DocumentEventHandler> action)
        {
            var alias = entities.FirstOrDefault()?.ContentType.Alias;

            if (!string.IsNullOrEmpty(alias))
            {
                var handlers = data.Where(info => (info.Attribute.Include == null || info.Attribute.Include.Length == 0 || info.Attribute.Include.Contains(alias)) && (info.Attribute.Exclude == null || !info.Attribute.Exclude.Contains(alias)));

                foreach (var handler in handlers)
                {
                    action(handler.Handler);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="action"></param>
        private static void HandleEvents(IContent entity, Action<DocumentEventHandler> action)
        {
            HandleEvents(new List<IContent> { entity }, action);
        }

        /// <summary>
        /// 
        /// </summary>
        private static void Init(Assembly assembly)
        {
            var handlerTypes = assembly.GetTypes().Where(t => t.BaseType == typeof(DocumentEventHandler));

            foreach (var handlerType in handlerTypes)
            {
                var attribute = handlerType.GetCustomAttribute<DocumentEventHandlerAttribute>();

                if (attribute != null)
                {
                    data.Add(new DocumentEventHandlerInfo
                    {
                        Handler = Activator.CreateInstance(handlerType) as DocumentEventHandler,
                        Attribute = handlerType.GetCustomAttribute<DocumentEventHandlerAttribute>()
                    });
                }
            }

            BindEvents();
        }

        /// <summary>
        /// 
        /// </summary>
        private static void BindEvents()
        {
            ContentService.Copied += (cs, args) => HandleEvents(args.Original, h => h.Copied(cs, args));
            ContentService.Copying += (cs, args) => HandleEvents(args.Original, h => h.Copying(cs, args));
            ContentService.Created += (contentService, args) => HandleEvents(args.Entity, h => h.Created(contentService, args));
            ContentService.Deleted += (contentService, args) => HandleEvents(args.DeletedEntities, h => h.Deleted(contentService, args));
            ContentService.DeletedBlueprint += (cs, args) => HandleEvents(args.DeletedEntities, h => h.DeletedBlueprint(cs, args));
            ContentService.Deleting += (contentService, args) => HandleEvents(args.DeletedEntities, h => h.Deleting(contentService, args));
            ContentService.Moved += (cs, args) => HandleEvents(args.MoveInfoCollection.Select(mic => mic.Entity), h => h.Moved(cs, args));
            ContentService.Moving += (cs, args) => HandleEvents(args.MoveInfoCollection.Select(mic => mic.Entity), h => h.Moving(cs, args));
            ContentService.RolledBack += (cs, args) => HandleEvents(args.Entity, h => h.RolledBack(cs, args));
            ContentService.RollingBack += (cs, args) => HandleEvents(args.Entity, h => h.RollingBack(cs, args));
            ContentService.Saved += (contentService, args) => HandleEvents(args.SavedEntities, h => h.Saved(contentService, args));
            ContentService.SavedBlueprint += (contentService, args) => HandleEvents(args.SavedEntities, h => h.SavedBlueprint(contentService, args));
            ContentService.Saving += (contentService, args) => HandleEvents(args.SavedEntities, h => h.Saving(contentService, args));
            ContentService.SendingToPublish += (cs, args) => HandleEvents(args.Entity, h => h.SendingToPublish(cs, args));
            ContentService.SentToPublish += (cs, args) => HandleEvents(args.Entity, h => h.SentToPublish(cs, args));
            ContentService.Trashed += (cs, args) => HandleEvents(args.MoveInfoCollection.Select(mic => mic.Entity), h => h.Trashed(cs, args));
            ContentService.Trashing += (cs, args) => HandleEvents(args.MoveInfoCollection.Select(mic => mic.Entity), h => h.Trashing(cs, args));
            ContentService.Published += (strategy, args) => HandleEvents(args.PublishedEntities, h => h.Published(strategy, args));
            ContentService.Publishing += (strategy, args) => HandleEvents(args.PublishedEntities, h => h.Publishing(strategy, args));
            ContentService.UnPublished += (strategy, args) => HandleEvents(args.PublishedEntities, h => h.Unpublished(strategy, args));
            ContentService.UnPublishing += (strategy, args) => HandleEvents(args.PublishedEntities, h => h.Unpublishing(strategy, args));
        }
    }
}
