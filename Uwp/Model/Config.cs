using SQLite;

namespace Hubery.Lavcode.Uwp.Model
{
    public class Config
    {
        [PrimaryKey]
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
