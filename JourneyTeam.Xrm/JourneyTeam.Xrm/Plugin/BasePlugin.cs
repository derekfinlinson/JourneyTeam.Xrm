using System;
using Microsoft.Xrm.Sdk;

namespace JourneyTeam.Xrm.Plugin
{
    public abstract class BasePlugin<TRequest, TResponse> : BasePluginHandler<ActionContext<TRequest, TResponse>>
        where TRequest : OrganizationRequest, new() where TResponse : OrganizationResponse, new()
    {
        protected BasePlugin()
        {
        }

        public override ActionContext<TRequest, TResponse> CreatePluginContext(IServiceProvider serviceProvider)
        {
            return new ActionContext<TRequest, TResponse>(serviceProvider, RegisteredEvents);
        }
    }
}
