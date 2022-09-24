using Lavcode.Model;
using Newtonsoft.Json;

namespace Lavcode.Service.Api.Dtos
{
    public class UpsertIconDto
    {
        [JsonProperty("iconType")]
        public IconType IconType { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
