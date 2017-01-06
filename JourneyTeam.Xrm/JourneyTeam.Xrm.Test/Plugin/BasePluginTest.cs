using System;
using Microsoft.Xrm.Sdk;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace JourneyTeam.Xrm.Test.Plugin
{
    [TestClass]
    public class BasePluginTest
    {
        private readonly Type _childType;

        public BasePluginTest(Type childType)
        {
            _childType = childType;
        }

        /// <summary>
        /// Invokes the plug-in.
        /// </summary>
        /// <param name="context">IPluginExecutionContext</param>
        /// <param name="service">The Organization Service, either mocked or real</param>
        public void InvokePlugin(IPluginExecutionContext context, IOrganizationService service)
        {
            var testClass = Activator.CreateInstance(_childType) as IPlugin;

            var factoryMock = new Mock<IOrganizationServiceFactory>();
            var tracingServiceMock = new Mock<ITracingService>();
            var serviceProviderMock = new Mock<IServiceProvider>();

            // Organization Service Factory Mock
            factoryMock.Setup(t => t.CreateOrganizationService(It.IsAny<Guid>())).Returns(service);
            var factory = factoryMock.Object;

            // Tracing Service - Content written appears in output
            tracingServiceMock.Setup(t => t.Trace(It.IsAny<string>(), It.IsAny<object[]>()))
                .Callback<string, object[]>(MoqExtensions.WriteTrace);

            // Service Provider Mock
            serviceProviderMock.Setup(t => t.GetService(It.Is<Type>(i => i == typeof (ITracingService))))
                .Returns(tracingServiceMock.Object);
            serviceProviderMock.Setup(t => t.GetService(It.Is<Type>(i => i == typeof (IOrganizationServiceFactory))))
                .Returns(factory);
            serviceProviderMock.Setup(t => t.GetService(It.Is<Type>(i => i == typeof (IPluginExecutionContext))))
                .Returns(context);

            var serviceProvider = serviceProviderMock.Object;

            testClass?.Execute(serviceProvider);
        }
    }
}
