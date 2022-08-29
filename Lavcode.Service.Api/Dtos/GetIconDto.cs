using Lavcode.Model;
using Newtonsoft.Json;

namespace Lavcode.Service.Api.Dtos
{
    public class GetIconDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("iconType")]
        public IconType IconType { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        public IconModel ToModel()
        {
            return new IconModel()
            {
                Id = Id,
                IconType = IconType,
                Value = Value
            };
        }
    }
}
