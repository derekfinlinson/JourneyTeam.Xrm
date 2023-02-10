using Microsoft.Xrm.Sdk;
using System;

namespace Xrm {
    public static partial class OptionSetExtensions {
        /// <summary>
        /// Get early bound metadata values for optionset options
        /// </summary>
        /// <param name="optionSet">Enum</param>
        /// <returns>OptionSetMetadataAttribute</returns>
        public static OptionSetMetadataAttribute GetMetadata(this Enum optionSet) {
            var type = optionSet.GetType();
            var memInfo = type.GetMember(optionSet.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(OptionSetMetadataAttribute), false);
            return (attributes.Length > 0) ? (OptionSetMetadataAttribute)attributes[0] : default(OptionSetMetadataAttribute);
        }
    }
}
