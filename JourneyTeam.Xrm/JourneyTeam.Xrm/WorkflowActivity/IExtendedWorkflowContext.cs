using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;

namespace JourneyTeam.Xrm.WorkflowActivity
{
    public interface IExtendedWorkflowContext : IWorkflowContext
    {
        string WorkflowTypeName { get; }
        EntityReference PrimaryEntity { get; }
        IWorkflowContext WorkflowContext { get; }
        CodeActivityContext ExecutionContext { get; }
        ITracingService TracingService { get; }
        IOrganizationService OrganizationService { get; }
        IOrganizationService SystemOrganizationService { get; }
        IOrganizationService InitiatingUserOrganizationService { get; }
        void Trace(string message);
    }
}
