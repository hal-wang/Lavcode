using HTools.Uwp.Helpers;
using Lavcode.Model;
using Lavcode.Uwp.Modules.SqliteSync.Model;
using System;

namespace Lavcode.Uwp.Helpers
{
    public class SettingHelper : SettingConfigBase
    {
        private SettingHelper() { }
        public static SettingHelper Instance { get; } = new SettingHelper();

        #region 教程
        public bool AddFolderTaught
        {
            get => Get(false);
            set => Set(value);
        }

        public bool AddPasswordTaught
        {
            get => Get(false);
            set => Set(value);
        }

        public bool PasswordListTaught
        {
            get => Get(false);
            set => Set(value);
        }

        public bool SvgTaught
        {
            get => Get(false);
            set => Set(value);
        }

        public bool SyncTaught
        {
            get => Get(false);
            set => Set(value);
        }
        #endregion

        #region Sqlite Sync
        public string DavAccount
        {
            get => Get<string>();
            set => Set(value);
        }

        public string DavPassword
        {
            get => Get<string>();
            set => Set(value);
        }

        public string DavCustomUrl
        {
            get => Get<string>();
            set => Set(value);
        }

        public CloudType DavCloudType
        {
            get => (CloudType)Get((int)CloudType.Jgy);
            set => Set((int)value);
        }

        /// <summary>
        /// 用户输入的备份密码
        /// </summary>
        public string SyncFilePassword
        {
            get => Get<string>();
            set => Set(value);
        }
        #endregion

        /// <summary>
        /// 选中的文件夹ID
        /// </summary>
        public string SelectedFolderId
        {
            get => Get<string>();
            set => Set(value);
        }

        public bool IsFirstInited
        {
            get => Get(false);
            set => Set(value);
        }

        public bool IsBgVisible
        {
            get => Get(true);
            set
            {
                Set(value);
                IsBgVisibleChanged?.Invoke(value);
            }
        }
        public event Action<bool> IsBgVisibleChanged;

        public string GitHubToken
        {
            get => Get<string>();
            set => Set(value);
        }

        public string GiteeToken
        {
            get => Get<string>();
            set => Set(value);
        }

        public string GiteeRefreeToken
        {
            get => Get<string>();
            set => Set(value);
        }

        public Provider Provider
        {
            get => (Provider)Get((int)Provider.GitHub);
            set => Set((int)value);
        }

        public bool IsAuthOpen
        {
            get => Get(true, $"Is{Provider}AuthOpen");
            set => Set(value, $"Is{Provider}AuthOpen");
        }

        public bool IsAutoLogin
        {
            get => Get(true);
            set => Set(value);
        }

        public bool UseProxy
        {
            get => Get(false);
            set => Set(value);
        }
    }
}
