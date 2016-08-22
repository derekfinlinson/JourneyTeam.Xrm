using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;
using Moq;

namespace JourneyTeam.Xrm.Test
{
    public class BasePluginIntegrationTest
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

        private readonly Type _childType;

        public BasePluginIntegrationTest(Type childType)
        {
            _childType = childType;
        }

        [ClassInitialize()]
        protected virtual void ClassInitialize(TestContext testContext) { }

        [ClassCleanup()]
        protected virtual void ClassTearDown() { }

        /// <summary>
        /// Invokes the plug-in.
        /// </summary>
        /// <param name="target">The target entity</param>
        /// <param name="inputs"></param>
        /// <param name="outputs"></param>
        protected void InvokePlugin(ref Entity target, ParameterCollection inputs, ParameterCollection outputs)
        {
            InvokePlugin(ref target, inputs, outputs, null, null);
        }

        /// <summary>
        /// Invokes the plug-in.
        /// </summary>
        /// <param name="target">The target entity</param>
        /// <param name="outputs"></param>
        /// <param name="preImage">The pre image</param>
        /// <param name="postImage">The post image</param>
        /// <param name="inputs"></param>
        protected void InvokePlugin(ref Entity target, ParameterCollection inputs, ParameterCollection outputs, Entity preImage, Entity postImage)
        {
            var testClass = Activator.CreateInstance(_childType) as IPlugin;

            var factoryMock = new Mock<IOrganizationServiceFactory>();
            var tracingServiceMock = new Mock<ITracingService>();
            var pluginContextMock = new Mock<IPluginExecutionContext>();
            var serviceProviderMock = new Mock<IServiceProvider>();

            //Organization Service Factory Mock
            var orgService = OrganizationService;

            factoryMock.Setup(t => t.CreateOrganizationService(It.IsAny<Guid>())).Returns(orgService);
            var factory = factoryMock.Object;
            
            //Tracing Service - Content written appears in output
            tracingServiceMock.Setup(t => t.Trace(It.IsAny<string>(), It.IsAny<object[]>()))
                .Callback<string, object[]>(MoqExtensions.WriteTrace);
            var tracingService = tracingServiceMock.Object;

            //Plug-in Context Mock
            pluginContextMock.Setup(t => t.InputParameters).Returns(inputs);
            pluginContextMock.Setup(t => t.OutputParameters).Returns(outputs);
            pluginContextMock.Setup(t => t.UserId).Returns(GetUser(orgService));
            pluginContextMock.Setup(t => t.PrimaryEntityName).Returns(target.LogicalName);

            var pluginContext = pluginContextMock.Object;

            //Service Provider Mock
            serviceProviderMock.Setup(t => t.GetService(It.Is<Type>(i => i == typeof (ITracingService))))
                .Returns(tracingService);
            serviceProviderMock.Setup(t => t.GetService(It.Is<Type>(i => i == typeof (IOrganizationServiceFactory))))
                .Returns(factory);
            serviceProviderMock.Setup(t => t.GetService(It.Is<Type>(i => i == typeof (IPluginExecutionContext))))
                .Returns(pluginContext);
            if (preImage != null)
                pluginContextMock.Setup(t => t.PreEntityImages)
                    .Returns(new EntityImageCollection() {new KeyValuePair<string, Entity>("preImage", preImage)});
            if (postImage != null)
                pluginContextMock.Setup(t => t.PostEntityImages)
                    .Returns(new EntityImageCollection() {new KeyValuePair<string, Entity>("postImage", postImage)});

            var serviceProvider = serviceProviderMock.Object;

            testClass?.Execute(serviceProvider);
        }

        protected Guid GetUser(IOrganizationService service)
        {
            var request = new WhoAmIRequest();

            var response = (WhoAmIResponse) service.Execute(request);

            return response.UserId;
        }
    }
}
