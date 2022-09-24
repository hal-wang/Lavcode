using Lavcode.Model;
using Newtonsoft.Json;
using System;

namespace Lavcode.Service.Api.Dtos
{
    public class GetFolderDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("updatedAt")]
        public long UpdatedAt { get; set; }

        [JsonProperty("icon")]
        public GetIconDto Icon { get; set; }

        public FolderModel ToModel()
        {
            return new FolderModel()
            {
                Id = Id,
                Name = Name,
                Order = Order,
                UpdatedAt = DateTimeOffset.FromUnixTimeMilliseconds(UpdatedAt),
                Icon = Icon.ToModel()
            };
        }
    }
}
