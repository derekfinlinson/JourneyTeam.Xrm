using System;
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
        /// Gets or sets the name of the child class.
        /// </summary>
        /// <value>The name of the child class.</value>
        protected string ChildClassName { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BasePlugin"/> class.
        /// </summary>
        /// <param name="childClassName">The <see cref="Type"/> of the derived class.</param>
        protected BasePlugin(Type childClassName)
        {
            ChildClassName = childClassName.ToString();
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

            // Construct the local plug-in context.
            var localcontext = new LocalPluginContext(serviceProvider);

            localcontext.Trace($"Entered {ChildClassName}.Execute()");

            try
            {
                // Invoke the custom implementation 
                ExecutePlugin(localcontext);
            }
            catch (FaultException<OrganizationServiceFault> e)
            {
                localcontext.Trace($"Exception: {e}");

                // Handle the exception.
                throw;
            }
            finally
            {
                localcontext.Trace($"Exiting {ChildClassName}.Execute()");
            }
        }

        /// <summary>
        /// Placeholder for a custom plug-in implementation. 
        /// </summary>
        /// <param name="localContext">Context for the current plug-in.</param>
        protected abstract void ExecutePlugin(LocalPluginContext localContext);
    }
}
