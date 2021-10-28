using HTools.Uwp.Helpers;
using Lavcode.Model;
using System;

namespace Lavcode.Uwp.Common
{
    public class SettingHelper : SettingConfigBase
    {
        private SettingHelper() { }
        public static SettingHelper Instance { get; } = new SettingHelper();

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

        public string GitHubToken
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public Provider Provider
        {
            get => (Provider)Get((int)Provider.Sqlite);
            set => Set((int)value);
        }
    }
}