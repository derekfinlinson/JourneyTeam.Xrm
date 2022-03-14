using System.Reflection.Emit;
using System.Collections.Generic;
using System.Text;

namespace Xrm
{
    public class EntityBuilder<T> where T : EntityBuilder<T>
    {
        protected readonly List<string> _attributes = new List<string>();
        protected readonly List<FilterBuilder> _filters = new List<FilterBuilder>();
        protected readonly List<LinkEntityBuilder> _linkEntities = new List<LinkEntityBuilder>();

        /// <summary>
        /// Add attributes to entity
        /// </summary>
        /// <param name="attributes">Attributes to add</param>
        /// <returns></returns>
        public T WithAttributes(params string[] attributes)
        {
            foreach (var attribute in attributes)
            {
                _attributes.Add($"<attribute name='{attribute.ToLower()}' />");
            }

            return this as T;
        }

        /// <summary>
        /// Add attribute to entity
        /// </summary>
        /// <param name="logicalName">Logical name of attribute</param>
        /// <param name="groupBy">Group by</param>
        /// /// <param name="dateGrouping">Date grouping</param>
        /// <param name="aggregate">Aggregate</param>
        /// <param name="alias">Alias</param>
        /// <returns></returns>
        public T WithAttribute(string logicalName, bool groupBy = false, string dateGrouping = null, string aggregate = null, string alias = null)
        {
            var builder = new StringBuilder($"<attribute name='{logicalName.ToLower()}'");

            if (groupBy)
            {
                builder.Append(" groupby='true'");

                if (!string.IsNullOrWhiteSpace(dateGrouping))
                {
                    builder.Append($" dategrouping='{dateGrouping.ToLower()}'");
                }
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

        /// <summary>
        /// Add filter
        /// </summary>
        /// <param name="filter">FilterBuilder</param>
        /// <returns></returns>
        public T WithFilter(FilterBuilder filter)
        {
            _filters.Add(filter);

            return this as T;
        }

        /// <summary>
        /// Add link entity
        /// </summary>
        /// <param name="linkEntity">LinkEntityBuilder</param>
        /// <returns></returns>
        public T WithLinkEntity(LinkEntityBuilder linkEntity)
        {
            _linkEntities.Add(linkEntity);

            return this as T;
        }
    }
}