using System;
using System.Activities;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;

namespace Xrm
{
    public abstract class BaseWorkflowActivity : CodeActivity
    {
        protected override void Execute(CodeActivityContext activityContext)
        {
            if (activityContext == null)
            {
                throw new ArgumentNullException(nameof(activityContext));
            }

            // Get local workflow context
            var context = new BaseWorkflowActivityContext(activityContext, this);

            context.Trace($"Entered {context.WorkflowTypeName}.Execute()");

            try
            {
                // Invoke the custom implementation 
                ExecuteWorkflowActivity(context);
            }
            catch (FaultException<OrganizationServiceFault> e)
            {
                context.Trace($"Exception: {e}");

                // Handle the exception.
                throw;
            }
            finally
            {
                context.Trace($"Exiting {context.WorkflowTypeName}.Execute()");
            }
        }

        /// <summary>
        /// Execution method for the workflow activity
        /// </summary>
        /// <param name="localContext">Context for the current plug-in.</param>
        public abstract void ExecuteWorkflowActivity(IExtendedWorkflowContext context);
    }
}
