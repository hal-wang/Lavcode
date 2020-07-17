using Hubery.Lavcode.Uwp.Controls.Loading;
using System;

namespace Hubery.Lavcode.Uwp.Helpers
{
    public static class LoadingHelper
    {
        #region Loading
        public static Action<bool> IsLoadingChanged;

        private static bool _isLoading = false;
        public static bool IsLoading
        {
            get { return _isLoading; }
            private set
            {
                _isLoading = value;
                IsLoadingChanged?.Invoke(value);
            }
        }

        private static Loading _loading = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadingStr">加载时的文本，空字符串不显示</param>
        /// <param name="clickable">能否点击界面元素</param>
        /// <param name="backgroundOpacity">背景透明的</param>
        public static void Show(string loadingStr = "", bool clickable = false, double backgroundOpacity = 0.5, double paneOpacity = 0.6)
        {
            if (_loading == null)
            {
                _loading = new Loading();
            }

            _loading.Show(loadingStr, clickable, backgroundOpacity, paneOpacity);
            IsLoading = true;
        }

        public static void Hide()
        {
            _loading?.Hide();
            _loading = null;
            IsLoading = false;
        }
        #endregion
    }
}