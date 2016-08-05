/// <reference path="Jt.Xrm.js" />
/// <reference path="Jt.Services.js" />

var Jt = window.Jt || { };

(function(utility) {
    utility.setLookupValue = function(attribute, entityType, id, name) {
        var lookup = [];
        lookup[0] = {
            id: id,
            name: name,
            entityType: entityType
        };

        Xrm.Page.getAttribute(attribute).setValue(lookup);
        Xrm.Page.getAttribute(attribute).setSubmitMode(true);
    };
})(Jt.Utility = Jt.Utility || {});