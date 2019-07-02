using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Messages;

namespace Xrm
{
    public class BaseWorkflowActivityContext : IExtendedWorkflowContext
    {
        #region Private members for lazy loading

        private readonly IOrganizationServiceFactory _factory;
        private IOrganizationService _organizationService;
        private IOrganizationService _systemOrganizationService;
        private IOrganizationService _initiatedOrganizationService;
        private ITracingService _tracing;
        private IWorkflowContext _workflowContext;

        #endregion

        #region IWorkflowContext Properties

        public string StageName => WorkflowContext.StageName;

        public int WorkflowCategory => WorkflowContext.WorkflowCategory;

        public int WorkflowMode => WorkflowContext.WorkflowMode;

        public IWorkflowContext ParentContext => WorkflowContext.ParentContext;

        public int Mode => WorkflowContext.Mode;

        public int IsolationMode => WorkflowContext.IsolationMode;

        public int Depth => WorkflowContext.Depth;

        public string MessageName => WorkflowContext.MessageName;

        public string PrimaryEntityName => WorkflowContext.PrimaryEntityName;

        public Guid? RequestId => WorkflowContext.RequestId;

        public string SecondaryEntityName => WorkflowContext.SecondaryEntityName;

        public ParameterCollection InputParameters => WorkflowContext.InputParameters;

        public ParameterCollection OutputParameters => WorkflowContext.OutputParameters;

        public ParameterCollection SharedVariables => WorkflowContext.SharedVariables;

        public Guid UserId => WorkflowContext.UserId;

        public Guid InitiatingUserId => WorkflowContext.InitiatingUserId;

        public Guid BusinessUnitId => WorkflowContext.BusinessUnitId;

        public Guid OrganizationId => WorkflowContext.OrganizationId;

        public string OrganizationName => WorkflowContext.OrganizationName;

        public Guid PrimaryEntityId => WorkflowContext.PrimaryEntityId;

        public EntityImageCollection PreEntityImages => WorkflowContext.PreEntityImages;

        public EntityImageCollection PostEntityImages => WorkflowContext.PostEntityImages;

        public EntityReference OwningExtension => WorkflowContext.OwningExtension;

        public Guid CorrelationId => WorkflowContext.CorrelationId;

        public bool IsExecutingOffline => WorkflowContext.IsExecutingOffline;

        public bool IsOfflinePlayback => WorkflowContext.IsOfflinePlayback;

        public bool IsInTransaction => WorkflowContext.IsInTransaction;

        public Guid OperationId => WorkflowContext.OperationId;

        public DateTime OperationCreatedOn => WorkflowContext.OperationCreatedOn;

        #endregion

        #region IExtendedWorkflowContext Properties

        /// <summary>
        /// Name of the workflow activity the context is running against
        /// </summary>
        public string WorkflowTypeName { get; }

        /// <summary>
        /// Extends ActivityContext and provides additional functionality for CodeActivity
        /// </summary>
        public CodeActivityContext ActivityContext { get; }

        #endregion

        #region IExecutionContext Properties

        /// <summary>
        /// Primary entity from the context as an entity reference
        /// </summary>
        public EntityReference PrimaryEntity => new EntityReference(PrimaryEntityName, PrimaryEntityId);

        /// <summary>
        /// Workflow context
        /// </summary>
        public IWorkflowContext WorkflowContext => _workflowContext ?? (_workflowContext = ActivityContext.GetExtension<IWorkflowContext>());

        /// <summary>
        /// Provides logging run-time trace information for plug-ins
        /// </summary>
        public ITracingService TracingService => _tracing ?? (_tracing = (ITracingService)ActivityContext.GetExtension<ITracingService>());

        /// <summary>
        /// <see cref="IOrganizationService"/> using the user from the plugin context
        /// </summary>
        public IOrganizationService OrganizationService => _organizationService ?? (_organizationService = CreateOrganizationService(UserId));

        /// <summary>
        /// <see cref="IOrganizationService"/> using the SYSTEM user
        /// </summary>
        public IOrganizationService SystemOrganizationService => _systemOrganizationService ?? (_systemOrganizationService = CreateOrganizationService(null));

        /// <summary>
        /// <see cref="IOrganizationService"/> using the initiating user from the plugin context
        /// </summary>
        public IOrganizationService InitiatingUserOrganizationService => _initiatedOrganizationService ?? (_initiatedOrganizationService = CreateOrganizationService(InitiatingUserId));

        #endregion

        public BaseWorkflowActivityContext(CodeActivityContext activityContext, BaseWorkflowActivity workflow)
        {
            // Obtain the activity execution context
            ActivityContext = activityContext ?? throw new ArgumentNullException(nameof(activityContext));

            // Obtain the organization factory service from the activity context
            _factory = activityContext.GetExtension<IOrganizationServiceFactory>();

            WorkflowTypeName = workflow.GetType().FullName;
        }

        #region IOrganizationService methods

        public Guid Create(Entity entity)
        {
            return OrganizationService.Create(entity);
        }

        public Entity Retrieve(string entityName, Guid id, ColumnSet columnSet)
        {
            return OrganizationService.Retrieve(entityName, id, columnSet);
        }

        public void Update(Entity entity)
        {
            OrganizationService.Update(entity);
        }

        public void Delete(string entityName, Guid id)
        {
            OrganizationService.Delete(entityName, id);
        }

        public OrganizationResponse Execute(OrganizationRequest request)
        {
            return OrganizationService.Execute(request);
        }

        public void Associate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            OrganizationService.Associate(entityName, entityId, relationship, relatedEntities);
        }

        public void Disassociate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
            OrganizationService.Disassociate(entityName, entityId, relationship, relatedEntities);
        }

        public EntityCollection RetrieveMultiple(QueryBase query)
        {
            return OrganizationService.RetrieveMultiple(query);
        }

        #endregion

        /// <summary>
        /// Create CRM Organization Service for a specific user id
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>CRM Organization Service</returns>
        /// <remarks>Useful for impersonation</remarks>
        public IOrganizationService CreateOrganizationService(Guid? userId)
        {
            return _factory.CreateOrganizationService(userId);
        }

        public EntityReference RecordUrlToEntityReference(string url)
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

            var response = (RetrieveMetadataChangesResponse)SystemOrganizationService.Execute(retrieveMetadataChangesRequest);

            if (response.EntityMetadata.Count >= 1)
            {
                return new EntityReference(response.EntityMetadata[0].LogicalName, id);
            }

            return null;
        }

        /// <summary>
        /// Writes a trace message to the CRM trace log.
        /// </summary>
        /// <param name="format">Message format</param>
        /// <param name="args">Message format arguments</param>
        public void Trace(string format, params object[] args)
        {
            TracingService.Trace(format, args);
        }
    }
}
