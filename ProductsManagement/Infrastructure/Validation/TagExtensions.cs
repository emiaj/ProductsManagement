using System.Collections.Generic;
using FubuCore;
using HtmlTags;

namespace ProductsManagement.Infrastructure.Validation
{
    public static class TagExtensions
    {
        public static IDictionary<string, object> Rules(this HtmlTag tag)
        {
            if (!tag.HasMetaData("rules"))
            {
                tag.MetaData("rules", new Dictionary<string, object>());
            }
            return tag.MetaData("rules").As<IDictionary<string, object>>();
        }

        public static IDictionary<string, string> Messages(this HtmlTag tag)
        {
            if (!tag.Rules().ContainsKey("messages"))
            {
                tag.Rules()["messages"] = new Dictionary<string, string>();
            }
            return tag.Rules()["messages"].As<IDictionary<string, string>>();
        }

        public static void Messages(this HtmlTag tag, string key, string message)
        {
            tag.Messages()[key] = message;
        }
        public static void Rules(this HtmlTag tag, string key)
        {
            tag.Rules(key, true);
        }

        public static void Rules(this HtmlTag tag, string key, object value)
        {
            tag.Rules()[key] = value;
        }

    }
}