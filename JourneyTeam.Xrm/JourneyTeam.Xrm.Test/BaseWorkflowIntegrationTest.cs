using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Moq;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Crm.Sdk.Messages;

namespace JourneyTeam.Xrm.Test
{
    public class BaseWorkflowIntegrationTest
    {
        public IOrganizationService OrganizationService
        {
            get
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CRMConnectionString"].ConnectionString;
                if (connectionString.IndexOf("[orgname]", StringComparison.OrdinalIgnoreCase) >= 0)
                    throw new Exception("CRM connection string not set in app.config.");

                var connection =
                    CrmConnection.Parse(ConfigurationManager.ConnectionStrings["CRMConnectionString"].ConnectionString);

                return new OrganizationService(connection);
            }
        }

        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) { }

        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup]
        public static void ClassCleanup() { }

        /// <summary>
        /// Invokes the workflow.
        /// </summary>
        /// <param name="workflowType"></param>
        /// <param name="target">The target entity</param>
        /// <param name="inputs">The workflow input parameters</param>
        /// <returns>The workflow output parameters</returns>
        protected IDictionary<string, object> InvokeWorkflow(Type workflowType, ref Entity target, Dictionary<string, object> inputs)
        {
            var testClass = Activator.CreateInstance(workflowType) as CodeActivity;

            var factoryMock = new Mock<IOrganizationServiceFactory>();
            var tracingServiceMock = new Mock<ITracingService>();
            var workflowContextMock = new Mock<IWorkflowContext>();

            var orgService = OrganizationService;

            //Mock workflow Context
            var workflowUserId = Guid.NewGuid();
            var workflowCorrelationId = Guid.NewGuid();
            var workflowInitiatingUserId = Guid.NewGuid();

            //Workflow Context Mock
            workflowContextMock.Setup(t => t.InitiatingUserId).Returns(workflowInitiatingUserId);
            workflowContextMock.Setup(t => t.CorrelationId).Returns(workflowCorrelationId);
            workflowContextMock.Setup(t => t.UserId).Returns(workflowUserId);
            var workflowContext = workflowContextMock.Object;

            //Organization Service Factory Mock
            factoryMock.Setup(t => t.CreateOrganizationService(It.IsAny<Guid>())).Returns(orgService);
            var factory = factoryMock.Object;

            //Tracing Service - Content written appears in output
            tracingServiceMock.Setup(t => t.Trace(It.IsAny<string>(), It.IsAny<object[]>())).Callback<string, object[]>(MoqExtensions.WriteTrace);
            var tracingService = tracingServiceMock.Object;

            //Parameter Collection
            ParameterCollection inputParameters = new ParameterCollection { { "Target", target } };
            workflowContextMock.Setup(t => t.InputParameters).Returns(inputParameters);

            //Workflow Invoker
            var invoker = new WorkflowInvoker(testClass);
            invoker.Extensions.Add(() => tracingService);
            invoker.Extensions.Add(() => workflowContext);
            invoker.Extensions.Add(() => factory);

            return invoker.Invoke(inputs);
        }

        protected Guid GetUser(IOrganizationService service)
        {
            var request = new WhoAmIRequest();

            var response = (WhoAmIResponse)service.Execute(request);

            return response.UserId;
        }
    }
}
