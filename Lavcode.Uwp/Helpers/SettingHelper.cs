using Lavcode.Uwp.View.Sync;
using HTools.Uwp.Helpers;
using System;

namespace Lavcode.Uwp.Helpers
{
    public class SettingHelper : SettingConfigBase
    {
        private SettingHelper() { }
        public static SettingHelper Instance { get; } = new SettingHelper();

        #region Sync
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
        #endregion


        #region 教程
        public bool AddFolderTaught
        {
            get { return Get(false); }
            set { Set(value); }
        }

        public bool AddPasswordTaught
        {
            get { return Get(false); }
            set { Set(value); }
        }

        public bool PasswordListTaught
        {
            get { return Get(false); }
            set { Set(value); }
        }

        public bool SvgTaught
        {
            get { return Get(false); }
            set { Set(value); }
        }

        public bool SyncTaught
        {
            get { return Get(false); }
            set { Set(value); }
        }
        #endregion


        /// <summary>
        /// 选中的文件夹ID
        /// </summary>
        public string SelectedFolderId
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        /// <summary>
        /// 首次使用
        /// </summary>
        public bool IsFirstUse
        {
            get { return Get(true); }
            set { Set(value); }
        }

        public bool IsAuthOpen
        {
            get { return Get(true); }
            set { Set(value); }
        }

        public bool IsBgVisible
        {
            get { return Get(true); }
            set
            {
                Set(value);
                IsBgVisibleChanged?.Invoke(value);
            }
        }
        public Action<bool> IsBgVisibleChanged;
    }
}