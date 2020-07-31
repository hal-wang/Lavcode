using Newtonsoft.Json;
using System;

namespace Hubery.Lavcode.Uwp.Model.Api
{
    public class Comment
    {
        public string Id { get; set; }
        public string Body { get; set; }
        public User User { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
