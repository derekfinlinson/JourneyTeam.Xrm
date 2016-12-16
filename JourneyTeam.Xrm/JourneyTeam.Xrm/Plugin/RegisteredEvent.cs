using System;

namespace JourneyTeam.Xrm.Plugin
{
    /// <summary>
    /// Plugin execution event
    /// </summary>
    public class RegisteredEvent
    {
        /// <summary>
        /// Expected pipeline stage of the plugin
        /// </summary>
        public PipelineStage Stage { get; set; }
        /// <summary>
        /// Expected MessageName of the plugin
        /// </summary>
        public string MessageName { get; set; }
        /// <summary>
        /// Expected EntityLogicalName of the plugin
        /// </summary>
        public string EntityLogicalName { get; set; }
        /// <summary>
        /// Action to execute for the event
        /// </summary>
        public Action<LocalPluginContext> Execute { get; set; }
    }
}
