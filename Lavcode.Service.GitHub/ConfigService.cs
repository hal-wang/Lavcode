using HTools.Config;
using Lavcode.IService;
using System;

namespace Lavcode.Service.GitHub
{
    public class ConfigService : ConfigBase<string>, IConfigService
    {
        public ConfigService(IConService cs)
        {
        }

        public override bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public override void Remove(string key)
        {
            throw new NotImplementedException();
        }

        protected override string GetValue(string key)
        {
            throw new NotImplementedException();
        }

        protected override void SetValue(string value, string key)
        {
            throw new NotImplementedException();
        }

        public DateTime LastEditTime
        {
            get => Get(DateTime.Now);
            set => Set(value);
        }
    }
}
