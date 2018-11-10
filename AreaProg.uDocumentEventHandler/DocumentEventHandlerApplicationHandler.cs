namespace AreaProg.uDocumentEventHandler
{
    using System;
    using System.Configuration;
    using System.Linq;
    using Umbraco.Core;
    using Umbraco.Core.Logging;

    /// <summary>
    /// Application handler that automatically binds the document events using the web.config settings
    /// </summary>
    public class DocumentEventHandlerApplicationHandler : ApplicationEventHandler
    {
        private const string WebConfigSetting = "AreaProg.uDocumentEventHandler.ModelAssembly";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="umbracoApplication"></param>
        /// <param name="applicationContext"></param>
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            LogHelper.Info<DocumentEventHandlerApplicationHandler>("Initializing...");

            var settings = ConfigurationManager.AppSettings[WebConfigSetting];

            if (!string.IsNullOrWhiteSpace(settings))
            {
                LogHelper.Info<DocumentEventHandlerApplicationHandler>($"Trying to load the assembly \"{settings}\"...");

                var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name.Equals(settings, StringComparison.InvariantCultureIgnoreCase));

                if (assembly != null)
                {
                    LogHelper.Info<DocumentEventHandlerApplicationHandler>($"Assembly loaded!");
                    LogHelper.Info<DocumentEventHandlerApplicationHandler>($"Binding events...");

                    DocumentEventHandlerBinder.Bind(assembly);

                    LogHelper.Info<DocumentEventHandlerApplicationHandler>($"Events bound!");
                }
            }

            LogHelper.Info<DocumentEventHandlerApplicationHandler>("Initialized!");
        }
    }
}
