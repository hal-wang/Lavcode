using Newtonsoft.Json;

namespace Lavcode.Service.Api.Dtos
{
    public class CreateFolderDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("icon")]
        public UpsertIconDto Icon { get; set; }
    }
}
