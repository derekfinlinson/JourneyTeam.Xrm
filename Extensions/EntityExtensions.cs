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
        /// Retrieve entity metadata
        /// </summary>
        /// <param name="entity">Entity to retrieve metadata for</param>
        /// <param name="service">IOrganizationService</param>
        /// <returns>RetrieveEntityResponse</returns>
        public static RetrieveEntityResponse GetEntityMetadata(this Entity entity, IOrganizationService service)
        {
            var request = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.Attributes,
                LogicalName = entity.LogicalName
            };

            return (RetrieveEntityResponse)service.Execute(request);
        }

        /// <summary>
        /// Clone entity. Returned entity must be passed to IOrganizationService.Create
        /// </summary>
        /// <param name="entity">Entity to clone</param>
        /// <param name="service">IOrganizationService</param>
        /// <returns></returns>
        public static T CloneEntity<T>(this Entity entity, IOrganizationService service) where T : Entity
        {
            var clone = new Entity(entity.LogicalName);

            var response = entity.GetEntityMetadata(service);

            var attributes = response.EntityMetadata.Attributes
                .Where(a => a.IsValidForCreate == true && a.IsPrimaryId == false)
                .Select(a => a.LogicalName);

            foreach (var field in attributes)
            {
                if (entity.GetAttributeValue<object>(field) != null)
                {
                    clone[field] = entity[field];
                }
            }

            return clone.ToEntity<T>();
        }

        /// <summary>
        /// Clone entity with provided columns. Returned entity must be passed to IOrganizationService.Create
        /// </summary>
        /// <param name="entity">Entity to clone</param>
        /// <param name="columnSet">Columns to clone</param>
        /// <param name="service">IOrganizationService</param>
        /// <returns></returns>
        public static T CloneEntity<T>(this Entity entity, IOrganizationService service, ColumnSet columnSet) where T : Entity
        {
            var clone = new Entity(entity.LogicalName);

            foreach (var column in columnSet.Columns)
            {
                if (entity.GetAttributeValue<object>(column) != null)
                {
                    clone[column] = entity[column];
                }
            }

            return clone.ToEntity<T>();
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

        /// <summary>
        /// Merge attributes from an entity if current entity doesn't contain them
        /// </summary>
        /// <param name="baseEntity">Base entity</param>
        /// <param name="entity">Entity to merge</param>
        /// <returns>Merged entity</returns>
        public static T CoalesceEntityAttributes<T>(this T baseEntity, T entity) where T : Entity
        {
            if (entity == null)
            {
                return baseEntity;
            }

            // Create copy of baseEntity so we don't overwrite anything on accident
            var combined = baseEntity;

            foreach (var attribute in entity.Attributes.Where(a => !baseEntity.Attributes.Contains(a.Key)))
            {
                combined[attribute.Key] = attribute.Value;
            }

            return combined;
        }
    }
}