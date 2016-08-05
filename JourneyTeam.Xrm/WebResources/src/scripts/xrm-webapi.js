/// <reference path="../../typings/globals/xrm/index.d.ts" />
"use strict";
class WebApi {
    static getClientUrl() {
        let context;
        if (typeof GetGlobalContext != "undefined") {
            context = GetGlobalContext();
        }
        else {
            if (typeof Xrm != "undefined") {
                context = Xrm.Page.context;
            }
            else {
                throw new Error("Context is not available.");
            }
        }
        return context.getClientUrl() + "/api/data/v8.0/";
    }
    static errorHandler(response) {
        try {
            return JSON.parse(response).error;
        }
        catch (e) {
            return new Error("Unexpected Error");
        }
    }
    static isGuid(parameter, message) {
        parameter = parameter.replace(/[{}]/g, "");
        if (typeof parameter == "string") {
            if (/^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/.test(parameter) ===
                false) {
                throw new Error(message);
            }
        }
        else {
            throw new Error(message);
        }
        return parameter;
    }
    static getXmlHttpRequest(method, enitySet, options) {
        const req = new XMLHttpRequest();
        req.open(method, this.getClientUrl() + enitySet + options, true);
        req.setRequestHeader("Accept", "application/json");
        req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        req.setRequestHeader("OData-MaxVersion", "4.0");
        req.setRequestHeader("OData-Version", "4.0");
        return req;
    }
    /**
    * @description Retrieve a record
    * @param {string} id
    * @param {string} entitySet
    * @param {string} options
    * @returns {Promise}
    */
    static retrieveRecord(id, entitySet, options) {
        const vm = this;
        id = vm.isGuid(id, "Jt.WebApi.retrieveRecord id parameter must be a valid guid");
        var req = vm.getXmlHttpRequest("GET", entitySet, `(${id})${options}`);
        return new Promise((resolve, reject) => {
            req.onreadystatechange = function () {
                if (this.readyState === 4 /* complete */) {
                    req.onreadystatechange = null;
                    if (this.status === 200) {
                        resolve(JSON.parse(this.response));
                    }
                    else {
                        reject(vm.errorHandler(JSON.parse(this.response)));
                    }
                }
            };
            req.send();
        });
    }
    ;
    /**
     * @description Retrieve records
     * @param {} entitySet
     * @param {} options
     * @returns {}
     */
    static retrieveRecords(entitySet, options) {
        var vm = this;
        var req = vm.getXmlHttpRequest("GET", entitySet, options);
        return new Promise((resolve, reject) => {
            req.onreadystatechange = function () {
                if (this.readyState === 4 /* complete */) {
                    req.onreadystatechange = null;
                    if (this.status === 200) {
                        resolve(JSON.parse(this.response));
                    }
                    else {
                        reject(vm.errorHandler(JSON.parse(this.response)));
                    }
                }
            };
            req.send();
        });
    }
    ;
    /** @description Create a new entity
    * @param {string} entitySet The name of the entity set for the type of entity you want to create (eq, accounts).
    * @param {object} entity An object with the properties for the entity you want to create.
    */
    static createRecord(entitySet, entity) {
        var vm = this;
        var req = vm.getXmlHttpRequest("POST", entitySet, "");
        return new Promise((resolve, reject) => {
            req.onreadystatechange = function () {
                if (this.readyState === 4 /* complete */) {
                    req.onreadystatechange = null;
                    if (this.status === 200) {
                        resolve(this.getResponseHeader("OData-EntityId"));
                    }
                    else {
                        reject(vm.errorHandler(this.response));
                    }
                }
            };
            req.send(JSON.stringify(entity));
        });
    }
    ;
    /**
     * @description
     * @param {} entitySet
     * @param {} id
     * @param {} entity
     * @returns {}
     */
    static updateRecord(entitySet, id, entity) {
        var vm = this;
        id = vm.isGuid(id, "Jt.WebApi.updateRecord id parameter must be a valid guid");
        var req = vm.getXmlHttpRequest("PATCH", entitySet, "(" + id + ")");
        return new Promise((resolve, reject) => {
            req.onreadystatechange = function () {
                if (this.readyState === 4 /* complete */) {
                    req.onreadystatechange = null;
                    if (this.status === 204) {
                        resolve();
                    }
                    else {
                        reject(vm.errorHandler(this.response));
                    }
                }
            };
            req.send(JSON.stringify(entity));
        });
    }
    ;
    /**
     * @description
     * @param {} entitySet
     * @param {} id
     * @param {} attribute
     * @param {} value
     * @returns {}
     */
    static updateProperty(entitySet, id, attribute, value) {
        const vm = this;
        id = vm.isGuid(id, "Jt.WebApi.updateProperty id parameter must be a valid guid");
        var req = vm.getXmlHttpRequest("PUT", entitySet, "(" + id + ")");
        return new Promise((resolve, reject) => {
            req.onreadystatechange = function () {
                if (this.readyState === 4 /* complete */) {
                    req.onreadystatechange = null;
                    if (this.status === 204) {
                        resolve();
                    }
                    else {
                        reject(vm.errorHandler(this.response));
                    }
                }
            };
            req.send();
        });
    }
    ;
    /**
     * @description
     * @param {} entitySet
     * @param {} id
     * @param {} attribute
     * @param {} onSuccess
     * @param {} onError
     * @returns {}
     */
    static deleteProperty(entitySet, id, attribute) {
    }
    ;
    static deleteRecord(id, entitySet) {
    }
    ;
    static associateRecords(parentId, parentType, relationshipName, childId, childType) {
    }
    ;
    static disassociateRecords(parentId, parentType, relationshipName, childId) {
    }
    ;
    /**
     * @description Execute a default or custom action
     * @param {string} id
     * @param {string} entitySet
     * @param {string} actionName
     * @param {string} inputs
     * @returns {string}
     */
    static executeAction(id, entitySet, actionName, inputs) {
        var vm = this;
        id = vm.isGuid(id, "Jt.WebApi.executeAction id parameter must be a valid guid");
        var req = vm.getXmlHttpRequest("POST", entitySet, `(${id})/Microsoft.Dynamics.CRM.${actionName}`);
        return new Promise((resolve, reject) => {
            req.onreadystatechange = function () {
                if (this.readyState === 4 /* complete */) {
                    req.onreadystatechange = null;
                    if (this.status === 200) {
                        resolve(JSON.parse(this.response));
                    }
                    else if (this.status === 204) {
                        resolve();
                    }
                    else {
                        reject(vm.errorHandler(this.response));
                    }
                }
            };
            inputs != null ? req.send(JSON.stringify(inputs)) : req.send();
        });
    }
    ;
}
exports.WebApi = WebApi;
