using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Xrm
{
    public static class IExtendedExecutionContextExtensions
    {
        /// <summary>
        /// Get Target entity from InputParameters
        /// </summary>
        /// <param name="context">IExtendedExecutionContext</param>
        /// <typeparam name="T">Early bound entity type</typeparam>
        /// <returns></returns>
        public static T GetTargetEntity<T>(this IExtendedExecutionContext context) where T : Entity
        {
            if (!context.InputParameters.Contains("Target") || !(context.InputParameters["Target"] is Entity))
            {
                return null;
            }

            var entity = (Entity)context.InputParameters["Target"];

            return entity.ToEntity<T>();
        }

        /// <summary>
        /// Get first post image from PostEntityImages
        /// </summary>
        /// <param name="context">IExtendedExecutionContext</param>
        /// <typeparam name="T">Early bound entity type</typeparam>
        /// <returns></returns>
        public static T GetFirstPostImage<T>(this IExtendedExecutionContext context) where T : Entity
        {
            if (context.PostEntityImages.Count == 0)
            {
                return null;
            }

            return context.PostEntityImages.First().Value.ToEntity<T>();
        }

        /// <summary>
        /// Get first pre image from PreEntityImages
        /// </summary>
        /// <param name="context">IExtendedExecutionContext</param>
        /// <typeparam name="T">Early bound entity type</typeparam>
        /// <returns></returns>
        public static T GetFirstPreImage<T>(this IExtendedExecutionContext context) where T : Entity
        {
            if (context.PreEntityImages.Count == 0)
            {
                return null;
            }

            return context.PreEntityImages.First().Value.ToEntity<T>();
        }
    }
}
