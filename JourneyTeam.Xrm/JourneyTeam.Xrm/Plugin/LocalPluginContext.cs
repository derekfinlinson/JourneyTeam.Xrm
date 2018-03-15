using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;

namespace JourneyTeam.Xrm.Plugin
{
    public class LocalPluginContext : IExtendedPluginContext
    {
        /// <summary>
        /// Primary entity from the context as an entity reference
        /// </summary>
        public EntityReference PrimaryEntity => new EntityReference(PluginExecutionContext.PrimaryEntityName, PluginExecutionContext.PrimaryEntityId);

        /// <summary>
        /// Event the current plugin is executing for
        /// </summary>
        public RegisteredEvent Event { get; set; }

        /// <summary>
        /// IPluginExecutionContext contains information that describes the run-time environment in which the plug-in executes, 
        /// information related to the execution pipeline, and entity business information
        /// </summary>
        public IPluginExecutionContext PluginExecutionContext { get; }

        /// <summary>
        /// Synchronous registered plug-ins can post the execution context to the Microsoft Azure Service Bus. <br/> 
        /// It is through this notification service that synchronous plug-ins can send brokered messages to the Microsoft Azure Service Bus
        /// </summary>
        public IServiceEndpointNotificationService NotificationService { get; }

        /// <summary>
        /// Provides logging run-time trace information for plug-ins
        /// </summary>
        public ITracingService TracingService { get; set; }

        /// <summary>
        /// Pre Image alias name
        /// </summary>
        public readonly string PreImageAlias = "PreImage";

        /// <summary>
        /// Post Image alias name
        /// </summary>
        public readonly string PostImageAlias = "PostImage";

        private readonly IOrganizationServiceFactory _factory;
        private IOrganizationService _organizationService;
        private IOrganizationService _systemOrganizationService;
        private IOrganizationService _initiatedOrganizationService;

        public IOrganizationService OrganizationService => _organizationService ?? (_organizationService = CreateOrganizationService(PluginExecutionContext.UserId));

        public IOrganizationService SystemOrganizationService => _systemOrganizationService ?? (_systemOrganizationService = CreateOrganizationService(null));

        public IOrganizationService InitiatingUserOrganizationService => _initiatedOrganizationService ?? (_initiatedOrganizationService = CreateOrganizationService(PluginExecutionContext.InitiatingUserId));

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
            PluginExecutionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Obtain the tracing service from the service provider.
            TracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Get the notification service from the service provider.
            NotificationService = (IServiceEndpointNotificationService)serviceProvider.GetService(typeof(IServiceEndpointNotificationService));

            // Obtain the organization factory service from the service provider.
            _factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            // Set Event
            Event = PluginExecutionContext.GetEvent(events);
        }

        public IOrganizationService CreateOrganizationService(Guid? userId)
        {
            return _factory.CreateOrganizationService(userId);
        }

        /// <summary>
        ///     Writes a trace message to the CRM trace log.
        /// </summary>
        /// <param name="message">Message name to trace.</param>
        public void Trace(string message)
        {
            if (string.IsNullOrWhiteSpace(message) || TracingService == null)
            {
                return;
            }

            TracingService.Trace(message);
        }
    }
}
