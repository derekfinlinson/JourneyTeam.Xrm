// ReSharper disable UseOfImplicitGlobalInFunctionScope
exports.soap = (function () {
    var vm = this;
    /**
     * @description Retrieve the client url
     * @returns {string} 
     */
    function getClientUrl() {
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

        return context.getClientUrl() + "/XRMServices/2011/Organization.svc/web";
    }

    /**
     * @description Get error from XML response
     * @param {XmlHttpRequest} req XMLHttpRequest response
     * @returns {Error}
     */
    function errorHandler(req) {
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
     * @description Check whether required parameters are null or undefined
     * @param {string} parameter Parameter to check
     * @param {funtion} message Error message text to include when the error is thrown     
     */
    function isNullOrUndefined(parameter, message) {
        if ((typeof parameter === "undefined") || parameter === null) {
            throw new Error(message);
        }
    }
    
    /**
     * @description Create XMLHttpRequest object
     * @param {string} method Request method (POST or GET)
     * @param {string} type Schema name of the entity
     * @param {string} options Query parameters to include in URL
     * @returns {XMLHttpRequest} 
     */
    function getXmlHttpRequest(action) {
        var req = new XMLHttpRequest();
        req.open("POST", getClientUrl(), true);
        try { req.responseType = 'msxml-document' } catch (e) { }
        req.setRequestHeader("Accept", "application/xml, text/xml, */*");
        req.setRequestHeader("Content-Type", "text/xml; charset=utf-8");
        req.setRequestHeader("SOAPAction", "http://schemas.microsoft.com/xrm/2011/Contracts/Services/IOrganizationService/" + action);
        return req;
    }

    /**
     * Namespaces used
     */
    var ns = {
        "s": "http://schemas.xmlsoap.org/soap/envelope/",
        "a": "http://schemas.microsoft.com/xrm/2011/Contracts",
        "i": "http://www.w3.org/2001/XMLSchema-instance",
        "b": "http://schemas.datacontract.org/2004/07/System.Collections.Generic",
        "c": "http://www.w3.org/2001/XMLSchema",
        "d": "http://schemas.microsoft.com/xrm/2011/Contracts/Services",
        "e": "http://schemas.microsoft.com/2003/10/Serialization/",
        "f": "http://schemas.microsoft.com/2003/10/Serialization/Arrays",
        "g": "http://schemas.microsoft.com/crm/2011/Contracts",
        "h": "http://schemas.microsoft.com/xrm/2011/Metadata",
        "j": "http://schemas.microsoft.com/xrm/2011/Metadata/Query",
        "k": "http://schemas.microsoft.com/xrm/2013/Metadata",
        "l": "http://schemas.microsoft.com/xrm/2012/Contracts"
    };

    /**
     * @description Get SOAP Envelope header
     * @returns {string} 
     */
    function getEnvelopeHeader () {
        var envelopeHeader = ["<s:Envelope "];

        for (var i in ns) {
            envelopeHeader.push(" xmlns:" + i + "=\"" + ns[i] + "\"");
        }

        envelopeHeader.push("><s:Header><a:SdkClientVersion>6.0</a:SdkClientVersion></s:Header>");

        return envelopeHeader.join("");
    }

    /**
     * @description Sets the namespaces to be used when querying the document
     * @param {string} doc      
     */
    function setSelectionNamespaces (doc) {
        if (typeof doc.setProperty != "undefined") {
            var namespaces = [];

            for (var i in ns) {
                namespaces.push("xmlns:" + i + "='" + ns[i] + "'");
            }

            doc.setProperty("SelectionNamespaces", namespaces.join(" "));
        }
    }

    /**
     * @description Execute a SOAP request
     * @param {string} request 
     * @param {object} passThruObj
     * @returns {Promise} 
     */
    vm.execute = function (request, passThruObj) {
        isNullOrUndefined(request, "Jt.Soap.execute request parameter is required");

        var executeXml = [
            getEnvelopeHeader(),
            "<s:Body>",
            "<d:Execute>",
            request,
            "</d:Execute>",
            "</s:Body>",
            "</s:Envelope>"
        ].join("");

        return new Promise(function (resolve, reject) {
            var req = getXmlHttpRequest("Execute");

            req.onreadystatechange = function () {
                if (this.readyState === 4) {
                    this.onreadystatechange = null;
                    if (this.status === 200) {
                        var doc = this.responseXML;
                        setSelectionNamespaces(doc);
                        resolve(doc, passThruObj);
                    } else {
                        reject(errorHandler(this), passThruObj);
                    }
                }
            };

            req.send(executeXml);
        });
    };
})();