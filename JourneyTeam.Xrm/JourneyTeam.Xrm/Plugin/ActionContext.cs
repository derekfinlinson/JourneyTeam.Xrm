using System;
using System.Collections.Generic;

namespace JourneyTeam.Xrm.Plugin
{
    public class ActionContext<TRequest, TResponse> : LocalPluginContext
        where TRequest : Microsoft.Xrm.Sdk.OrganizationRequest, new()
        where TResponse : Microsoft.Xrm.Sdk.OrganizationResponse, new()
    {
        public TRequest Request { get; set; }
        public TResponse Response { get; set; }

        public ActionContext(IServiceProvider serviceProvider, IEnumerable<RegisteredEvent> events) : base(serviceProvider, events)
        {
            // Set Request and Response
            Request = new TRequest
            {
                Parameters = PluginExecutionContext.InputParameters
            };

            Response = new TResponse
            {
                Results = PluginExecutionContext.OutputParameters
            };
        }
    }
}
