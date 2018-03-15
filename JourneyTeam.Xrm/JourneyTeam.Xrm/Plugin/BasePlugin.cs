using System;
using Microsoft.Xrm.Sdk;

namespace JourneyTeam.Xrm.Plugin
{
    public abstract class BasePlugin : BasePluginHandler
    {
        protected BasePlugin()
        {
        }

        public override IExtendedPluginContext CreatePluginContext(IServiceProvider serviceProvider)
        {
            return new LocalPluginContext(serviceProvider, this);
        }
    }
}
