namespace AreaProg.uDocumentEventHandler
{
    using System;

    /// <summary>
    /// Defines that the class handles event for documents
    /// </summary>
    public class DocumentEventHandlerAttribute : Attribute
    {
        internal string[] Aliases { get; private set; }
        
        public DocumentEventHandlerAttribute(params string[] aliases)
        {
            Aliases = aliases;
        }
    }
}
