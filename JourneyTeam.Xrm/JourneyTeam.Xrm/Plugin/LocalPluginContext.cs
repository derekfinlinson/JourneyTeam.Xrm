﻿using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;

namespace JourneyTeam.Xrm.Plugin
{
    public class LocalPluginContext<TRequest, TResponse> : IExtendedPluginContext
        where TRequest : OrganizationRequest, new() where TResponse : OrganizationResponse, new()
    {
        #region IPluginExecutionContext Properties

        public int Stage => PluginExecutionContext.Stage;
        public IPluginExecutionContext ParentContext => PluginExecutionContext.ParentContext;
        public int Mode => PluginExecutionContext.Mode;
        public int IsolationMode => PluginExecutionContext.IsolationMode;
        public int Depth => PluginExecutionContext.Depth;
        public string MessageName => PluginExecutionContext.MessageName;
        public string PrimaryEntityName => PluginExecutionContext.PrimaryEntityName;
        public Guid? RequestId => PluginExecutionContext.RequestId;
        public string SecondaryEntityName => PluginExecutionContext.SecondaryEntityName;
        public ParameterCollection SharedVariables => PluginExecutionContext.SharedVariables;
        public Guid UserId => PluginExecutionContext.UserId;
        public Guid InitiatingUserId => PluginExecutionContext.InitiatingUserId;
        public Guid BusinessUnitId => PluginExecutionContext.BusinessUnitId;
        public Guid OrganizationId => PluginExecutionContext.OrganizationId;
        public string OrganizationName => PluginExecutionContext.OrganizationName;
        public Guid PrimaryEntityId => PluginExecutionContext.PrimaryEntityId;
        public EntityReference OwningExtension => PluginExecutionContext.OwningExtension;
        public Guid CorrelationId => PluginExecutionContext.CorrelationId;
        public bool IsExecutingOffline => PluginExecutionContext.IsExecutingOffline;
        public bool IsOfflinePlayback => PluginExecutionContext.IsOfflinePlayback;
        public bool IsInTransaction => PluginExecutionContext.IsInTransaction;
        public Guid OperationId => PluginExecutionContext.OperationId;
        public DateTime OperationCreatedOn => PluginExecutionContext.OperationCreatedOn;

        /// <summary>
        /// Plugin InputParameters
        /// </summary>
        public ParameterCollection InputParameters => PluginExecutionContext.InputParameters;

        /// <summary>
        /// Plugin OutputParameters
        /// </summary>
        public ParameterCollection OutputParameters => PluginExecutionContext.OutputParameters;

        /// <summary>
        /// Gets the pre entity images.
        /// </summary>
        /// <value>
        /// The pre entity images.
        /// </value>
        public virtual EntityImageCollection PreEntityImages => PluginExecutionContext.PreEntityImages;

        /// <summary>
        /// Gets the post entity images.
        /// </summary>
        /// <value>
        /// The post entity images.
        /// </value>
        public virtual EntityImageCollection PostEntityImages => PluginExecutionContext.PostEntityImages;

        #endregion

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
        /// Event the current plugin is executing for
        /// </summary>
        public RegisteredEvent Event { get; set; }

        public TRequest Request { get; set; }
        public TResponse Response { get; set; }

        private readonly IOrganizationServiceFactory _factory;
        private IOrganizationService _organizationService;
        private IOrganizationService _systemOrganizationService;
        private IOrganizationService _initiatedOrganizationService;

        public IOrganizationService OrganizationService => _organizationService ?? (_organizationService = CreateOrganizationService(UserId));

        public IOrganizationService SystemOrganizationService => _systemOrganizationService ?? (_systemOrganizationService = CreateOrganizationService(null));

        public IOrganizationService InitiatingUserOrganizationService => _initiatedOrganizationService ?? (_initiatedOrganizationService = CreateOrganizationService(InitiatingUserId));

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

            // Set Request and Response
            Request = new TRequest
            {
                Parameters = PluginExecutionContext.InputParameters
            };

            Response = new TResponse
            {
                Results = PluginExecutionContext.OutputParameters
            };
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
