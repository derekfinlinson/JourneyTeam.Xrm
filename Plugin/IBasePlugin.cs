using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace Xrm
{
    public interface IBasePlugin : IPlugin
    {
        /// <summary>
        /// List of <see href="RegisteredEvent" /> records for the plugin
        /// </summary>
        IEnumerable<RegisteredEvent> GetRegisteredEvents();

        /// <summary>
        /// Defalt execution method for the plugin
        /// </summary>
        /// <param name="context"><see href="IExtendedPluginContext" /> object for the current plug-in.</param>
        void ExecutePlugin(IExtendedPluginContext context);
    }
}
