using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using JourneyTeam.Xrm.Plugin;

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
        /// <param name="inputs"></param>
        /// <param name="outputs"></param>
        /// <param name="serviceMock">The mock Organization Service</param>
        /// <param name="registeredEvent"></param>
        public void InvokePlugin(ref Entity target, ParameterCollection inputs, ParameterCollection outputs, Mock<IOrganizationService> serviceMock, RegisteredEvent registeredEvent)
        {
            InvokePlugin(ref target, inputs, outputs, null, null, serviceMock, registeredEvent);
        }

        /// <summary>
        /// Invokes the plug-in.
        /// </summary>
        /// <param name="target">The target entity</param>
        /// <param name="inputs"></param>
        /// <param name="outputs"></param>
        /// <param name="preImage">The pre image</param>
        /// <param name="postImage">The post image</param>
        /// <param name="serviceMock">The mock Organization Service</param>
        /// <param name="registeredEvent"></param>
        public void InvokePlugin(ref Entity target, ParameterCollection inputs, ParameterCollection outputs, Entity preImage, Entity postImage, Mock<IOrganizationService> serviceMock, RegisteredEvent registeredEvent)
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

            //Plug-in Context Mock
            pluginContextMock.Setup(t => t.InputParameters).Returns(inputs);
            pluginContextMock.Setup(t => t.OutputParameters).Returns(outputs);
            pluginContextMock.Setup(t => t.UserId).Returns(Guid.NewGuid());
            pluginContextMock.Setup(t => t.PrimaryEntityName).Returns(target.LogicalName);
            pluginContextMock.Setup(t => t.PrimaryEntityId).Returns(target.Id);
            pluginContextMock.Setup(t => t.MessageName).Returns(registeredEvent.MessageName);
            pluginContextMock.Setup(t => t.Stage).Returns((int)registeredEvent.Stage);

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
                    .Returns(new EntityImageCollection() {new KeyValuePair<string, Entity>("PreImage", preImage)});
            if (postImage != null)
                pluginContextMock.Setup(t => t.PostEntityImages)
                    .Returns(new EntityImageCollection() {new KeyValuePair<string, Entity>("PostImage", postImage)});

            var serviceProvider = serviceProviderMock.Object;

            testClass?.Execute(serviceProvider);
        }
    }
}
