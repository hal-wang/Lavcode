using GalaSoft.MvvmLight.Messaging;
using HTools.Uwp.Helpers;
using Lavcode.Uwp.Common;
using Lavcode.Uwp.Modules.Auth;
using Lavcode.Uwp.Modules.Shell;
using System;
using System.Linq;
using System.Web;
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
                    Frame.Navigate(typeof(AuthPage), e.Arguments);
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
                ViewModelLocator.Register<Service.Sqlite.ConService>();
                Frame.Navigate(typeof(ShellPage), file);
            }
            Window.Current.Activate();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.Protocol)
            {
                var eventArgs = args as ProtocolActivatedEventArgs;
                var query = HttpUtility.ParseQueryString(eventArgs.Uri.Query);
                if (!query.AllKeys.Contains("provider")) return;
                switch (query["provider"])
                {
                    case "github":
                        Messenger.Default.Send(query, "OnGithubNotify");
                        break;
                }
            }
        }

        private void CreateFrame()
        {
            SettingHelper.Instance.Provider = SettingHelper.Instance.SettingProvider;

            if (Window.Current.Content is not Windows.UI.Xaml.Controls.Frame)
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