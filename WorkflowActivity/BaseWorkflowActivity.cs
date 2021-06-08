using System;
using System.Activities;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;

namespace Xrm
{
    public abstract class BaseWorkflowActivity : CodeActivity
    {
        protected override void Execute(CodeActivityContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // Get local workflow context
            var workflowContext = new BaseWorkflowActivityContext(context, this);

            workflowContext.Trace($"Entered {workflowContext.WorkflowTypeName}.Execute()");

            try
            {
                // Invoke the custom implementation 
                ExecuteWorkflowActivity(workflowContext);
            }
            catch (FaultException<OrganizationServiceFault> e)
            {
                workflowContext.Trace($"Exception: {e}");

                // Handle the exception.
                throw;
            }
            finally
            {
                workflowContext.Trace($"Exiting {workflowContext.WorkflowTypeName}.Execute()");
            }
        }

        /// <summary>
        /// Execution method for the workflow activity
        /// </summary>
        /// <param name="localContext">Context for the current plug-in.</param>
        public abstract void ExecuteWorkflowActivity(IExtendedWorkflowContext context);
    }
}
