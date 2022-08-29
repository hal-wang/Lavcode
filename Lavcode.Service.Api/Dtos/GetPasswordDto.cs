using Lavcode.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lavcode.Service.Api.Dtos
{
    public class GetPasswordDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

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

        [JsonProperty("updatedAt")]
        public long UpdatedAt { get; set; }

        [JsonProperty("icon")]
        public GetIconDto Icon { get; set; }

        [JsonProperty("keyValuePairs")]
        public IList<GetKeyValuePairDto> KeyValuePairs { get; set; }

        public PasswordModel ToModel()
        {
            return new PasswordModel()
            {
                Id = Id,
                FolderId = FolderId,
                Title = Title,
                Value = Value,
                Remark = Remark,
                Order = Order,
                UpdatedAt = DateTimeOffset.FromUnixTimeMilliseconds(UpdatedAt),
                Icon = Icon.ToModel(),
                KeyValuePairs = KeyValuePairs.Select(item => item.ToModel()).ToList()
            };
        }
    }
}
