using System.Activities;
using Microsoft.Xrm.Sdk.Workflow;

namespace Xrm
{
    public interface IExtendedWorkflowContext: IWorkflowContext, IExtendedExecutionContext
    {
        /// <summary>
        /// Fullname of the workflow activity
        /// </summary>
        string WorkflowTypeName { get; }

        /// <summary>
        /// Extends ActivityContext and provides additional functionality for CodeActivity
        /// </summary>
        CodeActivityContext ActivityContext { get; }
    }
}