using System.Text;

namespace Xrm
{
    public class LinkEntityBuilder : EntityBuilder<LinkEntityBuilder>
    {
        private string _linkEntity;

        public LinkEntityBuilder(string table, string from, string to, string alias = null, string linkType = "inner")
        {
            var builder = new StringBuilder("<link-entity");

            builder.Append($" name='{table}' from='{from}' to='{to}' link-type='{linkType}'");

            if (!string.IsNullOrWhiteSpace(alias))
            {
                builder.Append($" alias='{alias}'");
            }

            builder.Append(">");

            _linkEntity = builder.ToString();
        }

        /// <summary>
        /// Build link entity
        /// </summary>
        /// <returns></returns>
        public string Build()
        {
            var builder = new StringBuilder(_linkEntity);

            foreach (var attribute in _attributes)
            {
                builder.Append(attribute);
            }

            foreach (var filter in _filters)
            {
                builder.Append(filter.Build());
            }

            foreach (var linkEntity in _linkEntities)
            {
                builder.Append(linkEntity.Build());
            }

            builder.Append("</link-entity>");

            return builder.ToString();
        }
    }
}