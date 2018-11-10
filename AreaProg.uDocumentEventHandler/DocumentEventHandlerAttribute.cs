namespace AreaProg.uDocumentEventHandler
{
    using System;

    /// <summary>
    /// Defines that the class handlers event for documents
    /// </summary>
    public class DocumentEventHandlerAttribute : Attribute
    {
        /// <summary>
        /// List of document type aliases handled by the handler
        /// </summary>
        public string[] Include { get; set; }

        /// <summary>
        /// List of document types aliases that won't be handled by the handler
        /// </summary>
        public string[] Exclude { get; set; }
    }
}
