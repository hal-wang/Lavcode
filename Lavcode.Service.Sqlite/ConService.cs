using HTools;
using Lavcode.IService;
using Lavcode.Model;
using SQLite;
using System;
using System.Diagnostics;
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
            Connection.CreateTable<Folder>();
            Connection.CreateTable<Password>();
            Connection.CreateTable<Icon>();
            Connection.CreateTable<KeyValuePair>();
            Connection.CreateTable<DelectedItem>();
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }

        public void SetProxy(Func<bool> useProxy)
        {
        }
    }
}
