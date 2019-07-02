using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;

namespace Xrm
{
    public static partial class IPluginExecutionExtensions
    {
        /// <summary>
        /// Get matching RegisteredEvent from the plugin execution
        /// </summary>
        /// <param name="context">IPluginExecutionContext</param>
        /// <param name="events">List of registered events</param>
        /// <returns></returns>
        public static RegisteredEvent GetEvent(this IPluginExecutionContext context, IEnumerable<RegisteredEvent> events)
        {
            return events.FirstOrDefault(e =>
                    (int)e.Stage == context.Stage
                    && (int)e.Mode == context.Mode
                    && e.MessageName == context.MessageName
                    && (string.IsNullOrWhiteSpace(e.EntityLogicalName) || e.EntityLogicalName == context.PrimaryEntityName)
            );
        }

        /// <summary>
        /// Get typed shared variable from plugin context
        /// </summary>
        /// <param name="context">IPluginExecutionContext</param>
        /// <param name="key">Shared variable key</param>
        /// <typeparam name="T">Type of variable</typeparam>
        /// <returns></returns>
        public static T GetSharedVariable<T>(this IPluginExecutionContext context, string key)
        {
            while (context != null)
            {
                if (context.SharedVariables.ContainsKey(key))
                {
                    return (T)(context.SharedVariables[key] ?? default(T));
                }

                context = context.ParentContext;
            }

            return default(T);
        }

        /// <summary>
        /// Get shared variable from plugin context
        /// </summary>
        /// <param name="context">IPluginExecutionContext</param>
        /// <param name="key">Shared variable key</param>
        /// <returns></returns>
        public static object GetSharedVariable(this IPluginExecutionContext context, string key)
        {
            while (context != null)
            {
                if (context.SharedVariables.ContainsKey(key))
                {
                    return context.SharedVariables[key];
                }

                context = context.ParentContext;
            }

            return null;
        }
    }
}
