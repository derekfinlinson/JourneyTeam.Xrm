using System.Collections.Generic;
using System.Text;
using Microsoft.Xrm.Sdk.Query;

namespace Xrm
{
    public class FetchXmlBuilder
    {
        private string _fetch;
        private string _entity;
        private readonly List<string> _attributes = new List<string>();
        private readonly List<string> _orders = new List<string>();
        private readonly List<FilterBuilder> _filters = new List<FilterBuilder>();
        private readonly List<string> _linkEntities = new List<string>();
        
        public FetchXmlBuilder WithFetch(bool aggregate = false, int? count = null)
        {
            var builder = new StringBuilder("<fetch");

            if (aggregate)
            {
                builder.Append(" aggregate='true'");
            }

            if (count != null)
            {
                builder.Append($" count='{count}");
            }

            builder.Append(">");

            _fetch = builder.ToString();

            return this;
        }

        public FetchXmlBuilder WithEntity(string logicalName)
        {
            _entity = $"<entity name='{logicalName}'>";

            return this;
        }

        public FetchXmlBuilder WithAttribute(string logicalName, bool groupBy = false, string aggregate = null, string alias = null)
        {
            var builder = new StringBuilder($"<attribute name='{logicalName.ToLower()}'");

            if (groupBy)
            {
                builder.Append(" groupby='true'");
            }

            if (!string.IsNullOrWhiteSpace(aggregate))
            {
                builder.Append($" aggregate='{aggregate.ToLower()}'");
            }

            if (!string.IsNullOrWhiteSpace(alias))
            {
                builder.Append($" alias='{alias}'");
            }

            builder.Append(" />");

            _attributes.Add(builder.ToString());

            return this;
        }

        public FetchXmlBuilder WithFilter(FilterBuilder filter)
        {
            _filters.Add(filter);

            return this;
        }

        public FetchExpression Build()
        {
            var builder = new StringBuilder();

            builder.Append(_fetch);
            builder.Append(_entity);

            foreach (var attribute in _attributes)
            {
                builder.Append(attribute);
            }

            foreach (var order in _orders)
            {
                builder.Append(order);
            }

            foreach (var filter in _filters)
            {
                builder.Append(filter.Build());
            }

            foreach (var linkEntity in _linkEntities)
            {
                builder.Append(linkEntity);
            }

            builder.Append("</entity>");
            builder.Append("</fetch>");

            return new FetchExpression(builder.ToString());
        }
    }
}