using SQLite;

namespace Hubery.Lavcode.Uwp.Model
{
    public class KeyValuePair
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string SourceId { get; set; }

        [MaxLength(100)]
        public string Key { get; set; }

        [MaxLength(500)]
        public string Value { get; set; }
    }
}