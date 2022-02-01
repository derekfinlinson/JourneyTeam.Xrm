using System.Collections.Generic;
using System.Text;

namespace Xrm
{
    public class EntityBuilder<T> where T : EntityBuilder<T>
    {
        protected readonly List<string> _attributes = new List<string>();
        protected readonly List<FilterBuilder> _filters = new List<FilterBuilder>();
        protected readonly List<LinkEntityBuilder> _linkEntities = new List<LinkEntityBuilder>();

        public T WithAttributes(params string[] attributes)
        {
            foreach (var attribute in attributes)
            {
                _attributes.Add($"<attribute name='{attribute.ToLower()}' />");
            }

            return this as T;
        }

        public T WithAttribute(string logicalName, bool groupBy = false, string aggregate = null, string alias = null)
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

            return this as T;
        }

        public T WithFilter(FilterBuilder filter)
        {
            _filters.Add(filter);

            return this as T;
        }

        public T WithLinkEntity(LinkEntityBuilder linkEntity)
        {
            _linkEntities.Add(linkEntity);

            return this as T;
        }
    }
}