using Microsoft.Xrm.Sdk;

namespace Xrm
{
    public static partial class IOrganizationServiceExtensions
    {
        public static void Delete(this IOrganizationService service, EntityReference reference)
        {
            service.Delete(reference.LogicalName, reference.Id);
        }

        public static void Associate(this IOrganizationService service, EntityReference reference, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            service.Associate(reference.LogicalName, reference.Id, relationship, relatedEntities);
        }

        public static void Disassociate(this IOrganizationService service, EntityReference reference, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            service.Disassociate(reference.LogicalName, reference.Id, relationship, relatedEntities);
        }
    }
}