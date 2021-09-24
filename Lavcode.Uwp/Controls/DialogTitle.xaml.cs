using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lavcode.Controls
{
    public sealed partial class DialogTitle : UserControl
    {
        public DialogTitle()
        {
            InitializeComponent();
            Loaded += DialogTitle_Loaded;
        }

        private void DialogTitle_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= DialogTitle_Loaded;

            Width = (Dialog.Content as FrameworkElement).ActualWidth;
            SizeChanged += (ss, ee) => Width = (Dialog.Content as FrameworkElement).ActualWidth;
        }

        public ContentDialog Dialog
        {
            get => (ContentDialog)GetValue(DialogProperty);
            set => SetValue(DialogProperty, value);
        }

        // Using a DependencyProperty as the backing store for Dialog.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DialogProperty =
            DependencyProperty.Register("Dialog", typeof(ContentDialog), typeof(DialogTitle), new PropertyMetadata(null));


        public object Title
        {
            get => GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(object), typeof(DialogTitle), new PropertyMetadata(null));


        public bool Closable
        {
            get => (bool)GetValue(ClosableProperty);
            set => SetValue(ClosableProperty, value);
        }

        // Using a DependencyProperty as the backing store for Closable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClosableProperty =
            DependencyProperty.Register("Closable", typeof(bool), typeof(DialogTitle), new PropertyMetadata(true));


        public object TitleExtension
        {
            get => GetValue(TitleExtensionProperty);
            set => SetValue(TitleExtensionProperty, value);
        }

        public static readonly DependencyProperty TitleExtensionProperty =
            DependencyProperty.Register("TitleExtension", typeof(object), typeof(DialogTitle), new PropertyMetadata(null));

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Dialog?.Hide();
        }
    }
}
