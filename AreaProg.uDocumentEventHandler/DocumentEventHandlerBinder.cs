namespace AreaProg.uDocumentEventHandler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Umbraco.Core;
    using Umbraco.Core.Models;
    using Umbraco.Core.Models.PublishedContent;
    using Umbraco.Core.Services;

    /// <summary>
    /// 
    /// </summary>
    public class DocumentEventHandlerBinder : ApplicationEventHandler
    {
        private readonly Dictionary<string, List<DocumentEventHandler>> handlers = new Dictionary<string, List<DocumentEventHandler>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="umbracoApplication"></param>
        /// <param name="applicationContext"></param>
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            Init();

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="action"></param>
        private void HandleEvents(IEnumerable<IContent> entities, Action<DocumentEventHandler> action)
        {
            var alias = entities.FirstOrDefault()?.ContentType.Alias;

            if (!string.IsNullOrEmpty(alias) && handlers.ContainsKey(alias))
            {
                foreach (var handler in handlers[alias])
                {
                    action(handler);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="action"></param>
        private void HandleEvents(IContent entity, Action<DocumentEventHandler> action)
        {
            HandleEvents(new List<IContent> { entity }, action);
        }

        /// <summary>
        /// 
        /// </summary>
        private void Init()
        {
            // TODO: Add key in the web config to define where to models are located and where the document event handler are located
            var modelTypes = GetType().Assembly.GetTypes().Where(t => t.BaseType == typeof(PublishedContentModel));
            var handlerTypes = GetType().Assembly.GetTypes().Where(t => t.BaseType == typeof(DocumentEventHandler));

            // Looping through all the document type of the project
            foreach (var modelType in modelTypes)
            {
                var modelAlias = modelType.GetCustomAttribute<PublishedContentModelAttribute>().ContentTypeAlias;
                var modelHandlers = new List<DocumentEventHandler>();

                // Looping through all the document event handler class of the project
                foreach (var handlerType in handlerTypes)
                {
                    var handlerAliases = handlerType.GetCustomAttribute<DocumentEventHandlerAttribute>().Aliases;

                    if (handlerAliases.Length == 0 || handlerAliases.Contains(modelAlias))
                    {
                        modelHandlers.Add(Activator.CreateInstance(handlerType) as DocumentEventHandler);
                    }
                }

                handlers.Add(modelAlias, modelHandlers);
            }
        }
    }
}
