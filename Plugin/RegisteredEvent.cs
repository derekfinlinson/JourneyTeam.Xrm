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
        /// Expected <see href="SdkMessageProcessingStepMode" /> of the plugin
        /// </summary>
        /// <value></value>
        public SdkMessageProcessingStepMode Mode { get; set; }

        /// <summary>
        /// Action to execute for the event
        /// </summary>
        public Action<IExtendedPluginContext> Execute { get; set; }

        /// <summary>
        /// Constructor for a <see href="RegisteredEvent"> with a specific stage, mode and message using the default action
        /// </summary>
        /// <param name="stage">Pipeline stage</param>
        /// <param name="messageName">Message name</param>
        public RegisteredEvent(PipelineStage stage, SdkMessageProcessingStepMode mode, string messageName) : this(stage, mode, messageName, null, null) { }

        /// <summary>
        /// Constructor for a <see href="RegisteredEvent"> with a specific stage, mode, message and action
        /// </summary>
        /// <param name="stage">Pipeline stage</param>
        /// <param name="messageName">Message name</param>
        /// <param name="action">Action to execute</param>
        public RegisteredEvent(PipelineStage stage, SdkMessageProcessingStepMode mode, string messageName, Action<IExtendedPluginContext> action) : this(stage, mode, messageName, null, action) { }

        /// <summary>
        /// Constructor for a <see href="RegisteredEvent"> with a specific stage, mode, message and entity using the default action
        /// </summary>
        /// <param name="stage">Pipeline stage</param>
        /// <param name="messageName">Message name</param>
        /// <param name="entityLogicalName">Entity logical name</param>
        public RegisteredEvent(PipelineStage stage, SdkMessageProcessingStepMode mode, string messageName, string entityLogicalName) : this(stage, mode, messageName, entityLogicalName, null) { }

        /// <summary>
        /// Constructor for a <see href="RegisteredEvent"> with a specific stage, mode, message, entity and action
        /// </summary>
        /// <param name="stage">Pipeline stage</param>
        /// <param name="messageName">Message name</param>
        /// <param name="entityLogicalName">Entity logical name</param>
        /// <param name="action">Action to execute</param>
        public RegisteredEvent(PipelineStage stage, SdkMessageProcessingStepMode mode, string messageName, string entityLogicalName, Action<IExtendedPluginContext> action)
        {
            Stage = stage;
            Mode = mode;
            MessageName = messageName;
            EntityLogicalName = entityLogicalName;
            Execute = action;
        }
    }
}
