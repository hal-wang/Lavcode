using Newtonsoft.Json;

namespace Hubery.Lavcode.Uwp.Model.Api
{
    public class User
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }

        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }
        public string Url { get; set; }

        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }

        // more

        public string Type { get; set; }
    }
}
