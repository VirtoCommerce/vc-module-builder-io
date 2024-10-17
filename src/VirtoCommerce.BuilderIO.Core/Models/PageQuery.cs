using Newtonsoft.Json;

namespace VirtoCommerce.BuilderIO.Core.Models
{
    public class PageQuery
    {
        /*
            "@type": "@builder.io/core:Query",
            "operator": "is",
            "property": "urlPath",
            "value": "/about"
        */
        [JsonProperty("@type")]
        public string Type { get; set; }
        public string Operator { get; set; }
        public string Property { get; set; }
        public string Value { get; set; }
    }
}
