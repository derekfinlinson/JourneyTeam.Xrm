using Microsoft.Xrm.Sdk;

namespace JourneyTeam.Xrm.Plugin
{
    public interface IExtendedPluginContext : IPluginExecutionContext
    {
        IServiceEndpointNotificationService NotificationService { get; }
        ITracingService TracingService { get; }
        RegisteredEvent Event { get; }
        IOrganizationService OrganizationService { get; }
        IOrganizationService SystemOrganizationService { get; }
        IOrganizationService InitiatingUserOrganizationService { get; }
        void Trace(string message);
    }
}
