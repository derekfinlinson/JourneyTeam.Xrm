import Es6Promise = require("es6-promise");

namespace Jt.Crm {
    import Promise = Es6Promise.Promise;

    export class Rest {
        private getRestUrl() {
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
            return context.getClientUrl() + "/XRMServices/2011/OrganizationData.svc/";
        }

        private getXmlHttpRequest(method: string, type: string, options: string) {
            var req = new XMLHttpRequest();
            req.open(method, encodeURI(this.getRestUrl() + type + "Set" + options), true);
            req.setRequestHeader("Accept", "application/json");
            req.setRequestHeader("Content-Type", "application/json; charset=utf-8");

            return req;
        }

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

        private isString(parameter: string, message: string) {
            if (typeof parameter != "string") {
                throw new Error(message);
            }
        }

        private isNullOrUndefined(parameter: string, message: string) {
            if ((typeof parameter === "undefined") || parameter === null) {
                throw new Error(message);
            }
        }

        private dateReviver(key, value) {
            var a;
            if (typeof value === 'string') {
                a = /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)Z$/.exec(value);
                if (a) {
                    return new Date(Date.UTC(+a[1], +a[2] - 1, +a[3], +a[4], +a[5], +a[6]));
                }
            }

            return value;
        }

        private errorHandler(req) {
            if (req.status === 12029) {
                return new Error("The attempt to connect to the server failed.");
            }
            if (req.status === 12007) {
                return new Error("The server name could not be resolved.");
            }
            var errorText;
            try {
                errorText = JSON.parse(req.responseText).error.message.value;
            } catch (e) {
                errorText = req.responseText;
            }

            return new Error("Error : " +
                req.status +
                ": " +
                req.statusText +
                ": " +
                errorText);
        }

        /**
         * @description Send an asynchronous request to create a new record
         * @param {object} entity JavaScript object with properties corresponding to the Schema name of entity attributes
         * @param {string} entityName Schema Name of the Entity to create     
         * @returns {Error or object}
         */
        createRecord(entity: string, entityName: string) {
            let vm = this;
            vm.isNullOrUndefined(entity, "Jt.Rest.createRecord requires the object parameter.");
            vm.isString(entityName, "Jt.Rest.createRecord requires the type parameter is a string.");

            var req = vm.getXmlHttpRequest("POST", entityName, "");

            return new Promise((resolve, reject) => {
                req.onreadystatechange = function() {
                    if (this.readyState === 4 /* complete */) {
                        req.onreadystatechange = null;
                        if (this.status === 201) {
                            resolve(JSON.parse(this.responseText, vm.dateReviver).d);
                        } else {
                            reject(vm.errorHandler(this));
                        }
                    }
                };
                req.send(JSON.stringify(entity));
            });
        };

        /**
         * @description Send an asynchronous request to retrieve a record
         * @param {string} id Guid of the record to retrieve
         * @param {string} entityName Schema name of the entity type to retrieve
         * @param {string} select $select OData System Query Option to control which attributes will be returned
         * @param {string} expand $expand OData System Query Option value to control which related records are also returned
         * @returns {} 
         */
        retrieveRecord(id: string, entityName: string, select: string, expand: string) {
            const vm = this;
            id = vm.isGuid(id, "Jt.Rest.retrieveRecord requires the id parameter is a Guid");
            vm.isString(entityName, "Jt.Rest.retrieveRecord requires the entityName parameter is a string");

            if (select != null)
                vm.isString(select, "Jt.Rest.retrieveRecord select parameter must be a string");
            if (expand != null)
                vm.isString(expand, "Jt.Rest.retrieveRecord expand parameter must be a string");

            var systemQueryOptions = "";

            if (select != null || expand != null) {
                systemQueryOptions = "?";
                if (select != null) {
                    var selectString = "$select=" + select;
                    if (expand != null) {
                        selectString = selectString + "," + expand;
                    }
                    systemQueryOptions = systemQueryOptions + selectString;
                }
                if (expand != null) {
                    systemQueryOptions = systemQueryOptions + "&$expand=" + expand;
                }
            }

            var req = vm.getXmlHttpRequest("GET", entityName, `(guid'${id}')${systemQueryOptions}`);

            return new Promise((resolve, reject) => {
                req.onreadystatechange = function() {
                    if (this.readyState === 4 /* complete */) {
                        req.onreadystatechange = null;
                        if (this.status === 200) {
                            resolve(JSON.parse(this.responseText, vm.dateReviver).d);
                        } else {
                            reject(vm.errorHandler(this));
                        }
                    }
                };
                req.send();
            });
        };

        /**
         * @description Send an asynchronous request to update a record
         * @param {string} id Guid of the record to update
         * @param {string} entity Entity fields to update
         * @param {string} entityName Schema name of the entity to update
         * @returns {Error or nothing} 
         */
        updateRecord(id, entity, entityName) {
            const vm = this;
            vm.isNullOrUndefined(id, "Jt.Rest.updateRecord requires the id parameter.");
            vm.isNullOrUndefined(entity, "Jt.Rest.updateRecord requires the object parameter.");
            vm.isString(entityName, "Jt.Rest.updateRecord requires the entityName parameter.");

            var req = vm.getXmlHttpRequest("POST", entityName, `(guid'${id}')`);
            req.setRequestHeader("X-HTTP-Method", "MERGE");

            return new Promise((resolve, reject) => {
                req.onreadystatechange = function() {
                    if (this.readyState === 4 /* complete */) {
                        req.onreadystatechange = null;
                        if (this.status === 204 || this.status === 1223) {
                            resolve();
                        } else {
                            reject(vm.errorHandler(this));
                        }
                    }
                };
                req.send(JSON.stringify(entity));
            });
        };

