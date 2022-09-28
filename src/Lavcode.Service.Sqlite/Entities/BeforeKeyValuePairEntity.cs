using SQLite;

namespace Lavcode.Service.Sqlite.Entities
{
    [Table("KeyValuePair")]
    public class BeforeKeyValuePairEntity
    {
        public virtual int Id { get; set; }
        public virtual string SourceId { get; set; }
        public virtual string Key { get; set; }
        public virtual string Value { get; set; }
    }
}
