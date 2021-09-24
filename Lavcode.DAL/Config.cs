using Lavcode.Model;
using HTools;
using System;
using System.Runtime.CompilerServices;

namespace Lavcode.DAL
{
    public partial class SqliteHelper
    {
        private T GetConfig<T>(T defaultValue = default, [CallerMemberName] string key = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("键不能为空");
            }

            var result = Table<Config>().Where((item) => item.Key == key).FirstOrDefault();
            if (result == default)
            {
                return defaultValue;
            }
            else
            {
                return DataExtend.GetValue<T>(result.Value);
            }
        }

        private void SetConfig<T>(T value, [CallerMemberName] string key = null)
        {
            if (string.IsNullOrEmpty(key))
            {

                throw new Exception("键不能为空");
            }

            InsertOrReplace(new Config()
            {
                Key = key,
                Value = value.ToString()
            });
        }

        /// <summary>
        /// 最后编辑日期
        /// </summary>
        public DateTime LastEditTime
        {
            get => GetConfig(DateTime.Now);
            set => SetConfig(value);
        }
    }
}
