using HTools.Uwp.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.PasswordCore
{
    public sealed partial class PasswordKeyValuePairCustomDialog : ContentDialog
    {
        public PasswordKeyValuePairCustomDialog()
        {
            this.InitializeComponent();
        }


        public string Key
        {
            get { return (string)GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Key.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeyProperty =
            DependencyProperty.Register("Key", typeof(string), typeof(PasswordKeyValuePairCustomDialog), new PropertyMetadata(string.Empty));


        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (string.IsNullOrEmpty(Key))
            {
                args.Cancel = true;
                MessageHelper.ShowWarning("内容不能为空");
                return;
            }
        }
    }
}