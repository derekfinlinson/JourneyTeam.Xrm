using System;

namespace Xrm
{
    /// <summary>
    /// Plugin execution event
    /// </summary>
    public class RegisteredEvent
    {
        /// <summary>
        /// Expected <see href="PipelineStage" /> of the plugin
        /// </summary>
        public PipelineStage Stage { get; set; }

        /// <summary>
        /// Expected SDK Message of the plugin
        /// </summary>
        public string MessageName { get; set; }
        
        /// <summary>
        /// Expected EntityLogicalName of the plugin
        /// </summary>
        public string EntityLogicalName { get; set; }
        
        /// <summary>
        /// Action to execute for the event
        /// </summary>
        public Action<IExtendedPluginContext> Execute { get; set; }

        /// <summary>
        /// Constructor for a <see href="RegisteredEvent"> for a certain message and all entities
        /// </summary>
        /// <param name="stage">Pipeline stage</param>
        /// <param name="messageName">Message name</param>
        public RegisteredEvent(PipelineStage stage, string messageNage) : this(stage, messageNage, null, null) {}

        /// <summary>
        /// Constructor for a <see href="RegisteredEvent"> for a certain message and all entities with a specific action
        /// </summary>
        /// <param name="stage">Pipeline stage</param>
        /// <param name="messageName">Message name</param>
        /// <param name="action">Action to execute</param>
        public RegisteredEvent(PipelineStage stage, string messageName, Action<IExtendedPluginContext> action) : this(stage, messageName, null, action) {}

        /// <summary>
        /// Constructor for a <see href="RegisteredEvent"> for a certain message and a certain entity using the default action
        /// </summary>
        /// <param name="stage">Pipeline stage</param>
        /// <param name="messageName">Message name</param>
        /// <param name="entityLogicalName">Entity logical name</param>
        public RegisteredEvent(PipelineStage stage, string messageName, string entityLogicalName) : this(stage, messageName, entityLogicalName, null) {}

        /// <summary>
        /// Constructor for a <see href="RegisteredEvent"> for a certain message and a certain entity with a specific action
        /// </summary>
        /// <param name="stage">Pipeline stage</param>
        /// <param name="messageName">Message name</param>
        /// <param name="entityLogicalName">Entity logical name</param>
        /// <param name="action">Action to execute</param>
        public RegisteredEvent(PipelineStage stage, string messageName, string entityLogicalName, Action<IExtendedPluginContext> action)
        {
            Stage = stage;
            MessageName = messageName;
            EntityLogicalName = entityLogicalName;
            Execute = action;
        }
    }
}
