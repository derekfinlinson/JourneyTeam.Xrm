using System.Text;
using Microsoft.Xrm.Sdk.Query;

namespace Xrm
{
    public class FetchXmlBuilder : EntityBuilder<FetchXmlBuilder>
    {
        private string _fetch;
        private string _entity;

        public FetchXmlBuilder(bool aggregate = false, int? count = null, bool distinct = false)
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

            if (distinct)
            {
                builder.Append(" distinct='true'");
            }

            builder.Append(">");

            _fetch = builder.ToString();
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
                builder.Append(linkEntity.Build());
            }

            builder.Append("</entity>");
            builder.Append("</fetch>");

            return new FetchExpression(builder.ToString());
        }
    }
}