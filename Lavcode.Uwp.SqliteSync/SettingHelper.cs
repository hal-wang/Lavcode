using HTools.Uwp.Helpers;
using Lavcode.Uwp.SqliteSync.Model;

namespace Lavcode.Uwp.SqliteSync
{
    public class SettingHelper : SettingConfigBase
    {
        private SettingHelper() { }
        public static SettingHelper Instance { get; } = new SettingHelper();

        public string DavAccount
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string DavPassword
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string DavCustomUrl
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public CloudType DavCloudType
        {
            get { return (CloudType)Get((int)CloudType.Jgy); }
            set { Set((int)value); }
        }

        /// <summary>
        /// 用户输入的备份密码
        /// </summary>
        public string SyncFilePassword
        {
            get { return Get<string>(); }
            set { Set(value); }
        }
    }
}