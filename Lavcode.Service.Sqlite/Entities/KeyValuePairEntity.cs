using Lavcode.Model;
using SQLite;
using System;

namespace Lavcode.Service.Sqlite.Entities
{
    [Table("KeyValuePair")]
    public class KeyValuePairEntity
    {
        [PrimaryKey]
        public string Id { get; set; }
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