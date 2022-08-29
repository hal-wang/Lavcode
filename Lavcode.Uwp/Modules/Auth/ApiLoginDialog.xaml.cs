using HTools.Uwp.Helpers;
using Lavcode.Service.Api;
using Lavcode.Uwp.Helpers;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.Auth
{
    public sealed partial class ApiLoginDialog : ContentDialog, IResultDialog<string>
    {
        public ApiLoginDialog()
        {
            this.InitializeComponent();

            ApiUrl = SettingHelper.Instance.ApiUrl;
        }

        public string Result { get; set; }


        public string ApiUrl
        {
            get { return (string)GetValue(ApiUrlProperty); }
            set { SetValue(ApiUrlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ApiUrl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ApiUrlProperty =
            DependencyProperty.Register("ApiUrl", typeof(string), typeof(ApiLoginDialog), new PropertyMetadata(string.Empty));


        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(ApiLoginDialog), new PropertyMetadata(string.Empty));


        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            args.Cancel = true;

            using var conService = await ConService.CreateTemp(Global.Version, ApiUrl);
            string token;
            try
            {
                token = await conService.Login(Password);
            }
            catch (Exception ex)
            {
                MessageHelper.ShowDanger(ex.Message);
                return;
            }

            SettingHelper.Instance.ApiUrl = ApiUrl;
            SettingHelper.Instance.ApiToken = token;
            Result = token;
            this.Hide();
        }

        #region Static
        public static async Task<string> Login()
        {
            var token = SettingHelper.Instance.ApiToken;
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(SettingHelper.Instance.ApiUrl))
            {
                var loginDialog = new ApiLoginDialog();
                token = await loginDialog.QueueAsync<string>();
            }
            else
            {
                using var conService = await ConService.CreateTemp(Global.Version, SettingHelper.Instance.ApiUrl);
                if (!await conService.VerifyToken(token))
                {
                    MessageHelper.ShowDanger("登录过期，请重新登录");
                    var loginDialog = new ApiLoginDialog();
                    token = await loginDialog.QueueAsync<string>();
                }
            }

            return token;
        }
        #endregion

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            HelpTeachingTip.IsOpen = true;
        }
    }
}
