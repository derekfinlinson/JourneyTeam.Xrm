using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Xrm
{
    public static class Extensions
    {
        #region IPluginExecutionContext Extensions

        public static RegisteredEvent GetEvent(this IPluginExecutionContext context, IEnumerable<RegisteredEvent> events)
        {
            return events.FirstOrDefault(e =>
                    (int)e.Stage == context.Stage
                    && (int)e.Mode == context.Mode
                    && e.MessageName == context.MessageName
                    && (string.IsNullOrWhiteSpace(e.EntityLogicalName) || e.EntityLogicalName == context.PrimaryEntityName)
            );
        }

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

        #endregion

        #region IExtendedExecutionContext Extensions

        public static T GetTargetEntity<T>(this IExtendedExecutionContext context) where T : Entity
        {
            if (!context.InputParameters.Contains("Target") || !(context.InputParameters["Target"] is Entity))
            {
                return null;
            }

            var entity = (Entity)context.InputParameters["Target"];

            return entity.ToEntity<T>();
        }

        public static T GetFirstPostImage<T>(this IExtendedExecutionContext context) where T : Entity
        {
            if (context.PostEntityImages.Count == 0)
            {
                return null;
            }

            return context.PostEntityImages.First().Value.ToEntity<T>();
        }

        public static T GetFirstPreImage<T>(this IExtendedExecutionContext context) where T : Entity
        {
            if (context.PreEntityImages.Count == 0)
            {
                return null;
            }

            return context.PreEntityImages.First().Value.ToEntity<T>();
        }

        #endregion

        #region EntityReference Extensions

        public static T ToEntity<T>(this EntityReference reference, ColumnSet columns, IOrganizationService service) where T : Entity
        {
            return (T)service.Retrieve(reference.LogicalName, reference.Id, columns);
        }

        #endregion
    }
}
