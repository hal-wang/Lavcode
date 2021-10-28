using GalaSoft.MvvmLight.Ioc;
using HTools.Uwp.Helpers;
using Lavcode.Uwp.Common;
using Lavcode.Uwp.ViewModel;
using Microsoft.Toolkit.Uwp.UI.Helpers;
using System;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Lavcode.Uwp.View
{
    public sealed partial class MainPage : Page
    {
        private bool _inited = false;
        private readonly ThemeListener _themeListener = new();
        public MainPage()
        {
            DataContext = VM;
            InitializeComponent();
            TitleBarHelper.SetTitleBar();
            _themeListener.ThemeChanged += ThemeListener_ThemeChanged;
            Loaded += MainPage_Loaded;
        }

        public MainViewModel VM { get; } = SimpleIoc.Default.GetInstance<MainViewModel>();

        private async void MainPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (_inited)
            {
                return;
            }
            _inited = true;

            if (SettingHelper.Instance.IsFirstUse)
            {
                await new FirstUseDialog().QueueAsync();
                SettingHelper.Instance.IsFirstUse = false;
            }

            if (param is not null and StorageFile file)
            {
                Global.OpenedFile = file;
                FindName(nameof(SyncFileHandler));
            }
            else
            {
                LoadUI();
            }
        }

        ~MainPage()
        {
            _themeListener.ThemeChanged -= ThemeListener_ThemeChanged;
        }

        private void ThemeListener_ThemeChanged(ThemeListener sender)
        {
            TitleBarHelper.SetTitleBar();
        }

        private object param = null;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Frame.BackStack.Clear();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            param = e.Parameter;
        }

        /// <summary>
        /// lazy load
        /// </summary>
        public void LoadUI()
        {
            FindName(nameof(PasswordDetail));
            FindName(nameof(PasswordList));
            FindName(nameof(FolderList));
        }
    }
}
