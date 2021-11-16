# JourneyTeam.Xrm

Base classes and libraries for developing Dynamics 365 Plugins and Custom Workflow Activities.

## Includes

- [BasePlugin](Plugin/BasePlugin.cs) class
  - Configure [RegisteredEvent](Plugin/RegisteredEvent.cs) your plugin should execute for
  - Expanded [IPluginExecutionContext](Plugin/BasePluginContext.cs) with many useful features including implementing IOrganizationService
  - Create IOrganizationService for any user
- [BaseWorkflowActivity](WorkflowActivity/BaseWorkflowActivity.cs) class
  - Expanded [IWorkflowContext](WorkflowActivity/BaseWorkflowActivityContext.cs) with many useful features including implementing IOrganizationService  
  - Create IOrganizationService for any user 
- [ParallelOrganizationService](Parallel/ParallelOrganizationService.cs)
  - Execute IOrganizationService methods using Parallel.ForEach
- [MoneyExtensions](Extensions/MoneyExtensions.cs) methods
  - Add, Subtract, Multiple and Divide Money
- [FetchXml Builder](FetchXml/FetchXmlBuilder.cs)
- [IOrganizationServiceExtensions](Extensions/IOrganizationServiceExtensions.cs)
  - Overloads for Create, Update, Delete, Retrieve, RetrieveMultiple, Associate and Disassociate
- [EntityExtensions](Extensions/EntityExtensions.cs)
  - Clone entity
  - Get Aliased Values
- Other [Extension](Extensions) methods
