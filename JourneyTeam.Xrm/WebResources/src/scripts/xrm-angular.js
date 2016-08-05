// ReSharper disable UndeclaredGlobalVariableUsing
/**
 * CRM Service for Angular
 */
(function () {
    "use strict";

    angular
        .module("app")
        .factory("crmService", ["$http", crmService]);

    function crmService($http) {
        var getClientUrl = function() {
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
        };

        var service = {
            retrieveRecord: function(id, entitySetName, select, expand) {
                var systemQueryOptions = "";

                if (select != null || expand != null) {
                    systemQueryOptions = "?";
                    if (select != null) {
                        systemQueryOptions += "$select=" + select.join(",");
                    }
                    if (expand != null) {
                        systemQueryOptions += systemQueryOptions + "&$expand=" + expand.join(",");
                    }
                }

                var url = [
                    getClientUrl(),
                    entitySetName,
                    "(" + id + ")",
                    systemQueryOptions
                ].join("");

                return $http.get(url);
            },
            retrieveProperty: function() {
                
            },
            retrieveRecords: function(entitySetName, options) {
                var url = [
                    getClientUrl(),
                    entitySetName,
                    options
                ].join("");

                return $http.get(url);
            },
            createRecord: function() {

            },
            updateRecord: function() {
                
            },
            updateProperty: function() {
                
            }
        };

        return service;
    }
})();