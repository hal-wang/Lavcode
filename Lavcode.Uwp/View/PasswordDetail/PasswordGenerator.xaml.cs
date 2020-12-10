using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.View.PasswordDetail
{
    public sealed partial class PasswordGenerator : UserControl
    {
        public PasswordGenerator()
        {
            this.InitializeComponent();
        }

        public event TypedEventHandler<PasswordGenerator, string> PasswordChanged;

        private void GenerateBtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            PasswordChanged?.Invoke(this, Model.GeneratePassword());
        }
    }
}