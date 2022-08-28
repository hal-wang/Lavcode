using Lavcode.Model;

namespace Lavcode.Service.BaseGit.Entities
{
    public class KeyValuePairEntity : IEntity
    {
        public int Id { get; set; }
        public string SourceId { get; set; }
        public string Key { get; set; }
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

        public static KeyValuePairEntity FromModel(KeyValuePairModel model)
        {
            return new KeyValuePairEntity()
            {
                Id = model.Id,
                SourceId = model.SourceId,
                Key = model.Key,
                Value = model.Value
            };
        }
    }
}