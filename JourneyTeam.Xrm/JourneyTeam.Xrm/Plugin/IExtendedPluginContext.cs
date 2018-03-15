using Microsoft.Xrm.Sdk;

namespace JourneyTeam.Xrm.Plugin
{
    public interface IExtendedPluginContext : IPluginExecutionContext
    {
        OrganizationRequest Request { get; }
        OrganizationResponse Response { get; }
        string PluginTypeName { get; }
        RegisteredEvent Event { get; }
        EntityReference PrimaryEntity { get; }
        IPluginExecutionContext PluginExecutionContext { get; }
        IServiceEndpointNotificationService NotificationService { get; }
        ITracingService TracingService { get; }
        IOrganizationService OrganizationService { get; }
        IOrganizationService SystemOrganizationService { get; }
        IOrganizationService InitiatingUserOrganizationService { get; }
        void Trace(string message);
    }
}
