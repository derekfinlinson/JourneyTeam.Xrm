using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.PluginTelemetry;

namespace Xrm
{
    public interface IExtendedPluginContext : IPluginExecutionContext7, IExtendedExecutionContext
    {
        /// <summary>
        /// Fullname of the plugin
        /// </summary>
        string PluginTypeName { get; }

        /// <summary>
        /// Event the plugin is executing against
        /// </summary>
        RegisteredEvent Event { get; }

        /// <summary>
        /// Pre Image alias name
        /// </summary>
        string PreImageAlias { get; }

        /// <summary>
        /// Post Image alias name
        /// </summary>
        string PostImageAlias { get; }

        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Pipeline stage for the context
        /// </summary>
        PipelineStage PipelineStage { get; }

        /// <summary>
        /// IPluginExecutionContext7 contains information that describes the run-time environment in which the plug-in executes, 
        /// information related to the execution pipeline, and entity business information
        /// </summary>
        IPluginExecutionContext7 PluginExecutionContext { get; }

        /// <summary>
        /// Synchronous registered plug-ins can post the execution context to the Microsoft Azure Service Bus. <br/> 
        /// It is through this notification service that synchronous plug-ins can send brokered messages to the Microsoft Azure Service Bus
        /// </summary>
        IServiceEndpointNotificationService NotificationService { get; }

        /// <summary>
        /// ILogger for this plugin.
        /// </summary>
        ILogger Logger { get; }

        /// <summary>
        /// Get a <see href="OrganizationRequest" /> object for the current plugin execution
        /// </summary>
        T GetRequest<T>() where T : OrganizationRequest, new();

        /// <summary>
        /// Get a <see href="OrganizationResponse" /> object for the current plugin execution
        /// </summary>
        T GetResponse<T>() where T : OrganizationResponse, new();

        /// <summary>
        /// Prevent plugin from running multiple times for the same context
        /// </summary>
        bool IsDuplicatePluginExecution();
    }
}
