using System;
using System.Activities;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;

namespace JourneyTeam.Xrm.WorkflowActivity
{
    public abstract class BaseWorkflowActivity : CodeActivity
    {
        /// <summary>
        /// Gets or sets the name of the child class.
        /// </summary>
        /// <value>The name of the child class.</value>
        protected string ChildClassName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseWorkflowActivity"/> class.
        /// </summary>
        /// <param name="childClassName">The <see cref="Type"/> of the derived class.</param>
        protected BaseWorkflowActivity(Type childClassName)
        {
            ChildClassName = childClassName.ToString();
        }

        protected override void Execute(CodeActivityContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // Get local workflow context
            var localContext = new LocalWorkflowContext(context);

            localContext.Trace($"Entered {ChildClassName}.Execute()");

            try
            {
                // Invoke the custom implementation 
                ExecuteWorkflowActivity(localContext);
            }
            catch (FaultException<OrganizationServiceFault> e)
            {
                localContext.Trace($"Exception: {e}");

                // Handle the exception.
                throw;
            }
            finally
            {
                localContext.Trace($"Exiting {ChildClassName}.Execute()");
            }
        }

        /// <summary>
        /// Placeholder for a custom plug-in implementation. 
        /// </summary>
        /// <param name="localContext">Context for the current plug-in.</param>
        protected abstract void ExecuteWorkflowActivity(LocalWorkflowContext localContext);
    }
}
