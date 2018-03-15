using JourneyTeam.Xrm.Plugin;
using Microsoft.Xrm.Sdk.Messages;

namespace JourneyTeam.Xrm.Sample
{
    public class SamplePlugin : BasePlugin<CreateRequest, CreateResponse>
    {
        public override void RegisterEvents()
        {
            RegisteredEvents.Add(new RegisteredEvent(PipelineStage.PostOperation, "Create", "contact"));
        }

        protected override void ExecutePlugin(ActionContext<CreateRequest, CreateResponse> context)
        {
            // Get target
            var target = context.Request.Target;
        }
    }
}
