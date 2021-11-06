using HTools.Config;
using Lavcode.IService;
using Lavcode.Model;
using System;
using System.Linq;

namespace Lavcode.Service.GitHub
{
    public class ConfigService : ConfigBase<string>, IConfigService
    {
        private readonly ConService _con;

        public ConfigService(IConService cs)
        {
            _con = cs as ConService;
        }

        public override bool ContainsKey(string key)
        {
            return _con.ConfigIssue.Comments.Any(cfg => cfg.Value.Key == key);
        }

        public override async void Remove(string key)
        {
            await _con.DeleteComment<Config, string>(key, (item1, item2) => item1.Key == key);
        }

        protected override string GetValue(string key)
        {
            return _con.ConfigIssue.Comments.FirstOrDefault(cfg => cfg.Value.Key == key)?.Value?.Value;
        }

        protected override void SetValue(string value, string key)
        {
            var task = _con.UpsertComment(new Config()
            {
                Key = key,
                Value = value
            }, (item1, item2) => item1.Key == item2.Key);
            task.Wait();
        }

        public DateTime LastEditTime
        {
            get => Get(DateTime.Now);
            set => Set(value);
        }
    }
}
