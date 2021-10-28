using HTools;
using Lavcode.IService;
using Lavcode.Model;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lavcode.Service.Sqlite
{
    public class ConService : IConService
    {
        public SQLiteConnection Connection { get; set; }

        public async Task Connect(object args)
        {
            Connection = new SQLiteConnection(DynamicHelper.ToExpandoObject(args).FilePath as string);
            await TaskExtend.Run(() =>
            {
                CreateTables();
            });
        }

        public async Task Reconnect(object args)
        {
            Connection?.Dispose();
            Connection = new SQLiteConnection(DynamicHelper.ToExpandoObject(args).FilePath as string);
            await TaskExtend.Run(() =>
            {
                CreateTables();
            });
        }

        private void CreateTables()
        {
            Connection.CreateTable<Folder>();
            Connection.CreateTable<Password>();
            Connection.CreateTable<Icon>();
            Connection.CreateTable<KeyValuePair>();
            Connection.CreateTable<DelectedItem>();
            Connection.CreateTable<Config>();
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }
    }
}
