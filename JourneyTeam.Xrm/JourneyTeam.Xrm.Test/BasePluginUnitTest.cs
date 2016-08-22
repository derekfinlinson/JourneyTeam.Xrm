using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace JourneyTeam.Xrm.Test
{
    [TestClass]
    public class BasePluginUnitTest
    {
        private readonly Type _childType;

        public BasePluginUnitTest(Type childType)
        {
            _childType = childType;
        }

        /// <summary>
        /// Invokes the plug-in.
        /// </summary>
        /// <param name="target">The target entity</param>
        /// <param name="serviceMock">The mock Organization Service</param>
        public void InvokePlugin(ref Entity target, Mock<IOrganizationService> serviceMock)
        {
            InvokePlugin(ref target, null, null, serviceMock);
        }

        /// <summary>
        /// Invokes the plug-in.
        /// </summary>
        /// <param name="target">The target entity</param>
        /// <param name="preImage">The pre image</param>
        /// <param name="postImage">The post image</param>
        /// <param name="serviceMock">The mock Organization Service</param>
        public void InvokePlugin(ref Entity target, Entity preImage, Entity postImage, Mock<IOrganizationService> serviceMock)
        {
            var testClass = Activator.CreateInstance(_childType) as IPlugin;

            var factoryMock = new Mock<IOrganizationServiceFactory>();
            var tracingServiceMock = new Mock<ITracingService>();
            var pluginContextMock = new Mock<IPluginExecutionContext>();
            var serviceProviderMock = new Mock<IServiceProvider>();

            IOrganizationService service = serviceMock.Object;

            //Organization Service Factory Mock
            factoryMock.Setup(t => t.CreateOrganizationService(It.IsAny<Guid>())).Returns(service);
            var factory = factoryMock.Object;

            //Tracing Service - Content written appears in output
            tracingServiceMock.Setup(t => t.Trace(It.IsAny<string>(), It.IsAny<object[]>()))
                .Callback<string, object[]>(MoqExtensions.WriteTrace);
            var tracingService = tracingServiceMock.Object;

            //Parameter Collections
            ParameterCollection inputParameters = new ParameterCollection {{"Target", target}};
            ParameterCollection outputParameters = new ParameterCollection {{"id", Guid.NewGuid()}};

            //Plug-in Context Mock
            pluginContextMock.Setup(t => t.InputParameters).Returns(inputParameters);
            pluginContextMock.Setup(t => t.OutputParameters).Returns(outputParameters);
            pluginContextMock.Setup(t => t.UserId).Returns(Guid.NewGuid());
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
    }
}
