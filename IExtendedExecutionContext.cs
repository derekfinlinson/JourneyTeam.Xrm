using System;
using Microsoft.Xrm.Sdk;

namespace Xrm
{
    public interface IExtendedExecutionContext : IExecutionContext, ITracingService, IOrganizationService
    {
        /// <summary>
        /// Provides logging run-time trace information
        /// </summary>
        ITracingService TracingService { get; }

        /// <summary>
        /// <see cref="IOrganizationService"/> using the user from the context
        /// </summary>
        IOrganizationService OrganizationService { get; }

        /// <summary>
        /// <see cref="IOrganizationService"/> using the SYSTEM user
        /// </summary>
        IOrganizationService SystemOrganizationService { get; }

        /// <summary>
        /// <see cref="IOrganizationService"/> using the initiating user from the context
        /// </summary>
        IOrganizationService InitiatingUserOrganizationService { get; }

        /// <summary>
        /// The Managed Identity Service of the plugin, used to retrieve tokens for Azure resources.
        /// </summary>
        IManagedIdentityService ManagedIdentityService { get; }

        /// <summary>
        /// Gets an entity reference from the context PrimaryEntityName and PrimaryEntityId
        /// </summary>
        EntityReference PrimaryEntity { get; }

        /// <summary>
        /// Create CRM Organization Service for a specific user id
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>CRM Organization Service</returns>
        /// <remarks>Useful for impersonation</remarks>
        IOrganizationService CreateOrganizationService(Guid? userId);

        /// <summary>
        /// Provides an in memory cache
        /// </summary>
        /// <value></value>
        DataverseCache Cache { get; }
    }
}