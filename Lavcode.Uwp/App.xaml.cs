using HTools.Uwp.Helpers;
using Lavcode.Uwp.Helpers;
using Lavcode.Uwp.View;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Lavcode.Uwp
{
    sealed partial class App : Application
    {
        public static Frame Frame { get; private set; }

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            UnhandledException += App_UnhandledException;
        }

        private void App_UnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            try
            {
                MessageHelper.ShowError(e.Exception, 0, "UnhandledException" + sender?.GetType()?.ToString());
                e.Handled = true;
            }
            catch { }
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            CreateFrame();

            if (!e.PrelaunchActivated)
            {
                if (Frame.Content == null)
                {
                    if (SystemInformation.Instance.IsFirstRun || !SettingHelper.Instance.IsAuthOpen)
                    {
                        Frame.Navigate(typeof(MainPage), e.Arguments);
                    }
                    else
                    {
                        Frame.Navigate(typeof(AuthPage), e.Arguments);
                    }
                }
                Window.Current.Activate();
            }
        }

        protected override void OnFileActivated(FileActivatedEventArgs e)
        {
            CreateFrame();

            StorageFile file = e.Files.FirstOrDefault() as StorageFile;
            if (Frame.Content == null)
            {
                Frame.Navigate(typeof(MainPage), file);
            }
            Window.Current.Activate();
        }

        private void CreateFrame()
        {
            if (!(Window.Current.Content is Frame))
            {
                Frame = new Frame();
                Frame.NavigationFailed += OnNavigationFailed;
                Frame.Navigated += OnNavigated;
                SystemNavigationManager.GetForCurrentView().BackRequested += BackRequested;

                Window.Current.Content = Frame;
            }
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Frame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            MessageHelper.ShowDanger("跳转失败" + e.SourcePageType.FullName);
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (LoadingHelper.IsLoading ||
                Frame == null ||
                e.Handled ||
                !Frame.CanGoBack ||
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility == AppViewBackButtonVisibility.Collapsed)
            {
                return;
            }

            e.Handled = true;
            Frame.GoBack();
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }
    }
}