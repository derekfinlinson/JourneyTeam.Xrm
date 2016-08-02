using System;
using Microsoft.Xrm.Sdk;

namespace JourneyTeam.Xrm.Plugin
{
    public class LocalPluginContext
    {
        /// <summary>
        ///     Microsoft Dynamics CRM organization service.
        /// </summary>
        public IOrganizationService OrganizationService { get; private set; }

        /// <summary>
        ///     IPluginExecutionContext contains information that describes the run-time environment in which the plug-in executes, information related to the execution pipeline, and entity business information.
        /// </summary>
        public IPluginExecutionContext PluginExecutionContext { get; private set; }

        /// <summary>
        ///     Synchronous registered plug-ins can post the execution context to the Microsoft Azure Service Bus. <br/> 
        ///     It is through this notification service that synchronous plug-ins can send brokered messages to the Microsoft Azure Service Bus.
        /// </summary>
        public IServiceEndpointNotificationService NotificationService { get; private set; }

        /// <summary>
        ///     Provides logging run-time trace information for plug-ins. 
        /// </summary>
        public ITracingService TracingService { get; private set; }

        /// <summary>
        ///     Post Entity Images
        /// </summary>
        public EntityImageCollection PostImages { get; set; }

        /// <summary>
        ///     Pre Entity Images
        /// </summary>
        public EntityImageCollection PreImages { get; set; }

        /// <summary>
        /// Helper object that stores the services available in this plug-in.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public LocalPluginContext(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }
            
            // Obtain the execution context service from the service provider.
            PluginExecutionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            
            // Obtain the post and pre images
            PostImages = PluginExecutionContext.PostEntityImages;
            PreImages = PluginExecutionContext.PreEntityImages;

            // Obtain the tracing service from the service provider.
            TracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Get the notification service from the service provider.
            NotificationService = (IServiceEndpointNotificationService)serviceProvider.GetService(typeof(IServiceEndpointNotificationService));

            // Obtain the organization factory service from the service provider.
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            
            // Use the factory to generate the organization service.
            OrganizationService = factory.CreateOrganizationService(PluginExecutionContext.UserId);
        }

        /// <summary>
        /// Writes a trace message to the CRM trace log.
        /// </summary>
        /// <param name="message">Message name to trace.</param>
        public void Trace(string message)
        {
            if (string.IsNullOrWhiteSpace(message) || TracingService == null)
            {
                return;
            }

            TracingService.Trace(PluginExecutionContext == null
                ? message
                : $"{message}, Correlation Id: {PluginExecutionContext.CorrelationId}, Initiating User: {PluginExecutionContext.InitiatingUserId}");
        }
    }
}
