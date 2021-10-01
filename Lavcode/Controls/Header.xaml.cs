using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Controls
{
    public sealed partial class Header : ContentControl
    {
        public Header()
        {
            this.InitializeComponent();
        }



        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(Header), new PropertyMetadata(string.Empty));


    }
}
