using JourneyTeam.Xrm.Plugin;
using Microsoft.Xrm.Sdk.Messages;

namespace JourneyTeam.Xrm.Sample
{
    public class SamplePlugin : BasePlugin
    {
        public override void RegisterEvents()
        {
            RegisteredEvents.Add(new RegisteredEvent(PipelineStage.PostOperation, "Create", "contact"));
        }

        protected override void ExecutePlugin(IExtendedPluginContext context)
        {
            // Create request context to get correct OrganzationRequest/Response
            var requestContext = (RequestContext<CreateRequest, CreateResponse>)context;

            // Get target
            var target = requestContext.Request.Target;
        }
    }
}
