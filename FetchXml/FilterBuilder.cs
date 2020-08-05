using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Xrm
{
    public class FilterBuilder
    {
        private string _type = "and";

        private List<string> _conditions = new List<string>();

        private List<FilterBuilder> _childFilters = new List<FilterBuilder>();
        
        public FilterBuilder WithType(string type)
        {
            _type = type;

            return this;
        }

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