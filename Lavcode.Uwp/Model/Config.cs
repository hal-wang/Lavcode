using SQLite;

namespace Lavcode.Uwp.Model
{
    public class Config
    {
        [PrimaryKey]
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
