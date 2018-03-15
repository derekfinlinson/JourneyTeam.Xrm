using System;
using System.Collections.Generic;

namespace JourneyTeam.Xrm.Plugin
{
    public class RequestContext<TRequest, TResponse> : LocalPluginContext
        where TRequest : Microsoft.Xrm.Sdk.OrganizationRequest, new()
        where TResponse : Microsoft.Xrm.Sdk.OrganizationResponse, new()
    {
        public TRequest Request { get; set; }
        public TResponse Response { get; set; }

        public RequestContext(IServiceProvider serviceProvider, IPluginHandler plugin) : base(serviceProvider, plugin)
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
