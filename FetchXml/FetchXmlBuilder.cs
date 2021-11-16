using System.Text;
using Microsoft.Xrm.Sdk.Query;

namespace Xrm
{
    public class FetchXmlBuilder : EntityBuilder
    {
        private string _fetch;
        private string _entity;

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

        public FetchXmlBuilder WithEntity(string logicalName, string alias = null)
        {
            var builder = new StringBuilder($"<entity name='{logicalName}'");

            if (!string.IsNullOrWhiteSpace(alias))
            {
                builder.Append($" alias='{alias}'");
            }

            builder.Append(">");

            _entity = builder.ToString();

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