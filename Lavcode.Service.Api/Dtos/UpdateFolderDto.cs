using Newtonsoft.Json;

namespace Lavcode.Service.Api.Dtos
{
    public class UpdateFolderDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("icon")]
        public UpsertIconDto Icon { get; set; }
    }
}
