using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Xrm
{
    public abstract class BasePlugin : IBasePlugin
    {
        /// <summary>
        /// Plugin unsecure configuration object
        /// </summary>
        public string UnsecureConfig { get; }

        /// <summary>
        /// Plugin secure configuration object
        /// </summary>
        public string SecureConfig { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePlugin"/> class without configuration data.
        /// </summary>
        protected BasePlugin() : this(null, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePlugin"/> class with configuration data.
        /// </summary>
        /// <param name="unsecure">Unsecure configuration</param>
        /// <param name="secure">Secure configuration</param>
        protected BasePlugin(string unsecure, string secure)
        {
            // Set unsecure and secure config
            UnsecureConfig = unsecure;
            SecureConfig = secure;
        }

        /// <summary>
        /// Creates the base plugin context
        /// </summary>
        /// <param name="serviceProvider">The <see href="IServiceProvider" /> object</param>
        /// <param name="events">List of <see href="RegisteredEvents" /> for the plugin</param>
        /// <returns>IExtendedPluginContext object</returns>
        public virtual IExtendedPluginContext CreatePluginContext(IServiceProvider serviceProvider, IEnumerable<RegisteredEvent> events)
        {
            return new BasePluginContext(serviceProvider, events, this);
        }

        /// <summary>
        /// Main entry point for he business logic that the plug-in is to execute.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <remarks>
        /// For improved performance, Microsoft Dynamics CRM caches plug-in instances. 
        /// The plug-in's Execute method should be written to be stateless as the constructor 
        /// is not called for every invocation of the plug-in. Also, multiple system threads 
        /// could execute the plug-in at the same time. All per invocation state information 
        /// is stored in the context. This means that you should not use global variables in plug-ins.
        /// </remarks>
        public void Execute(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var events = GetRegisteredEvents();

            // Construct the local plug-in context.
            var context = CreatePluginContext(serviceProvider, events);

            context.Trace($"Entered {context.PluginTypeName}.Execute()");

            try
            {
                // Verify plugin is running for a registered event
                if (context.Event == null)
                {
                    context.Trace($"No Registered Event found for event: {context.MessageName}, Entity: {context.PrimaryEntityName}, and Stage: {context.PipelineStage.ToString()}!");
                    return;
                }

                if (context.IsDuplicatePluginExecution())
                {
                    context.Trace("Duplicate execution prevented");
                    return;
                }

                // Invoke the custom implementation
                var execute = context.Event.Execute == null
                    ? ExecutePlugin
                    : new Action<IExtendedPluginContext>(c => context.Event.Execute(c));

                var mode = (SdkMessageProcessingStepMode)context.Mode;
                
                context.Trace($"Executing registered event: {context.MessageName}, Entity: {context.PrimaryEntityName}, Mpde: {mode.ToString()}, and Stage: {context.PipelineStage.ToString()}!");

                execute(context);
            }
            catch (FaultException<OrganizationServiceFault> e)
            {
                context.Trace($"Exception: {e}");

                throw;
            }
            finally
            {
                context.Trace($"Exiting {context.PluginTypeName}.Execute()");
            }
        }

        /// <summary>
        /// Get list of <see href="RegisteredEvents" /> for the plugin.
        /// </summary>
        public abstract IEnumerable<RegisteredEvent> GetRegisteredEvents();

        /// <summary>
        /// Defalt execution method for the plugin
        /// </summary>
        /// <param name="context"><see href="IExtendedPluginContext" /> object for the current plug-in.</param>
        public abstract void ExecutePlugin(IExtendedPluginContext context);
    }
}
