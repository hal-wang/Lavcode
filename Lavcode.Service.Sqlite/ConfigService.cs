using HTools.Config;
using Lavcode.IService;
using System;

namespace Lavcode.Service.Sqlite
{
    public class ConfigService : SqliteConfigBase, IConfigService
    {
        public ConfigService(IConService cs) :
            base((cs as ConService).Connection)
        {
        }

        public DateTime LastEditTime
        {
            get => Get(DateTime.Now);
            set => Set(value);
        }
    }
}
