using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Moq;

namespace JourneyTeam.Xrm.Test.Plugin
{
    public class PluginContextBuilder
    {
        protected Mock<IPluginExecutionContext> Context { get; set; }

        public PluginContextBuilder()
        {
            Context = new Mock<IPluginExecutionContext>();
        }

        public PluginContextBuilder WithInputs(ParameterCollection inputs)
        {
            Context.Setup(t => t.InputParameters).Returns(inputs);
            return this;
        }

        public PluginContextBuilder WithOutputs(ParameterCollection outputs)
        {
            Context.Setup(t => t.OutputParameters).Returns(outputs);
            return this;
        }

        public PluginContextBuilder WithRegisteredEvent(string message, int stage)
        {
            Context.Setup(t => t.MessageName).Returns(message);
            Context.Setup(t => t.Stage).Returns(stage);
            return this;
        }

        public PluginContextBuilder WithUser(Guid id)
        {
            Context.Setup(t => t.UserId).Returns(id);
            WithInitiatingUser(id);

            return this;
        }

        public PluginContextBuilder WithInitiatingUser(Guid id)
        {
            Context.Setup(t => t.InitiatingUserId).Returns(id);
            return this;
        }

        public PluginContextBuilder WithTarget(string logicalName, Guid id)
        {
            Context.Setup(t => t.PrimaryEntityId).Returns(id);
            Context.Setup(t => t.PrimaryEntityName).Returns(logicalName);
            return this;
        }

        public PluginContextBuilder WithPreImage(Entity preImage, string imageAlias)
        {
            Context.Setup(t => t.PreEntityImages)
                    .Returns(new EntityImageCollection() { new KeyValuePair<string, Entity>(imageAlias, preImage) });
            return this;
        }

        public PluginContextBuilder WithPostImage(Entity postImage, string imageAlias)
        {
            Context.Setup(t => t.PostEntityImages)
                    .Returns(new EntityImageCollection() { new KeyValuePair<string, Entity>(imageAlias, postImage) });
            return this;
        }

        public IPluginExecutionContext Build()
        {
            return Context.Object;
        }
    }
}
