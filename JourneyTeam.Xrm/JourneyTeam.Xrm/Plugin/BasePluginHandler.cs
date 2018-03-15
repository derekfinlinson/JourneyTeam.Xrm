using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace JourneyTeam.Xrm.Plugin
{
    public abstract class BasePluginHandler<T> : IPlugin
        where T: IExtendedPluginContext
    {
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
        protected BasePluginHandler()
        {
            ChildClassName = GetType().FullName;
            RegisteredEvents = new List<RegisteredEvent>();
        }

        /// <summary>
        /// Register events for the plugin
        /// </summary>
        public abstract void RegisterEvents();

        /// <summary>
        ///     Creates the local plugin context
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public abstract T CreatePluginContext(IServiceProvider serviceProvider);

        /// <summary>
        /// Execution method for the plugin
        /// </summary>
        /// <param name="context">Context for the current plug-in.</param>
        protected abstract void ExecutePlugin(T context);

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
            var context = CreatePluginContext(serviceProvider);

            context.Trace($"Entered {ChildClassName}.Execute()");

            try
            {
                // Verify plugin is running for a registered event
                if (context.Event == null)
                {
                    var message = context.PluginExecutionContext.MessageName;
                    var entity = context.PluginExecutionContext.PrimaryEntityName;
                    var stage = context.PluginExecutionContext.Stage;

                    context.Trace($"No Registered Event Found for Event: {message}, Entity: {entity}, and Stage: {stage}!");
                    return;
                }

                // Invoke the custom implementation
                var execute = context.Event.Execute == null
                    ? ExecutePlugin
                    : new Action<T>(c => context.Event.Execute(c));

                execute(context);
            }
            catch (FaultException<OrganizationServiceFault> e)
            {
                context.Trace($"Exception: {e}");

                throw;
            }
            finally
            {
                context.Trace($"Exiting {ChildClassName}.Execute()");
            }
        }
    }
}
