// ReSharper disable UseOfImplicitGlobalInFunctionScope
exports.rest = (function () {
    var vm = this;
    /**
     * @description Retrieve the client url
     * @returns {string} 
     */
    function getClientUrl () {
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

    /**
     * @description Get error from XML response
     * @param {XmlHttpRequest} req XMLHttpRequest response
     * @returns {Error}
     */
    function errorHandler (req) {
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
     * Convert matching string values to Date objects
     * @param {string} key Key used to identify the object property
     * @param {string} value String value representing a date
     * @returns {date} 
     */
    function dateReviver (key, value) {
        var a;
        if (typeof value === 'string') {
            a = /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)Z$/.exec(value);
            if (a) {
                return new Date(Date.UTC(+a[1], +a[2] - 1, +a[3], +a[4], +a[5], +a[6]));
            }
        }

        return value;
    }

    /**
     * @description Check whether required parameters are null or undefined
     * @param {string} parameter Parameter to check
     * @param {funtion} message Error message text to include when the error is thrown     
     */
    function isNullOrUndefined (parameter, message) {
        if ((typeof parameter === "undefined") || parameter === null) {
            throw new Error(message);
        }
    }

    /**
     * @description Check whether a parameter is a string
     * @param {function} parameter Parameter to check
     * @param {string} message Error message text to include when the error is thrown
     * @returns {nothing} 
     */
    function isString (parameter, message) {
        if (typeof parameter != "string") {
            throw new Error(message);
        }
    }
    
    /**
     * @description Check whether parameter is a Guid
     * @param {string} parameter Parameter to check
     * @param {string} message Error message text to include when error is thrown
     * @returns {nothing} 
     */
    function isGuid (parameter, message) {
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
    
    /**
     * @description Create XMLHttpRequest object
     * @param {string} method Request method (POST or GET)
     * @param {string} type Schema name of the entity
     * @param {string} options Query parameters to include in URL
     * @returns {XMLHttpRequest} 
     */
    function getXmlHttpRequest (method, type, options) {
        var req = new XMLHttpRequest();
        req.open(method, encodeURI(getClientUrl() + type + "Set" + options), true);
        req.setRequestHeader("Accept", "application/json");
        req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    }

    /**
     * @description Send an asynchronous request to create a new record
     * @param {object} entity JavaScript object with properties corresponding to the Schema name of entity attributes
     * @param {string} logicalName Schema Name of the Entity to create
     * @returns {Error or object}
     */
    vm.createRecord = function (entity, logicalName) {
        isNullOrUndefined(entity, "Jt.Rest.createRecord requires the object parameter.");
        isString(logicalName, "Jt.Rest.createRecord requires the type parameter is a string.");

        var req = getXmlHttpRequest("POST", logicalName);

        req.onreadystatechange = function () {
            if (this.readyState === 4 /* complete */) {
                req.onreadystatechange = null;
                if (this.status === 201) {
                    successCallback(JSON.parse(this.responseText, dateReviver).d);
                } else {
                    errorCallback(errorHandler(this));
                }
            }
        };
        req.send(JSON.stringify(entity));
    };

    /**
     * @description Send an asynchronous request to retrieve a record
     * @param {string} id Guid of the record to retrieve
     * @param {string} logicalName Schema name of the entity type to retrieve
     * @param {Array<>} select $select OData System Query Option to control which attributes will be returned
     * @param {string} expand $expand OData System Query Option value to control which related records are also returned
     * @returns {} 
     */
    vm.retrieveRecord = function (id, logicalName, select, expand) {
        isNullOrUndefined(id, "Jt.Rest.retrieveRecord requires the id parameter is a string.");
        isString(logicalName, "Jt.Rest.retrieveRecord requires the type parameter is a string.");

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

        var req = getXmlHttpRequest("GET", logicalName, "(guid'" + id + "')" + systemQueryOptions);

        req.onreadystatechange = function () {
            if (this.readyState === 4 /* complete */) {
                req.onreadystatechange = null;
                if (this.status === 200) {
                    successCallback(JSON.parse(this.responseText, dateReviver).d);
                } else {
                    errorCallback(errorHandler(this));
                }
            }
        };
        req.send();
    };

    /**
     * @description Send an asynchronous request to update a record
     * @param {string} id Guid of the record to update
     * @param {string} entity Entity fields to update
     * @param {string} logicalName Schema name of the entity to update
     * @returns {Error or nothing} 
     */
    vm.updateRecord = function (id, entity, logicalName, successCallback, errorCallback) {
        isNullOrUndefined(id, "Jt.Rest.updateRecord requires the id parameter.");
        isNullOrUndefined(entity, "Jt.Rest.updateRecord requires the object parameter.");
        isString(logicalName, "Jt.Rest.updateRecord requires the type parameter.");

        var req = getXmlHttpRequest("POST", logicalName, "(guid'" + id + "')");
        req.setRequestHeader("X-HTTP-Method", "MERGE");

        req.onreadystatechange = function () {
            if (this.readyState === 4 /* complete */) {
                req.onreadystatechange = null;
                if (this.status === 204 || this.status === 1223) {
                    successCallback();
                } else {
                    errorCallback(errorHandler(this));
                }
            }
        };
        req.send(JSON.stringify(entity));
    };

    /**
     * @description Send an asynchronous request to delete a record
     * @param {string} id Guid of the record to delete
     * @param {string} logicalName Schema name of the entity to delete
     * @returns {Error or nothing} 
     */
    vm.deleteRecord = function (id, logicalName) {
        isGuid(id, "Jt.Rest.deleteRecord requires the id parameter.");
        isString(logicalName, "Jt.Rest.deleteRecord requires the type parameter.");

        var req = getXmlHttpRequest("POST", logicalName, "(guid'" + id + "')");
        req.setRequestHeader("X-HTTP-Method", "DELETE");

        req.onreadystatechange = function () {
            if (this.readyState === 4 /* complete */) {
                req.onreadystatechange = null;
                if (this.status === 204 || this.status === 1223) {
                    successCallback();
                } else {
                    errorCallback(errorHandler(this));
                }
            }
        };
        req.send();
    };

    /**
     * @description Send an asynchronous request to retrieve records
     * @param {string} logicalName Schema name of the entities to retrieve
     * @param {string} options OData System Query Options
     * @returns {Error or page with 50 records} 
     */
    vm.retrieveRecords = function (logicalName, options) {
        isString(logicalName, "Jt.Rest.retrieveMultipleRecords requires the type parameter is a string.");

        if (options != null)
            isString(options, "Jt.Rest.retrieveMultipleRecords requires the options parameter is a string.");

        var optionsString = "";

        if (options != null) {
            if (options.charAt(0) !== "?") {
                optionsString = "?" + options;
            } else {
                optionsString = options;
            }
        }

        var req = getXmlHttpRequest("GET", logicalName, optionsString);

        req.onreadystatechange = function () {
            if (this.readyState === 4 /* complete */) {
                req.onreadystatechange = null;
                if (this.status === 200) {
                    var returned = JSON.parse(this.responseText, dateReviver).d;
                    successCallback(returned.results);
                    if (returned.__next != null) {
                        var queryOptions = returned.__next.substring((restUrl() + logicalName + "Set").length);
                        Jt.Rest.retrieveMultipleRecords(logicalName, queryOptions);
                    } else {
                        onComplete();
                    }
                } else {
                    errorCallback(errorHandler(this));
                }
            }
        };
        req.send();
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
    vm.associateRecords = function (parentId, parentType, relationshipName, childId, childType) {
        isString(parentId, "Jt.Rest.associateRecords requires the parentId parameter is a string.");
        isString(parentType, "Jt.Rest.associateRecords requires the parentType parameter is a string.");
        isString(relationshipName, "Jt.Rest.associateRecords requires the relationshipName parameter is a string.");
        isString(childId, "Jt.Rest.associateRecords requires the childId parameter is a string.");
        isString(childType, "Jt.Rest.associateRecords requires the childType parameter is a string.");

        var req = getXmlHttpRequest("POST", parentType, "(guid'" + parentId + "')/$links/" + relationshipName);

        req.onreadystatechange = function () {
            if (this.readyState === 4 /* complete */) {
                req.onreadystatechange = null;
                if (this.status === 204 || this.status === 1223) {
                    successCallback();
                } else {
                    errorCallback(errorHandler(this));
                }
            }
        };
        var childEntityReference = {};
        childEntityReference.uri = restUrl() + "/" + childType + "Set(guid'" + childId + "')";
        req.send(JSON.stringify(childEntityReference));
    };

    /**
     * @description Send asynchronous request to disassociate records
     * @param {string} parentId Guid of the parent record
     * @param {string} parentType Schema name of the entity of the parent record
     * @param {string} relationshipName Schema name of the relationship
     * @param {string} childId Guid of the child record
     * @param {string} childType Schema name of the entity of the child record
     * @returns {} 
     */
    rest.disassociateRecords = function (parentId, parentType, relationshipName, childId) {
        isString(parentId, "Jt.Rest.disassociateRecords requires the parentId parameter is a string.");
        isString(parentType, "Jt.Rest.disassociateRecords requires the parentType parameter is a string.");
        isString(relationshipName, "Jt.Rest.disassociateRecords requires the relationshipName parameter is a string.");
        isString(childId, "Jt.Rest.disassociateRecords requires the childId parameter is a string.");

        var req = getXmlHttpRequest("POST",
            parentType,
            "(guid'" + parentId + "')/$links/" + relationshipName + "(guid'" + childId + "')");

        req.setRequestHeader("X-HTTP-Method", "DELETE");

        req.onreadystatechange = function () {
            if (this.readyState === 4 /* complete */) {
                req.onreadystatechange = null;
                if (this.status === 204 || this.status === 1223) {
                    successCallback();
                } else {
                    errorCallback(errorHandler(this));
                }
            }
        };

        req.send();
    };
})();