using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Moq;

namespace JourneyTeam.Xrm.Test.WorkflowActivity
{
    public class WorkflowContextBuilder
    {
        public Mock<IWorkflowContext> Context { get; set; }

        public WorkflowContextBuilder()
        {
            Context = new Mock<IWorkflowContext>();
            Context.Setup(t => t.CorrelationId).Returns(Guid.NewGuid());
        }

        public WorkflowContextBuilder WithInputs(ParameterCollection inputs)
        {
            Context.Setup(t => t.InputParameters).Returns(inputs);
            return this;
        }

        public WorkflowContextBuilder WithUser(Guid id)
        {
            Context.Setup(t => t.UserId).Returns(id);
            WithInitiatingUser(id);
            return this;
        }

        public WorkflowContextBuilder WithInitiatingUser(Guid id)
        {
            Context.Setup(t => t.InitiatingUserId).Returns(id);
            return this;
        }

        public WorkflowContextBuilder WithTarget(string logicalName, Guid id)
        {
            Context.Setup(t => t.PrimaryEntityName).Returns(logicalName);
            Context.Setup(t => t.PrimaryEntityId).Returns(id);
            return this;
        }

        public IWorkflowContext Build()
        {
            return Context.Object;
        }
    }
}
