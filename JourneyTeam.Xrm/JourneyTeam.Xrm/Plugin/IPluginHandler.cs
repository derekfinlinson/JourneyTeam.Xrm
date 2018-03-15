using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace JourneyTeam.Xrm.Plugin
{
    public interface IPluginHandler : IPlugin
    {
        List<RegisteredEvent> RegisteredEvents { get; }

        void RegisterEvents();
    }
}
