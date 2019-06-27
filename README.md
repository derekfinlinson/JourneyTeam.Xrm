# JourneyTeam.Xrm

[![Build Status](https://derekfinlinson.visualstudio.com/GitHub/_apis/build/status/derekfinlinson.JourneyTeam.Xrm)](https://derekfinlinson.visualstudio.com/GitHub/_build/latest?definitionId=6)

Base classes and libraries for developing Dynamics 365 Plugins and Custom Workflow Activities.

## Includes

- [BasePlugin](Plugin/BasePlugin.cs) class
  - Configure [RegisteredEvent](Plugin/RegisteredEvent.cs) your plugin should execute for
  - Expanded [IPluginExecutionContext](Plugin/BasePluginContext.cs) with lots of useful properties including implementing IOrganizationService
  - Create IOrganizationService for any user
- [BaseWorkflowActivity](WorkflowActivity/BaseWorkflowActivity.cs) class
  - Expanded [IWorkflowContext](WorkflowActivity/BaseWorkflowActivityContext.cs) with lots of useful properties including implementing IOrganizationService  
  - Create IOrganizationService for any user 
- [ParallelOrganizationService](Parallel/ParallelOrganizationService.cs)
  - Execute IOrganizationService methods using Parallel.ForEach
- [MoneyExtension](Extensions/MoneyExtensions.cs) methods
  - Add, Subtract, Multiple and Divide Money
- Other [Extension](Extensions) methods
