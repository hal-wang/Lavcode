using HTools.Config;
using Lavcode.IService;
using SQLite;
using System;

namespace Lavcode.Service.Sqlite
{
    public class ConfigService : SqliteConfigBase, IConfigService
    {
        public ConfigService(IConService cs) :
            base(new Func<SQLiteConnection>(() => (cs as ConService).Connection))
        {
        }

        public DateTime LastEditTime
        {
            get => Get(DateTime.Now);
            set => Set(value);
        }
    }
}
