/// <reference path="../../typings/globals/xrm/index.d.ts" />

export class WebApi {
    private static getClientUrl(): string {
        let context: any;

        if (typeof GetGlobalContext != "undefined") {
            context = GetGlobalContext();
        } else {
            if (typeof Xrm != "undefined") {
                context = Xrm.Page.context;
            } else {
                throw new Error("Context is not available.");
            }
        }

        return context.getClientUrl() + "/api/data/v8.0/";
    }

    private static errorHandler(response: any): any {
        try {
            return JSON.parse(response).error;
        } catch (e) {
            return new Error("Unexpected Error");
        }
    }

    private static isGuid(parameter: string, message: string): any {
        parameter = parameter.replace(/[{}]/g, "");
        if (typeof parameter == "string") {
            if (/^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/.test(parameter) ===
                false) {
                throw new Error(message);
            }
        } else {
            throw new Error(message);
        }

        return parameter;
    }

    private static getXmlHttpRequest(method: string, enitySet: string, options: string): XMLHttpRequest {
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
    static retrieveRecord(id: string, entitySet: string, options: string): any {
        const vm = this;
        id = vm.isGuid(id, "Jt.WebApi.retrieveRecord id parameter must be a valid guid");

        var req = vm.getXmlHttpRequest("GET", entitySet, `(${id})${options}`);

        return new Promise((resolve, reject) => {
            req.onreadystatechange = function() {
                if (this.readyState === 4 /* complete */) {
                    req.onreadystatechange = null;
                    if (this.status === 200) {
                        resolve(JSON.parse(this.response));
                    } else {
                        reject(vm.errorHandler(JSON.parse(this.response)));
                    }
                }
            };

            req.send();
        });
    };

    /**
     * @description Retrieve records
     * @param {} entitySet 
     * @param {} options 
     * @returns {} 
     */
    static retrieveRecords(entitySet: string, options: string): any {
        var vm = this;
        var req = vm.getXmlHttpRequest("GET", entitySet, options);

        return new Promise((resolve, reject) => {
            req.onreadystatechange = function() {
                if (this.readyState === 4 /* complete */) {
                    req.onreadystatechange = null;
                    if (this.status === 200) {
                        resolve(JSON.parse(this.response));
                    } else {
                        reject(vm.errorHandler(JSON.parse(this.response)));
                    }
                }
            };

            req.send();
        });
    };

    /** @description Create a new entity
    * @param {string} entitySet The name of the entity set for the type of entity you want to create (eq, accounts).
    * @param {object} entity An object with the properties for the entity you want to create.    
    */
    static createRecord(entitySet: string, entity: Object): any {
        var vm = this;
        var req = vm.getXmlHttpRequest("POST", entitySet, "");

        return new Promise((resolve, reject) => {
            req.onreadystatechange = function() {
                if (this.readyState === 4 /* complete */) {
                    req.onreadystatechange = null;
                    if (this.status === 200) {
                        resolve(this.getResponseHeader("OData-EntityId"));
                    } else {
                        reject(vm.errorHandler(this.response));
                    }
                }
            };

            req.send(JSON.stringify(entity));
        });
    };

    /**
     * @description 
     * @param {} entitySet 
     * @param {} id 
     * @param {} entity 
     * @returns {} 
     */
    static updateRecord(entitySet: string, id: string, entity: string): any {
        var vm = this;
        id = vm.isGuid(id, "Jt.WebApi.updateRecord id parameter must be a valid guid");

        var req = vm.getXmlHttpRequest("PATCH", entitySet, "(" + id + ")");

        return new Promise((resolve, reject) => {
            req.onreadystatechange = function() {
                if (this.readyState === 4 /* complete */) {
                    req.onreadystatechange = null;
                    if (this.status === 204) {
                        resolve();
                    } else {
                        reject(vm.errorHandler(this.response));
                    }
                }
            };

            req.send(JSON.stringify(entity));
        });
    };

    /**
     * @description
     * @param {} entitySet 
     * @param {} id 
     * @param {} attribute 
     * @param {} value 
     * @returns {} 
     */
    static updateProperty(entitySet: string, id: string, attribute: string, value: any): any {
        const vm = this;
        id = vm.isGuid(id, "Jt.WebApi.updateProperty id parameter must be a valid guid");

        var req = vm.getXmlHttpRequest("PUT", entitySet, "(" + id + ")");

        return new Promise((resolve, reject) => {
            req.onreadystatechange = function() {
                if (this.readyState === 4 /* complete */) {
                    req.onreadystatechange = null;
                    if (this.status === 204) {
                        resolve();
                    } else {
                        reject(vm.errorHandler(this.response));
                    }
                }
            };

            req.send();
        });
    };

    /**
     * @description 
     * @param {} entitySet 
     * @param {} id 
     * @param {} attribute 
     * @param {} onSuccess 
     * @param {} onError 
     * @returns {} 
     */
    static deleteProperty(entitySet: string, id: string, attribute: string): any {

    };

    static deleteRecord(id: string, entitySet: string): any {

    };

    static associateRecords(parentId: string,
        parentType: string,
        relationshipName: string,
        childId: string,
        childType:
        string) {

    };

    static disassociateRecords(parentId: string, parentType: string, relationshipName: string, childId: string) {

    };

    /**
     * @description Execute a default or custom action
     * @param {string} id 
     * @param {string} entitySet 
     * @param {string} actionName 
     * @param {string} inputs      
     * @returns {string} 
     */
    static executeAction(id: string, entitySet: string, actionName: string, inputs: string): any {
        var vm = this;
        id = vm.isGuid(id, "Jt.WebApi.executeAction id parameter must be a valid guid");

        var req = vm.getXmlHttpRequest("POST", entitySet, `(${id})/Microsoft.Dynamics.CRM.${actionName}`);

        return new Promise((resolve, reject) => {
            req.onreadystatechange = function() {
                if (this.readyState === 4 /* complete */) {
                    req.onreadystatechange = null;
                    if (this.status === 200) {
                        resolve(JSON.parse(this.response));
                    } else if (this.status === 204) {
                        resolve();
                    } else {
                        reject(vm.errorHandler(this.response));
                    }
                }
            };

            inputs != null ? req.send(JSON.stringify(inputs)) : req.send();
        });
    };
}