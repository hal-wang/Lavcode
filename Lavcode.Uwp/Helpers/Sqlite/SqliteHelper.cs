using Lavcode.Uwp.Model;
using SQLite;
using System;

namespace Lavcode.Uwp.Helpers.Sqlite
{
    public partial class SqliteHelper : SQLiteConnection
    {
        public static Action OnDbChanged;

        private void DbChanged()
        {
            LastEditTime = DateTime.Now;
            OnDbChanged?.Invoke();
        }

        private static bool _inited = false;
        public SqliteHelper() :
            base(Global.DbFilePath)
        {
            if (_inited)
            {
                return;
            }
            _inited = true;

            CreateTables();
        }

        public SqliteHelper(string sqliteFilePath) :
            base(sqliteFilePath)
        {
            CreateTables();
        }

        private void CreateTables()
        {
            CreateTable<Folder>();
            CreateTable<Password>();
            CreateTable<Icon>();
            CreateTable<KeyValuePair>();
            CreateTable<DelectedItem>();
            CreateTable<Config>();
        }
    }
}