namespace AreaProg.uDocumentEventHandler
{
    using Umbraco.Core.Events;
    using Umbraco.Core.Models;
    using Umbraco.Core.Publishing;
    using Umbraco.Core.Services;

    public abstract class DocumentEventHandler
    {
        public virtual void Copied(IContentService contentService, CopyEventArgs<IContent> args) { }

        public virtual void Copying(IContentService contentService, CopyEventArgs<IContent> args) { }

        public virtual void Created(IContentService contentService, NewEventArgs<IContent> args) { }

        public virtual void Deleted(IContentService contentService, DeleteEventArgs<IContent> args) { }

        public virtual void DeletedBlueprint(IContentService contentService, DeleteEventArgs<IContent> args) { }

        public virtual void Deleting(IContentService contentService, DeleteEventArgs<IContent> args) { }

        public virtual void Moved(IContentService contentService, MoveEventArgs<IContent> args) { }

        public virtual void Moving(IContentService contentService, MoveEventArgs<IContent> args) { }

        public virtual void Published(IPublishingStrategy strategy, PublishEventArgs<IContent> args) { }

        public virtual void Publishing(IPublishingStrategy strategy, PublishEventArgs<IContent> args) { }

        public virtual void RolledBack(IContentService contentService, RollbackEventArgs<IContent> args) { }

        public virtual void RollingBack(IContentService contentService, RollbackEventArgs<IContent> args) { }

        public virtual void Saved(IContentService contentService, SaveEventArgs<IContent> args) { }

        public virtual void SavedBlueprint(IContentService contentService, SaveEventArgs<IContent> args) { }

        public virtual void Saving(IContentService contentService, SaveEventArgs<IContent> args) { }

        public virtual void SentToPublish(IContentService contentService, SendToPublishEventArgs<IContent> args) { }

        public virtual void SendingToPublish(IContentService contentService, SendToPublishEventArgs<IContent> args) { }

        public virtual void Trashed(IContentService contentService, MoveEventArgs<IContent> args) { }

        public virtual void Trashing(IContentService contentService, MoveEventArgs<IContent> args) { }

        public virtual void Unpublished(IPublishingStrategy strategy, PublishEventArgs<IContent> args) { }

        public virtual void Unpublishing(IPublishingStrategy strategy, PublishEventArgs<IContent> args) { }
    }
}
