using Microsoft.Xrm.Sdk;

namespace Xrm
{
    public interface IExtendedPluginContext : IPluginExecutionContext, IExtendedExecutionContext
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

        /// <summary>
        /// IPluginExecutionContext contains information that describes the run-time environment in which the plug-in executes, 
        /// information related to the execution pipeline, and entity business information
        /// </summary>
        IPluginExecutionContext PluginExecutionContext { get; }

        /// <summary>
        /// Synchronous registered plug-ins can post the execution context to the Microsoft Azure Service Bus. <br/> 
        /// It is through this notification service that synchronous plug-ins can send brokered messages to the Microsoft Azure Service Bus
        /// </summary>
        IServiceEndpointNotificationService NotificationService { get; }

        /// <summary>
        /// Get a <see href="OrganizationRequest" /> object for the current plugin execution
        /// </summary>
        OrganizationRequest GetRequest<T>() where T : OrganizationRequest, new();

        /// <summary>
        /// Get a <see href="OrganizationResponse" /> object for the current plugin execution
        /// </summary>
        OrganizationResponse GetResponse<T>() where T : OrganizationResponse, new();

        bool PreventDuplicatePluginExecution(); 
    }
}
