using HTools;
using Lavcode.IService;
using Lavcode.Service.Sqlite.Entities;
using Newtonsoft.Json.Linq;
using SQLite;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Lavcode.Service.Sqlite
{
    public class ConService : IConService
    {
        public SQLiteConnection Connection { get; private set; }

        public Func<bool> UseProxy { get; } = null;

        public async Task<bool> Connect(object args)
        {
            var filePath = DynamicHelper.ToExpandoObject(args).FilePath as string;
            try
            {
                Connection?.Dispose();
                Connection = new SQLiteConnection(filePath);
                await TaskExtend.Run(() =>
                {
                    CreateTables();
                    TryUpdateKvpId();
                });
                return true;
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
                return false;
            }
        }

        private void CreateTables()
        {
            Connection.CreateTable<FolderEntity>();
            Connection.CreateTable<PasswordEntity>();
            Connection.CreateTable<IconEntity>();
            Connection.CreateTable<KeyValuePairEntity>();
            Connection.CreateTable<DelectedEntity>();
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }

        public void SetProxy(Func<bool> useProxy)
        {
        }

        private void TryUpdateKvpId()
        {
            if (!IsKvpIdNeedUpdate()) return;

            var kvps = Connection.Table<KeyValuePairEntity>().ToArray();
            foreach (var kvp in kvps)
            {
                // fix 合并冲突
                kvp.Id = kvp.Id + kvp.PasswordId;
            }
            Connection.RunInTransaction(() =>
            {
                Connection.Execute("drop table [KeyValuePair]");
                Connection.Execute(@"
CREATE TABLE [KeyValuePair] (
	[Id] varchar COLLATE BINARY NOT NULL PRIMARY KEY, 
	[SourceId] varchar COLLATE BINARY, 
	[Key] varchar(100) COLLATE BINARY, 
	[Value] varchar(1000) COLLATE BINARY
)
");
                Connection.InsertAll(kvps);
            });
        }

        private bool IsKvpIdNeedUpdate()
        {
            var tableInfo = Connection.Query<TableInfo>("pragma table_info('KeyValuePair')");
            if (tableInfo.Count != 4)
            {
                throw new Exception();
            }
            var col = tableInfo.FirstOrDefault(item => item.Name == "Id");
            if (col == null)
            {
                return false;
            }

            return col.Type == "integer";
        }
    }
}
