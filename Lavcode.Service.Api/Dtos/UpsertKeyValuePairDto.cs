using Newtonsoft.Json;

namespace Lavcode.Service.Api.Dtos
{
    public class UpsertKeyValuePairDto
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
