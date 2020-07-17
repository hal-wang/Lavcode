using SQLite;

namespace Hubery.Lavcode.Uwp.Helpers
{
    public class SqliteHelper<T> : SQLiteConnection
    {
        public SqliteHelper(string databasePath, bool storeDateTimeAsTicks = true) :
            base(databasePath, storeDateTimeAsTicks)
        {
            CreateTable<T>();
        }
    }
}
