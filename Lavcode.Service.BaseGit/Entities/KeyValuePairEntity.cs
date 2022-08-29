using Lavcode.Model;
using Newtonsoft.Json;

namespace Lavcode.Service.BaseGit.Entities
{
    public class KeyValuePairEntity : IEntity
    {
        public string Id { get; set; }

        [JsonProperty("SourceId")]
        public string PasswordId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public KeyValuePairModel ToModel()
        {
            return new KeyValuePairModel()
            {
                Id = Id,
                PasswordId = PasswordId,
                Key = Key,
                Value = Value
            };
        }

        public static KeyValuePairEntity FromModel(KeyValuePairModel model)
        {
            return new KeyValuePairEntity()
            {
                Id = model.Id,
                PasswordId = model.PasswordId,
                Key = model.Key,
                Value = model.Value
            };
        }
    }
}