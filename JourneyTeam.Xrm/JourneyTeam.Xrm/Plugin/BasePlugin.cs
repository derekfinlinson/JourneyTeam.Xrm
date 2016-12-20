using System;
using System.Collections.Generic;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;

namespace JourneyTeam.Xrm.Plugin
{
    public abstract class BasePlugin : IPlugin
    {
        /// <summary>
        ///     Pre Image alias name
        /// </summary>
        public const string PreImageAlias = "PreImage";

        /// <summary>
        ///     Post Image alias name
        /// </summary>
        public const string PostImageAlias = "PostImage";

        /// <summary>
        /// Registered events for the plugin
        /// </summary>
        public List<RegisteredEvent> RegisteredEvents { get; set; }

        /// <summary>
        /// Gets or sets the name of the child class.
        /// </summary>
        /// <value>The name of the child class.</value>
        protected string ChildClassName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePlugin"/> class.
        /// </summary>
        /// <param name="childClassName">The <see cref="Type"/> of the derived class.</param>
        protected BasePlugin(Type childClassName)
        {
            ChildClassName = childClassName.FullName;
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

            // Add registered events
            RegisterEvents();
            
            // Construct the local plug-in context.
            var localContext = new LocalPluginContext(serviceProvider, RegisteredEvents);

            localContext.Trace($"Entered {ChildClassName}.Execute()");

            try
            {
                // Verify plugin is running for a registered event
                if (localContext.Event == null)
                {
                    localContext.Trace(
                        $"No Registered Event Found for Event: {localContext.PluginExecutionContext.MessageName}, Entity: {localContext.PluginExecutionContext.PrimaryEntityName}, and Stage: {localContext.PluginExecutionContext.Stage}!");
                    return;
                }

                // Invoke the custom implementation 
                var execute = localContext.Event.Execute == null
                    ? ExecutePlugin
                    : new Action<LocalPluginContext>(c => localContext.Event.Execute(c));

                execute(localContext);
            }
            catch (FaultException<OrganizationServiceFault> e)
            {
                localContext.Trace($"Exception: {e}");

                // Handle the exception.
                throw;
            }
            finally
            {
                localContext.Trace($"Exiting {ChildClassName}.Execute()");
            }
        }

        /// <summary>
        /// Register events for the plugin
        /// </summary>
        protected abstract void RegisterEvents();

        /// <summary>
        /// Execution method for the plugin
        /// </summary>
        /// <param name="localContext">Context for the current plug-in.</param>
        protected abstract void ExecutePlugin(LocalPluginContext localContext);
    }
}
