using Lavcode.Model;
using Newtonsoft.Json;

namespace Lavcode.Service.Api.Dtos
{
    public class GetKeyValuePairDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("sourceId")]
        public string SourceId { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        public KeyValuePairModel ToModel()
        {
            return new KeyValuePairModel()
            {
                Id = Id,
                SourceId = SourceId,
                Key = Key,
                Value = Value
            };
        }
    }
}
