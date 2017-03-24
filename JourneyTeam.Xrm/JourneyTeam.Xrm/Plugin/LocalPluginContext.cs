using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;

namespace JourneyTeam.Xrm.Plugin
{
    public class LocalPluginContext
    {
        /// <summary>
        /// Microsoft Dynamics CRM organization service factory
        /// </summary>
        private readonly IOrganizationServiceFactory _factory;

        /// <summary>
        /// Microsoft Dynamics CRM organization service
        /// </summary>
        public IOrganizationService OrganizationService { get; private set; }
        
        /// <summary>
        /// IPluginExecutionContext contains information that describes the run-time environment in which the plug-in executes, 
        /// information related to the execution pipeline, and entity business information
        /// </summary>
        public IPluginExecutionContext PluginExecutionContext { get; }
        
        /// <summary>
        /// Synchronous registered plug-ins can post the execution context to the Microsoft Azure Service Bus. <br/> 
        /// It is through this notification service that synchronous plug-ins can send brokered messages to the Microsoft Azure Service Bus
        /// </summary>
        public IServiceEndpointNotificationService NotificationService { get; private set; }

        /// <summary>
        /// Event the current plugin is executing for
        /// </summary>
        public RegisteredEvent Event { get; set; }

        /// <summary>
        /// Post Entity Images
        /// </summary>
        public EntityImageCollection PostImages { get; set; }

        /// <summary>
        /// Pre Entity Images
        /// </summary>
        public EntityImageCollection PreImages { get; set; }

        /// <summary>
        /// Plugin InputParameters
        /// </summary>
        public ParameterCollection InputParameters { get; set; }

        /// <summary>
        /// Provides logging run-time trace information for plug-ins
        /// </summary>
        private readonly ITracingService _tracingService;

        /// <summary>
        /// Helper object that stores the services available in this plug-in
        /// </summary>
        /// <param name="serviceProvider">IServiceProvider</param>
        /// <param name="events">List of events the plugin should fire against</param>
        public LocalPluginContext(IServiceProvider serviceProvider, IEnumerable<RegisteredEvent> events)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }
            
            // Obtain the execution context service from the service provider.
            this.PluginExecutionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            
            // Obtain the post and pre images
            this.PostImages = this.PluginExecutionContext.PostEntityImages;
            this.PreImages = this.PluginExecutionContext.PreEntityImages;
            this.InputParameters = this.InputParameters;

            // Obtain the tracing service from the service provider.
            this._tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Get the notification service from the service provider.
            this.NotificationService = (IServiceEndpointNotificationService)serviceProvider.GetService(typeof(IServiceEndpointNotificationService));

            // Obtain the organization factory service from the service provider.
            this._factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            
            // Use the factory to generate the organization service.
            this.OrganizationService = CreateOrganizationService(this.PluginExecutionContext.UserId);

            // Set Event
            this.Event = this.PluginExecutionContext.GetEvent(events);
        }

        /// <summary>
        /// Create CRM Organization Service for a specific user id
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>CRM Organization Service</returns>
        /// <remarks>Useful for impersonation</remarks>
        public IOrganizationService CreateOrganizationService(Guid userId)
        {
            return this._factory.CreateOrganizationService(userId);
        }

        /// <summary>
        /// Writes a trace message to the CRM trace log.
        /// </summary>
        /// <param name="message">Message name to trace.</param>
        public void Trace(string message)
        {
            if (string.IsNullOrWhiteSpace(message) || this._tracingService == null)
            {
                return;
            }
            
            this._tracingService.Trace(message);
        }
    }
}
