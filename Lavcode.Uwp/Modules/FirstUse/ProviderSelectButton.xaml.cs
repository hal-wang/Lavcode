using Lavcode.Model;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Lavcode.Uwp.Modules.FirstUse
{
    public sealed partial class ProviderSelectButton : UserControl
    {

        public event TypedEventHandler<ProviderSelectButton, Provider> OnSelect;

        public ProviderSelectButton()
        {
            this.InitializeComponent();
        }


        public Geometry IconData
        {
            get { return (Geometry)GetValue(IconDataProperty); }
            set { SetValue(IconDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconDataProperty =
            DependencyProperty.Register("IconData", typeof(Geometry), typeof(ProviderSelectButton), new PropertyMetadata(null));



        public Provider Provider
        {
            get { return (Provider)GetValue(ProviderProperty); }
            set { SetValue(ProviderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Provider.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProviderProperty =
            DependencyProperty.Register("Provider", typeof(Provider), typeof(ProviderSelectButton), new PropertyMetadata(Provider.Sqlite));



        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Description.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(ProviderSelectButton), new PropertyMetadata(string.Empty));

        private void Border_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            OnSelect?.Invoke(this, Provider);
        }
    }
}
