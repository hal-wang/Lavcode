using Lavcode.Model;
using SQLite;

namespace Lavcode.Service.Sqlite.Entities
{
    [Table("KeyValuePair")]
    public class KeyValuePairEntity
    {
        public virtual int Id { get; set; }
        public virtual string SourceId { get; set; }
        public virtual string Key { get; set; }
        public virtual string Value { get; set; }

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