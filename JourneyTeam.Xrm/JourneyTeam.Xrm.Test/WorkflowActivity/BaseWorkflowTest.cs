using System;
using System.Activities;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Moq;

namespace JourneyTeam.Xrm.Test.WorkflowActivity
{
    [TestClass]
    public class BaseWorkflowTest
    {
        private readonly Type _childType;

        public BaseWorkflowTest(Type childType)
        {
            _childType = childType;
        }
        /// <summary>
        /// Invokes the workflow.
        /// </summary>
        /// <param name="context">IWorkflowContext</param>
        /// <param name="inputs">The workflow input parameters</param>
        /// <param name="service">Mock or actual IOrganization Service</param>
        /// <returns>The workflow output parameters</returns>
        protected IDictionary<string, object> InvokeWorkflow(IWorkflowContext context, Dictionary<string, object> inputs, IOrganizationService service)
        {
            var testClass = (CodeActivity)Activator.CreateInstance(_childType);
            
            var factoryMock = new Mock<IOrganizationServiceFactory>();
            var tracingServiceMock = new Mock<ITracingService>();

            //Organization Service Factory Mock
            factoryMock.Setup(t => t.CreateOrganizationService(It.IsAny<Guid>())).Returns(service);
            var factory = factoryMock.Object;

            //Tracing Service - Content written appears in output
            tracingServiceMock.Setup(t => t.Trace(It.IsAny<string>(), It.IsAny<object[]>())).Callback<string, object[]>(MoqExtensions.WriteTrace);
            var tracingService = tracingServiceMock.Object;

            //Workflow Invoker
            var invoker = new WorkflowInvoker(testClass);
            invoker.Extensions.Add(() => tracingService);
            invoker.Extensions.Add(() => context);
            invoker.Extensions.Add(() => factory);

            return invoker.Invoke(inputs);
        }
    }
}
