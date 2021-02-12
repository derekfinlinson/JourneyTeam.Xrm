namespace Xrm
{
    /// <summary>
    /// Mode of the plugin execution
    /// </summary>
    public enum SdkMessageProcessingStepMode
    {
        /// <summary>
        /// Synchronous: 0
        /// </summary>
        Synchronous = 0,

        /// <summary>
        /// Asynchronous: 1
        /// </summary>
        Asynchronous = 1,

        /// <summary>
        /// Custom API: 2
        /// </summary>
        CustomAPI = 2
    }
}