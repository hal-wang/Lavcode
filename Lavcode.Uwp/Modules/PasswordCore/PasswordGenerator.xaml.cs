using Microsoft.Extensions.DependencyInjection;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.Modules.PasswordCore
{
    public sealed partial class PasswordGenerator : UserControl
    {
        public PasswordGenerator()
        {
            DataContext = VM;
            this.InitializeComponent();
        }

        public PasswordGeneratorViewModel VM { get; } = ServiceProvider.Services.GetService<PasswordGeneratorViewModel>();

        public event TypedEventHandler<PasswordGenerator, string> PasswordChanged;

        private void GenerateBtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            PasswordChanged?.Invoke(this, VM.GeneratePassword());
        }
    }
}