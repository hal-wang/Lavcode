using HTools;
using Lavcode.IService;
using Lavcode.Model;
using SQLite;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Lavcode.Service.Sqlite
{
    public class ConService : IConService
    {
        public SQLiteConnection Connection { get; private set; }

        public async Task<bool> Connect(object args)
        {
            var filePath = DynamicHelper.ToExpandoObject(args).FilePath as string;
            if (!File.Exists(filePath))
            {
                return false;
            }

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
            Connection.CreateTable<Config>();
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }
    }
}
