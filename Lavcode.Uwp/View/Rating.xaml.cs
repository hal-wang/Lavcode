using Hubery.Tools.Uwp.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Uwp.View
{
    public sealed partial class Rating : UserControl
    {
        public Rating()
        {
            this.InitializeComponent();
        }

        private async void Rating_Click(object sender, RoutedEventArgs e)
        {
            await PopupHelper.ShowRating();
        }
    }
}
