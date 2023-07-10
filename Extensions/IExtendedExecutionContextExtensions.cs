using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;

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

        /// <summary>
        /// Get an entity reference from a record URL
        /// </summary>
        /// <param name="context">IExtendedExecutionContext</param>
        /// <param name="url">Record URL</param>
        /// <returns>EntityReference</returns>
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

        /// <summary>
        /// Get typed environment variable
        /// </summary>
        /// <param name="context">IExtendedExecutionContext</param>
        /// <param name="variable">Schema name of variable to retrieve</param>
        /// <typeparam name="T">Type of variable</typeparam>
        /// <returns>Current or default variable value</returns>
        public static T GetEnvironmentVariable<T>(this IExtendedExecutionContext context, string variable)
        {
            var fetch = $@"
              <fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
	            <entity name='environmentvariabledefinition'>
		          <attribute name='defaultvalue' alias='default' />
                  <attribute name='type' alias='type' />
		          <filter type='and'>
		            <condition attribute='schemaname' operator='eq' value='{variable}' />
		          </filter>
		          <link-entity name='environmentvariablevalue' from='environmentvariabledefinitionid' to='environmentvariabledefinitionid' link-type='outer'>
		            <attribute name='value' alias='current' />
		          </link-entity>
	            </entity>
	          </fetch>";

            var entity = context.RetrieveMultiple(fetch).Entities.FirstOrDefault();

            if (entity == null)
            {
                return default(T);
            }

            if (entity.GetAttributeValue<OptionSetValue>("type").Value == 100000005) // Secret
            {
                var request = new OrganizationRequest("RetrieveEnvironmentVariableSecretValueRequest")
                {
                    ["EnvironmentVariableName"] = variable
                };

                var response = context.Execute(request);

                return (T)response.Results["EnvironmentVariableSecretValue"];
            }

            if (entity.GetAliasedValue<T>("current") != null)
            {
                return entity.GetAliasedValue<T>("current");
            }

            return entity.GetAliasedValue<T>("default");
        }

        /// <summary>
        /// Check if user is the SYSTEM account
        /// </summary>
        /// <param name="context">IExtendedExecutionContext</param>
        /// <param name="userId">User id to check</param>
        /// <returns>If user is the SYSTEM account</returns>
        public static bool IsSystemAccount(this IExtendedExecutionContext context, Guid userId)
        {
            var user = context.Retrieve("systemuser", userId, new ColumnSet("fullname"));

            return user.GetAttributeValue<string>("fullname") == "SYSTEM";
        }

        /// <summary>
        /// Check if user has a specific role
        /// /// </summary>
        /// <param name="context">IExtendedExecutionContext</param>
        /// <param name="userId">User id to check</param>
        /// <param name="role">Role or roles to check</param>
        /// <returns>User has the role</returns>
        public static bool UserHasRole(this IExtendedExecutionContext context, Guid userId, params string[] role)
        {
            var query = new QueryExpression("role")
            {
                ColumnSet = new ColumnSet("roleid")
            };

            query.Criteria.AddCondition("name", ConditionOperator.In, role);

            var link = query.AddLink("systemuserroles", "roleid", "roleid");
            link.LinkCriteria.AddCondition("systemuserid", ConditionOperator.Equal, userId);

            var results = context.RetrieveMultiple(query);

            return results.Entities.Count > 0;
        }

        /// <summary>
        /// Check if user has a specific role
        /// /// </summary>
        /// <param name="context">IExtendedExecutionContext</param>
        /// <param name="userId">User id to check</param>
        /// <param name="roleIds">Ids of roles to check</param>
        /// <returns>User has the role</returns>
        public static bool UserHasRole(this IExtendedExecutionContext context, Guid userId, params Guid[] roleIds)
        {
            var query = new QueryExpression("role")
            {
                ColumnSet = new ColumnSet("roleid")
            };

            query.Criteria.AddCondition("roleid", ConditionOperator.In, roleIds);

            var link = query.AddLink("systemuserroles", "roleid", "roleid");
            link.LinkCriteria.AddCondition("systemuserid", ConditionOperator.Equal, userId);

            var results = context.RetrieveMultiple(query);

            return results.Entities.Count > 0;
        }
    }
}
