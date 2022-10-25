using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;

namespace Xrm
{
    public static partial class EntityReferenceExtensions
    {
        /// <summary>
        /// Retrieve entity from EntityReference
        /// </summary>
        /// <param name="reference">EntityReference</param>
        /// <param name="columns">Columns to retrieve</param>
        /// <param name="service">IOrganizationService</param>
        /// <typeparam name="T">Early bound entity type</typeparam>
        /// <returns></returns>
        public static T ToEntity<T>(this EntityReference reference, ColumnSet columns, IOrganizationService service) where T : Entity
        {
            if (reference == null)
            {
                return null;
            }
            
            return (T)service.Retrieve(reference.LogicalName, reference.Id, columns);
        }

        /// <summary>
        /// Retrieve entity metadata
        /// </summary>
        /// <param name="reference">EntityReference to retrieve metadata for</param>
        /// <param name="service">IOrganizationService</param>
        /// <returns>RetrieveEntityResponse</returns>
        public static RetrieveEntityResponse GetTableMetadata(this EntityReference reference, IOrganizationService service)
        {
            var request = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.Attributes,
                LogicalName = reference.LogicalName
            };

            return (RetrieveEntityResponse)service.Execute(request);
        }
    }
}
