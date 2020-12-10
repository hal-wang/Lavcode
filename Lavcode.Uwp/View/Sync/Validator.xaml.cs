using Lavcode.Uwp.Helpers;
using HTools;
using HTools.Uwp.Controls.Dialog;
using HTools.Uwp.Helpers;
using System.Security.Cryptography;
using System.Text;
using Windows.UI.Xaml;

namespace Lavcode.Uwp.View.Sync
{
    /// <summary>
    /// 1、使用MD5转换输入的密码
    /// 2、使用1的结果加密文件
    /// 3、将1的结果，使用SHA-2加密后，写入文件头部，用于验证密码正确性
    /// 4、在设置中，记录用户输入的密码
    /// 
    /// 传入shaPassword将是验证状态，验证通过才返回 ContentDialogResult.Primary
    /// </summary>
    public sealed partial class Validator : LayoutDialog
    {
        #region Static
        /// <summary>
        /// 验证密码正确性
        /// </summary>
        /// <param name="shaPassword">存储于文件中的，即用于验证用的</param>
        /// <param name="md5Password">加密文件用的密码，即真实使用的</param>
        /// <returns></returns>
        public static bool IsMd5PasswordTrue(string shaPassword, string md5Password)
        {
            return shaPassword == SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(md5Password)).ToX2().ToUpper();
        }

        /// <summary>
        /// 验证密码正确性
        /// </summary>
        /// <param name="shaPassword">存储于文件中的，即用于验证用的</param>
        /// <param name="password">用户输入的密码，不用于加密，也不用于验证</param>
        /// <returns></returns>
        public static bool IsPasswordTrue(string shaPassword, string password)
        {
            return IsMd5PasswordTrue(shaPassword, MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(password)).ToX2().ToUpper());
        }
        #endregion


        private readonly string _shaPassword; // 用于验证的密码

        /// <summary>
        /// 输入密码，验证密码正确性，获得SHA256(MD5(pswd))处理的结果
        /// </summary>
        /// <param name="shaPassword">用MD5处理后的字符串</param>
        public Validator(string shaPassword = null)
        {
            this.InitializeComponent();

            InputPassword = SettingHelper.Instance.SyncFilePassword;
            _shaPassword = shaPassword;
        }

        public string InputPassword
        {
            get { return (string)GetValue(InputPasswordProperty); }
            set { SetValue(InputPasswordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InputPassword.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InputPasswordProperty =
            DependencyProperty.Register("InputPassword", typeof(string), typeof(Validator), new PropertyMetadata(string.Empty));


        private void LayoutDialog_PrimaryButtonClick(LayoutDialog sender, LayoutDialogButtonClickEventArgs args)
        {
            if (string.IsNullOrEmpty(InputPassword))
            {
                MessageHelper.ShowSticky(PasswordBox, "请输入密码");
                args.Cancel = true;
                return;
            }
            if (InputPassword.Length < 6 || InputPassword.Length > 32)
            {
                MessageHelper.ShowSticky(PasswordBox, "密码长度在 6-32 之间");
                args.Cancel = true;
                return;
            }

            // 需要验证密码，并且验证不通过
            if (_shaPassword != null && !IsPasswordTrue(_shaPassword, InputPassword))
            {
                MessageHelper.ShowDanger("密码错误");
                args.Cancel = true;
                return;
            }

            SettingHelper.Instance.SyncFilePassword = InputPassword;
        }
    }
}
