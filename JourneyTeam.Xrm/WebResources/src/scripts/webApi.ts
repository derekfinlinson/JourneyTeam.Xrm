import Es6Promise = require("es6-promise");

namespace Jt.Crm {
    import Promise = Es6Promise.Promise;

    export class WebApi {
        private isGuid(parameter: string, message: string) {
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

        private isString(parameter:string, message:string) {
            if (typeof parameter != "string") {
                throw new Error(message);
            }
        }

        private isNullOrUndefined(parameter:Object, message:string) {
            if ((typeof parameter === "undefined") || parameter === null) {
                throw new Error(message);
            }
        }

        private errorHandler(response:string) {
            try {
                return JSON.parse(response).error;
            } catch (e) {
                return new Error("Unexpected Error");
            }
        }

        private getApiUrl() {
            var context;

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

        private getXmlHttpRequest(method: string, entitySet: string, options: string) {
            var req = new XMLHttpRequest();
            req.open(method, this.getApiUrl() + entitySet + options, true);
            req.setRequestHeader("Accept", "application/json");
            req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
            req.setRequestHeader("OData-MaxVersion", "4.0");
            req.setRequestHeader("OData-Version", "4.0");
            return req;
        }

        /**
         * @description Retrieve a record
         * @param {string} id 
         * @param {string} entitySetName 
         * @param {string} options     
         * @returns {Promise} 
         */
        retrieve(id: string, entitySet: string, options: string) {
            const vm = this;

            id = vm.isGuid(id, "Jt.WebApi.retrieveRecord id parameter must be a valid guid");
            vm.isString(entitySet, "Jt.WebApi.retrieveRecord entitySet parameter must be a string");
            vm.isString(entitySet, "Jt.WebApi.retrieveRecord options parameter must be a string");

            var req = vm.getXmlHttpRequest("GET", entitySet, `(${id})${options}`);

            return new Promise((resolve, reject) => {
                req.onreadystatechange = function () {
                    if (this.readyState === 4 /* complete */) {
                        req.onreadystatechange = null;
                        if (this.status === 200) {
                            resolve(JSON.parse(this.response));
                        } else {
                            reject(this.errorHandler(JSON.parse(this.response)));
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
        retrieveMultiple(entitySet: string, options: string) {
            const vm = this;
            vm.isString(entitySet, "Jt.WebApi.retrieveRecords entitySetName parameter must be a string");

            var req = vm.getXmlHttpRequest("GET", entitySet, options);

            return new Promise((resolve, reject) => {
                req.onreadystatechange = function () {                    
                    if (this.readyState === 4 /* complete */) {
                        req.onreadystatechange = null;
                        if (this.status === 200) {
                            resolve(JSON.parse(this.response));
                        } else {
                            reject(this.errorHandler(JSON.parse(this.response)));
                        }
                    }
                };

                req.send();
            });
        };

        /** @description Create a new entity
        * @param {string} entitySet The name of the entity set for the type of entity you want to create (eq, accounts).
        * @param {Object} entity An object with the properties for the entity you want to create.    
        */
        create(entitySet: string, entity:Object) {
            const vm = this;
            vm.isString(entitySet, "Jt.WebApi.createRecord entitySet parameter must be a string.");
            vm.isNullOrUndefined(entity, "Jt.WebApi.createRecord entity parameter must not be null or undefined.");

            var req = vm.getXmlHttpRequest("POST", entitySet, "");

            return new Promise((resolve, reject) => {
                req.onreadystatechange = function () {
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
         * @param {string} entitySet
         * @param {string} id 
         * @param {Object} entity 
         * @returns {}
         */
        updateRecord (entitySet:string, id:string, entity:Object) {
            const vm = this;
            vm.isString(entitySet, "Jt.WebApi.updateRecord entitySet parameter must be a string.");
            id = vm.isGuid(id, "Jt.WebApi.updateRecord id parameter must be a valid guid");
            vm.isNullOrUndefined(entity, "Jt.WebApi.updateRecord entity parameter must not be null or undefined.");

            var req = vm.getXmlHttpRequest("PATCH", entitySet, `(${id})`);

            return new Promise((resolve, reject) => {
                req.onreadystatechange = function () {
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
         * @param {string} entitySet 
         * @param {string} id 
         * @param {string} attribute 
         * @param {any} value 
         * @returns {} 
         */
        updateProperty (entitySet:string, id:string, attribute:string, value:any) {
            const vm = this;
            vm.isString(entitySet, "Jt.WebApi.updateProperty entitySet parameter must be a string.");
            id = vm.isGuid(id, "Jt.WebApi.updateProperty id parameter must be a valid guid");
            vm.isString(attribute, "Jt.WebApi.updateProperty attribute parameter must be a string.");
            vm.isNullOrUndefined(value, "Jt.WebApi.updateProperty value parameter is required.");

            var req = vm.getXmlHttpRequest("PUT", entitySet, `(${id})`);

            return new Promise((resolve, reject) => {
                req.onreadystatechange = function () {
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
         * @param {string} entitySet 
         * @param {string} id 
         * @param {string} attribute          
         * @returns {} 
         */
        deleteProperty (entitySet:string, id:string, attribute:string) {

        };

        /**
         * @description Delete a record
         * @param {string} id
         * @param {string} entitySet
         */
        deleteRecord (id:string, entitySet:string) {

        };

        associateRecords (parentId:string,
            parentType:string,
            relationshipName:string,
            childId:string,
            childType:string) {

        };

        disassociateRecords (parentId:string, parentType:string, relationshipName:string, childId:string) {

        };

        /**
         * @description Execute a default or custom action
         * @param {string} id 
         * @param {string} entitySet
         * @param {string} actionName 
         * @param {Object} inputs
         * @returns {string} 
         */
        executeAction (id:string, entitySet:string, actionName:string, inputs:string) {
            const vm = this;
            id = vm.isGuid(id, "Jt.WebApi.executeAction id parameter must be a valid guid");
            vm.isString(entitySet, "Jt.WebApi.executeAction entitySet parameter must be a string");
            vm.isString(actionName, "Jt.WebApi.executeAction actionName parameter must be a string");

            var req = vm.getXmlHttpRequest("POST", entitySet, `(${id})/Microsoft.Dynamics.CRM.${actionName}`);

            return new Promise((resolve, reject) => {
                req.onreadystatechange = function () {
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
}