        /**
         * @description Send an asynchronous request to delete a record
         * @param {string} id Guid of the record to delete
         * @param {string} entityName Schema name of the entity to delete
         * @returns {Error or nothing} 
         */
        deleteRecord(id, entityName) {
            const vm = this;
            vm.isGuid(id, "Jt.Rest.deleteRecord requires the id parameter.");
            vm.isString(entityName, "Jt.Rest.deleteRecord requires the entityName parameter.");

            var req = vm.getXmlHttpRequest("POST", entityName, `(guid'${id}')`);
            req.setRequestHeader("X-HTTP-Method", "DELETE");

            return new Promise((resolve, reject) => {
                req.onreadystatechange = function() {
                    if (this.readyState === 4 /* complete */) {
                        req.onreadystatechange = null;
                        if (this.status === 204 || this.status === 1223) {
                            resolve();
                        } else {
                            reject(vm.errorHandler(this));
                        }
                    }
                };
                req.send();
            });
        };

        /**
         * @description Send an asynchronous request to retrieve records
         * @param {string} logicalName Schema name of the entities to retrieve
         * @param {string} options OData System Query Options
         * @returns {Error or page with 50 records} 
         */
        retrieveMultipleRecords(logicalName, options) {
            const vm = this;
            vm.isString(logicalName, "Jt.Rest.retrieveMultipleRecords requires the type parameter is a string.");

            if (options != null)
                vm.isString(options, "Jt.Rest.retrieveMultipleRecords requires the options parameter is a string.");

            var optionsString = "";

            if (options != null) {
                if (options.charAt(0) !== "?") {
                    optionsString = "?" + options;
                } else {
                    optionsString = options;
                }
            }

            var req = vm.getXmlHttpRequest("GET", logicalName, optionsString);

            return new Promise((resolve, reject) => {
                req.onreadystatechange = function() {
                    if (this.readyState === 4 /* complete */) {
                        req.onreadystatechange = null;
                        if (this.status === 200) {
                            var returned = JSON.parse(this.responseText, vm.dateReviver).d;
                            resolve(returned.results);
                        } else {
                            reject(vm.errorHandler(this));
                        }
                    }
                };
                req.send();
            });
        };

        /**
         * @description Send asynchronous request to associate records
         * @param {string} parentId Guid of the parent record
         * @param {string} parentType Schema name of the entity of the parent record
         * @param {string} relationshipName Schema name of the relationship
         * @param {string} childId Guid of the child record
         * @param {string} childType Schema name of the entity of the child record      
         * @returns {Error or nothing} 
         */
        associateRecords(parentId, parentType, relationshipName, childId, childType) {
            const vm = this;
            vm.isString(parentId, "Jt.Rest.associateRecords requires the parentId parameter is a string.");
            vm.isString(parentType, "Jt.Rest.associateRecords requires the parentType parameter is a string.");
            vm.isString(relationshipName,
                "Jt.Rest.associateRecords requires the relationshipName parameter is a string.");
            vm.isString(childId, "Jt.Rest.associateRecords requires the childId parameter is a string.");
            vm.isString(childType, "Jt.Rest.associateRecords requires the childType parameter is a string.");

            var req = vm.getXmlHttpRequest("POST", parentType, "(guid'" + parentId + "')/$links/" + relationshipName);

            return new Promise((resolve, reject) => {
                req.onreadystatechange = function() {
                    if (this.readyState === 4 /* complete */) {
                        req.onreadystatechange = null;
                        if (this.status === 204 || this.status === 1223) {
                            resolve();
                        } else {
                            reject(vm.errorHandler(this));
                        }
                    }
                };
                var childEntityReference = { uri: "" };
                childEntityReference.uri = vm.getRestUrl() + "/" + childType + "Set(guid'" + childId + "')";
                req.send(JSON.stringify(childEntityReference));
            });
        };

        /**
         * @description Send asynchronous request to disassociate records
         * @param {string} parentId Guid of the parent record
         * @param {string} parentType Schema name of the entity of the parent record
         * @param {string} relationshipName Schema name of the relationship
         * @param {string} childId Guid of the child record
         * @param {string} childType Schema name of the entity of the child record
         * @returns {Promise} 
         */
        disassociateRecords(parentId, parentType, relationshipName, childId) {
            const vm = this;
            vm.isString(parentId, "Jt.Rest.disassociateRecords requires the parentId parameter is a string.");
            vm.isString(parentType, "Jt.Rest.disassociateRecords requires the parentType parameter is a string.");
            vm.isString(relationshipName,
                "Jt.Rest.disassociateRecords requires the relationshipName parameter is a string.");
            vm.isString(childId, "Jt.Rest.disassociateRecords requires the childId parameter is a string.");

            var req = vm.getXmlHttpRequest("POST",
                parentType,
                `(guid'${parentId}')/$links/${relationshipName}(guid'${childId}')`);

            req.setRequestHeader("X-HTTP-Method", "DELETE");

            return new Promise((resolve, reject) => {
                req.onreadystatechange = function() {
                    if (this.readyState === 4 /* complete */) {
                        req.onreadystatechange = null;
                        if (this.status === 204 || this.status === 1223) {
                            resolve();
                        } else {
                            reject(vm.errorHandler(this));
                        }
                    }
                };

                req.send();
            });
        }
    }
}