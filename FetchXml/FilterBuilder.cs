using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Xrm
{
    public class FilterBuilder
    {
        private string _type = "and";

        private readonly List<string> _conditions = new List<string>();

        private readonly List<FilterBuilder> _childFilters = new List<FilterBuilder>();

        public FilterBuilder(string type = "and")
        {
            _type = type;
        }

        /// <summary>
        /// Add condition with a value or values
        /// </summary>
        /// <param name="attribute">Attribute to filter on</param>
        /// <param name="conditionOperator">Condition operator</param>
        /// <param name="value">Value or values</param>
        /// <returns>FilterBuilder</returns>
        public FilterBuilder WithCondition(string attribute, string conditionOperator, object value)
        {
            var builder = new StringBuilder($"<condition attribute='{attribute}' operator='{conditionOperator}'");

            if (conditionOperator == "in" || conditionOperator == "between")
            {
                var values = (IEnumerable)value;

                builder.Append(">");

                foreach (var item in values)
                {
                    builder.Append($"<value>{item}</value>");
                }

                builder.Append("</condition>");
            }
            else
            {
                builder.Append($" value='{value}' />");
            }

            _conditions.Add(builder.ToString());

            return this;
        }

        /// <summary>
        /// Add condition that doesn't require a value like null or not-null
        /// </summary>
        /// <param name="attribute">Attribute to filter on</param>
        /// <param name="conditionOperator">Condition operator</param>
        /// <returns>FilterBuilder</returns>
        public FilterBuilder WithCondition(string attribute, string conditionOperator)
        {
            var builder = new StringBuilder($"<condition attribute='{attribute}' operator='{conditionOperator}' />");

            _conditions.Add(builder.ToString());

            return this;
        }

        /// <summary>
        /// Add child filter
        /// </summary>
        /// <param name="childFilter">Child FilterBuilder</param>
        /// <returns>FilterBuilder</returns>
        public FilterBuilder WithFilter(FilterBuilder childFilter)
        {
            _childFilters.Add(childFilter);

            return this;
        }

        /// <summary>
        /// Build FilterBuilder
        /// </summary>
        /// <returns>Filter string</returns>
        public string Build()
        {
            var builder = new StringBuilder($"<filter type='{_type}'>");

            foreach (var childFilter in _childFilters)
            {
                builder.Append(childFilter.Build());
            }

            foreach (var condition in _conditions)
            {
                builder.Append(condition);
            }

            builder.Append("</filter>");

            return builder.ToString();
        }
    }
}