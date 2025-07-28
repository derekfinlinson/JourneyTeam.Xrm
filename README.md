# JourneyTeam.Xrm

Base classes and library for developing Microsoft Dataverse Plugins 
and Custom Workflow Activities.

| NuGet |
| ----- |
| [![#](https://img.shields.io/nuget/v/JourneyTeam.Xrm.svg)](https://www.nuget.org/packages/JourneyTeam.Xrm/) |

## Releases

- Versions 2.x supports CRM 2011+
- Versions 3.x supports CRM 2016+

## Features

### [BasePlugin](Plugin/BasePlugin.cs)
- Configure [RegisteredEvent](Plugin/RegisteredEvent.cs) your plugin should execute for
- Specify which method each [RegisteredEvent](Plugin/RegisteredEvent.cs) should execute
- Expanded [IPluginExecutionContext](Plugin/BasePluginContext.cs) with many useful features
- [IPluginExecutionContext](Plugin/BasePluginContext.cs) implements IOrganizationService
- Create IOrganizationService for any user

### [BaseWorkflowActivity](WorkflowActivity/BaseWorkflowActivity.cs)
- Expanded [IWorkflowContext](WorkflowActivity/BaseWorkflowActivityContext.cs) with many useful features
- [IWorkflowContext](WorkflowActivity/BaseWorkflowActivityContext.cs) implements IOrganizationService
- Create IOrganizationService for any user

### [MoneyExtensions](Extensions/MoneyExtensions.cs) methods
- Add
- Subtract
- Multiply
- Divide

### [FetchXml Builder](FetchXml/FetchXmlBuilder.cs)
- Fluent builder class for FetchXML queries
- Extension method to convert FilterExpression to a FilterBuilder

### [IOrganizationServiceExtensions](Extensions/IOrganizationServiceExtensions.cs)
- Overloads for Create, Update, Delete, Retrieve, RetrieveMultiple, Associate and Disassociate
- Upload file to a file type column

### [EntityExtensions](Extensions/EntityExtensions.cs)
- Clone entity
- Get Aliased Values

### [JsonSerializer](Utilities/JsonSerializer.cs)
- Serialize object to JSON string
- Deserialize JSON string to object

### Other [Extension](Extensions) methods
- Convert EntityReference to Entity
- Get EnvironmentVariable
- Check if UserHasRole
- Convert RecordUrl to EntityReference
