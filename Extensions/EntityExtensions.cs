using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;

namespace Xrm
{
    public static partial class EntityExtensions
    {
        /// <summary>
        /// Clone entity. Returned entity must be passed to IOrganizationService.Create
        /// </summary>
        /// <param name="entity">Entity to clone</param>
        /// <param name="service">IOrganizationService</param>
        /// <returns></returns>
        public static Entity CloneEntity(this Entity entity, IOrganizationService service)
        {
            var clone = new Entity(entity.LogicalName);

            var request = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.Attributes,
                LogicalName = entity.LogicalName
            };

            var response = (RetrieveEntityResponse)service.Execute(request);

            var attributes = response.EntityMetadata.Attributes.Where(a => a.IsValidForCreate == true && a.IsPrimaryId == false).Select(a => a.LogicalName);

            foreach (var field in attributes)
            {
                if (entity.GetAttributeValue<object>(field) != null)
                {
                    clone[field] = entity[field];
                }
            }

            return clone;
        }

        /// <summary>
        /// Clone entity with provided columns. Returned entity must be passed to IOrganizationService.Create
        /// </summary>
        /// <param name="entity">Entity to clone</param>
        /// <param name="columnSet">Columns to clone</param>
        /// <param name="service">IOrganizationService</param>
        /// <returns></returns>
        public static Entity CloneEntity(this Entity entity, IOrganizationService service, ColumnSet columnSet)
        {
            var clone = new Entity(entity.LogicalName);

            foreach (var column in columnSet.Columns)
            {
                if (entity.GetAttributeValue<object>(column) != null)
                {
                    clone[column] = entity[column];
                }
            }

            return clone;
        }

        /// <summary>
        /// Get AliasedValue
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="attributeName">Attribute logical name</param>
        /// <typeparam name="T">Type of attribute</typeparam>
        /// <returns></returns>
        public static T GetAliasedValue<T>(this Entity entity, string attributeName)
        {
            if (!entity.Contains(attributeName))
            {
                if (typeof(T) == typeof(Money))
                {
                    return (T)(object)new Money(0);
                }
                
                return default(T);
            }

            var attribute = entity[attributeName];

            if (!(attribute is AliasedValue aliased))
            {
                throw new InvalidCastException($"Attribute {attributeName} is of type {attribute.GetType().Name}");
            }

            if (aliased?.Value == null)
            {
                if (typeof(T) == typeof(Money))
                {
                    return (T)(object)new Money(0);
                }
                
                return default(T);
            }
            
            try
            {
                return (T)aliased.Value;
            }
            catch (InvalidCastException)
            {
                throw new InvalidCastException($"Unable to cast {attributeName} from {aliased.Value.GetType().Name} to {typeof(T).Name}");
            }
        }
    }
}