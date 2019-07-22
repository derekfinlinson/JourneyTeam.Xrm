using System.Collections;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;

namespace Xrm
{
    public static partial class IOrganizationServiceExtensions
    {
        /// <summary>
        /// Creates a list of records.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="entities">A list of entity instances that contains the properties to set in the newly created records.</param>
        public static IEnumerable<Entity> Create(this IOrganizationService service, IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
            {
                entity.Id = service.Create(entity);
            }

            return entities;
        }

        /// <summary>
        /// Updates a list of existing records.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="entities">A list of entity instances that have one or more properties set to be updated in the records.</param>
        public static void Update(this IOrganizationService service, IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
            {
                service.Update(entity);
            }
        }

        /// <summary>
        /// Deletes a record.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="reference">EntityReference record to delete.</param>
        public static void Delete(this IOrganizationService service, EntityReference reference)
        {
            service.Delete(reference.LogicalName, reference.Id);
        }

        /// <summary>
        /// Deletes a list of records.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="entities">A list of entity reference records to delete.</param>
        public static void Delete(this IOrganizationService service, IEnumerable<EntityReference> references)
        {
            foreach (var reference in references)
            {
                service.Delete(reference);
            }
        }
        
        /// <summary>
        /// Creates a link between records.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="reference">EntityReference to disassociate.</param>
        /// <param name="relationship">The name of the relationship to be used to create the link.</param>
        /// <param name="relatedEntities">A collection of entity references (references to records) to be associated.</param>
        public static void Associate(this IOrganizationService service, EntityReference reference, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            service.Associate(reference.LogicalName, reference.Id, relationship, relatedEntities);
        }

        /// <summary>
        /// Deletes a link between records.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="reference">EntityReference to disassociate.</param>
        /// <param name="relationship">The name of the relationship to be used to remove the link.</param>
        /// <param name="relatedEntities">A collection of entity references (references to records) to be disassociated.</param>
        public static void Disassociate(this IOrganizationService service, EntityReference reference, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            service.Disassociate(reference.LogicalName, reference.Id, relationship, relatedEntities);
        }
    }
}