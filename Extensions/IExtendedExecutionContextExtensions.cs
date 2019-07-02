using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata.Query;

namespace Xrm
{
    public static partial class IExtendedExecutionContextExtensions
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

        public static EntityReference RecordUrlToEntityReference(this IExtendedExecutionContext context, string url)
        {
            var uri = new Uri(url);

            int entityTypeCode = 0;
            var id = Guid.Empty;

            var parameters = uri.Query.TrimStart('?').Split('&');

            foreach (var param in parameters)
            {
                var nameValue = param.Split('=');

                switch (nameValue[0])
                {
                    case "etc":
                        entityTypeCode = int.Parse(nameValue[1]);
                        break;
                    case "id":
                        id = new Guid(nameValue[1]);
                        break;
                }

                if (entityTypeCode != 0 && id != Guid.Empty)
                {
                    break;
                }
            }

            if (id == Guid.Empty)
            {
                return null;
            }

            var retrieveMetadataChangesRequest = new RetrieveMetadataChangesRequest()
            {
                Query = new EntityQueryExpression()
                {
                    Criteria = new MetadataFilterExpression
                    {
                        Conditions =
                        {
                            new MetadataConditionExpression("ObjectTypeCode ", MetadataConditionOperator.Equals, entityTypeCode)
                        }
                    },
                    Properties = new MetadataPropertiesExpression
                    {
                        PropertyNames =
                        {
                            "LogicalName"
                        }
                    }
                }
            };

            var response = (RetrieveMetadataChangesResponse)context.SystemOrganizationService.Execute(retrieveMetadataChangesRequest);

            if (response.EntityMetadata.Count >= 1)
            {
                return new EntityReference(response.EntityMetadata[0].LogicalName, id);
            }

            return null;
        }
    }
}
