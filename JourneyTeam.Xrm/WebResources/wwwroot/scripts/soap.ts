import Es6Promise = require("es6-promise");

namespace Jt.Crm {
    import Promise = Es6Promise.Promise;

    export class Soap {
        private ns: Object;

        constructor() {
            this.ns = {
                "s":
                    "http://schemas.xmlsoap.org/soap/envelope/",
                "a":
                    "http://schemas.microsoft.com/xrm/2011/Contracts",
                "i":
                    "http://www.w3.org/2001/XMLSchema-instance",
                "b":
                    "http://schemas.datacontract.org/2004/07/System.Collections.Generic",
                "c":
                    "http://www.w3.org/2001/XMLSchema",
                "d":
                    "http://schemas.microsoft.com/xrm/2011/Contracts/Services",
                "e":
                    "http://schemas.microsoft.com/2003/10/Serialization/",
                "f":
                    "http://schemas.microsoft.com/2003/10/Serialization/Arrays",
                "g":
                    "http://schemas.microsoft.com/crm/2011/Contracts",
                "h":
                    "http://schemas.microsoft.com/xrm/2011/Metadata",
                "j":
                    "http://schemas.microsoft.com/xrm/2011/Metadata/Query",
                "k":
                    "http://schemas.microsoft.com/xrm/2013/Metadata",
                "l":
                    "http://schemas.microsoft.com/xrm/2012/Contracts"
            }
        }

        /**
         * @description Retrieve the SOAP Endpoint
         * @returns {string} 
         */
        private getSoapUrl() {
            let context;

            if (typeof GetGlobalContext != "undefined") {
                context = GetGlobalContext();
            } else {
                if (typeof Xrm != "undefined") {
                    context = Xrm.Page.context;
                } else {
                    throw new Error("Context is not available.");
                }
            }
            return context.getClientUrl() + "/XRMServices/2011/Organization.svc/web";
        }

        /**
         * @description Create XMLHttpRequest object
         * @returns {XMLHttpRequest} 
         */
        private getXmlHttpRequest(action: string) {
            let req = new XMLHttpRequest();
            req.open("POST", this.getSoapUrl(), true);
            try {
                req.responseType = "msxml-document";
            } catch (e) {
            }
            req.setRequestHeader("Accept", "application/xml, text/xml, */*");
            req.setRequestHeader("Content-Type", "text/xml; charset=utf-8");
            req.setRequestHeader("SOAPAction",
                "http://schemas.microsoft.com/xrm/2011/Contracts/Services/IOrganizationService/" + action);
            return req;
        }

        /**
         * @description Get SOAP Envelope header
         * @returns {string} 
         */
        private getEnvelopeHeader() {
            let envelopeHeader = ["<s:Envelope "];

            for (var i in this.ns) {
                envelopeHeader.push(" xmlns:" + i + "=\"" + this.ns[i] + "\"");
            }

            envelopeHeader.push("><s:Header><a:SdkClientVersion>6.0</a:SdkClientVersion></s:Header>");

            return envelopeHeader.join("");
        };

        /**
         * @description Sets the namespaces to be used when querying the document
         * @param {string} doc      
         */
        private setSelectionNamespaces(doc) {
            if (typeof doc.setProperty != "undefined") {
                let namespaces = [];

                for (var i in this.ns) {
                    namespaces.push("xmlns:" + i + "='" + this.ns[i] + "'");
                }

                doc.setProperty("SelectionNamespaces", namespaces.join(" "));
            }
        }

        private isNullOrUndefined(parameter: string, message: string) {
            if ((typeof parameter === "undefined") || parameter === null) {
                throw new Error(message);
            }
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
         * @description Execute a SOAP request
         * @param {string} request 
         * @param {object} passThruObj
         * @returns {Promise} 
         */
        execute(request, passThruObj) {
            let vm = this;
            vm.isNullOrUndefined(request, "Jt.Soap.execute request parameter is required");

            var executeXml = [
                vm.getEnvelopeHeader(),
                "<s:Body>",
                "<d:Execute>",
                request,
                "</d:Execute>",
                "</s:Body>",
                "</s:Envelope>"
            ].join("");

            return new Promise((resolve, reject) => {
                var req = vm.getXmlHttpRequest("Execute");

                req.onreadystatechange = function() {
                    if (this.readyState === 4) {
                        this.onreadystatechange = null;
                        if (this.status === 200) {
                            var doc = this.responseXML;
                            vm.setSelectionNamespaces(doc);
                            resolve(doc);
                        } else {
                            reject(vm.errorHandler(this));
                        }
                    }
                };

                req.send(executeXml);
            });
        }
    }
}