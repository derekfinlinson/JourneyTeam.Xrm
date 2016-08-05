/**
 * Intellisense file for the Xrm object
 * DO NOT INCLUDE IN CRM
 */

// ReSharper disable UnusedParameter
var Xrm = {};

(function(attribute) {
    attribute.getInitialValue = function() {};
    attribute.getOption = function(value) {};
    attribute.getOptions = function() {};
    attribute.getSelectedOption = function() {};
    attribute.getText = function() {};
    attribute.getAttributeType = function() {};
    attribute.getFormat = function() {};
    attribute.getIsDirty = function() {};
    attribute.getIsPartyList = function() {};
    attribute.getMaxLength = function() {};
    attribute.getName = function() {};
    attribute.getParent = Xrm.Page.data.entity;
    attribute.getUserPrivilege = function() {};
    attribute.getMax = function() {};
    attribute.getMin = function() {};
    attribute.getPrecision = function() {};
    attribute.addOnChange = function(reference) {};
    attribute.removeOnChange = function(reference) {};
    attribute.fireOnChange = function() {};
    attribute.getRequiredLevel = function() {};
    attribute.setRequiredLevel = function(requirementLevel) {};
    attribute.getSubmitMode = function() {};
    attribute.setSubmitMode = function(submitMode) {};
    attribute.getValue = function() {};
    attribute.setValue = function(value) {};
})(window.attribute = {});

(function(control) {
    control.showAutoComplete = function(object) {};
    control.hideAutoComplete = function() {};
    control.getDisabled = function() {};
    control.setDisabled = function(bool) {};
    control.getAttribute = function() { return Xrm.Page.getAttribute() };
    control.getControlType = function() {};
    control.getName = function() {};
    control.getParent = function() {};
    control.getValue = function() {};
    control.addOnKeyPress = function(reference) {};
    control.removeOnKeyPress = function(reference) {};
    control.fireOnKeyPress = function() {};
    control.getLabel = function() {};
    control.setLabel = function(label) {};
    control.addCustomFilter = function(filter, entityLogicalName) {};
    control.addCustomView = function(viewId, entityName, viewDisplayName, fetchXml, layoutXml, isDefault) {};
    control.getDefaultView = function() {};
    control.setDefaultView = function(viewGuid) {};
    control.addPreSearch = function(handler) {};
    control.removePreSearch = function(handler) {};
    control.setNotification = function(message, uniqueId) {};
    control.clearNotification = function(uniqudId) {};
    control.addOption = function(option, index) {};
    control.clearOptions = function() {};
    control.removeOption = function(number) {};
    control.setFocus = function() {};
    control.getShowTime = function() {};
    control.setShowTime = function(bool) {};
    control.refresh = function() {};
    control.getVisible = function() {};
    control.setVisible = function() {};
    control.getData = function() {};
    control.setData = function(string) {};
    control.getInitialUrl = function() {};
    control.getObject = function() {};
    control.getSrc = function() {};
    control.setSrc = function(string) {};
})(window.control = {});

(function(xrm) {
    xrm.getAttribute = function(logicalName) { return attribute; };
    xrm.getControl = function(logicalName) { return control; };
})(Xrm.Page = {});

(function (ui) {
    ui.close = function() {};
    ui.getCurrentControl = function() {};
    ui.getFormType = function() {};
    ui.clearFormNotification = function(uniqueId) {};
    ui.setFormNotification = function(message, level, uniqueId) {};
    ui.refreshRibbon = function() {};
    ui.getViewPortHeight = function() {};
    ui.getViewPortWidth = function() {};
})(Xrm.Page.ui = {});

(function (data) {
    data.refresh = function(save, successCallback, errorCallback) {};
    data.save = function(saveOptions, successCallback, errorCallback) {};
})(Xrm.Page.data = {});

(function (entity) {
    entity.getDataXml = function () { };
    entity.getEntityName = function () { };
    entity.getId = function () { };
    entity.getIsDirty = function () { };
    entity.addOnSave = function (functionReference) { };
    entity.removeOnSave = function (functionReference) { };
    entity.getPrimaryAttributeValue = function () { };
    entity.save = function (option) { };
})(Xrm.Page.data.entity = {});

(function (attributes) {
    attributes.forEach = function (functionReference) { };
    attributes.get = function () { return attribute; };
    attributes.getLength = function () { };
})(Xrm.Page.data.entity.attributes = {});

(function (process) {
    process.getEnabledProcesses = function(callbackFunction) {};
    process.getSelectedStage = function () { };
    process.addOnStageChange = function (handler) { };
    process.removeOnStageChange = function (handler) { };
    process.addOnStageSelected = function (handler) { };
    process.removeOnStageSelected = function (functionReference) { };
    process.moveNext = function (callbackFunction) { };
    process.movePrevious = function (callbackFunction) { };
})(Xrm.Page.data.process = {});

(function () {
    this.alertDialog = function(message, onCloseCallback) {};
    this.confirmDialog = function(message, yesCloseCallback, noCloseCallback) {};
    this.isActivityType = function(entityName) {};
    this.openEntityForm = function(name, id, parameters, windowOptions) {};
    this.openQuickCreate = function(entityLogicalName, createFromEntity, parameters, successCallback, errorCallback) {};
    this.openWebResource = function(webResourceName, webResourceData, width, height) {};
})(Xrm.Utility = {});

(function() {
    this.getClientUrl = function() {};
    this.getCurrentTheme = function() {};
    this.getIsAutoSaveEnabled = function() {};
    this.getOrgLcid = function() {};
    this.getOrgUniqueName = function() {};
    this.getQueryStringParameters = function() {};
    this.getTimeZoneOffsetMinutes = function() {};
    this.getUserId = function() {};
    this.getUserLcid = function() {};
    this.getUserName = function() {};
    this.getUserRoles = function() {};
    this.prependOrgName = function() {};
})(Xrm.Page.context = {});

(function() {
    this.getClient = function() {};
    this.getClientState = function() {};
    this.getFormFactor = function() {};
})(Xrm.Page.context.client = {});

(function(controls) {
    controls.forEach = function(functionReference) {};
    controls.get = function(value) { return control; };
    controls.getLength = function() {};
})(Xrm.Page.ui.controls = {});

(function(tabs) {

})(Xrm.Page.ui.tabs = {});