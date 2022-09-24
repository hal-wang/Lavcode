using Newtonsoft.Json;
using System.Collections.Generic;

namespace Lavcode.Service.Api.Dtos
{
    public class UpdatePasswordDto
    {
        [JsonProperty("folderId")]
        public string FolderId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("remark")]
        public string Remark { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("icon")]
        public UpsertIconDto Icon { get; set; }

        [JsonProperty("keyValuePairs")]
        public IList<UpsertKeyValuePairDto> KeyValuePairs { get; set; }
    }
}
