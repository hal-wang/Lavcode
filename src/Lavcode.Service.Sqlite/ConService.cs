using HTools;
using Lavcode.IService;
using Lavcode.Service.Sqlite.Entities;
using SQLite;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lavcode.Service.Sqlite
{
    public class ConService : IConService
    {
        public SQLiteConnection Connection { get; private set; }

        public Func<bool> UseProxy { get; } = null;

        private object _args;
        public async Task<bool> Connect(object args)
        {
            _args = args;

            var filePath = DynamicHelper.ToExpandoObject(args).FilePath as string;
            Connection?.Dispose();
            Connection = new SQLiteConnection(filePath);
            await TaskExtend.Run(() =>
            {
                TryUpdateKvpId();
                CreateTables();
            });
            return true;
        }

        public virtual async Task<bool> Refresh()
        {
            return await Connect(_args);
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

            var index = 1;
            var kvps = Connection.Table<BeforeKeyValuePairEntity>().ToArray().Select(item => new KeyValuePairEntity()
            {
                // fix 合并冲突
                Id = item.Id + item.SourceId + index++,
                PasswordId = item.SourceId,
                Key = item.Key,
                Value = item.Value,
            });
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
            if (tableInfo.Count == 0) return false;
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
