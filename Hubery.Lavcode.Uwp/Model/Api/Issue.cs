using Newtonsoft.Json;
using System;

namespace Hubery.Lavcode.Uwp.Model.Api
{
    public class Issue
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string State { get; set; }
        public User User { get; set; }
        public int Comments { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
        [JsonProperty("issue_type")]
        public string IssueType { get; set; }
    }
}
