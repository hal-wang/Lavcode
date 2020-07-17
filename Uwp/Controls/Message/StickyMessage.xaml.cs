using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Hubery.Lavcode.Uwp.Controls.Message
{
    internal sealed partial class StickyMessage : UserControl, IMessage
    {
        private Flyout _flyout;

        public StickyMessage()
        {
            this.InitializeComponent();

            _flyout = new Flyout()
            {
                Content = this,
                FlyoutPresenterStyle = Resources["StickMsgFlyoutPresenterStyle"] as Style,
                Placement = Windows.UI.Xaml.Controls.Primitives.FlyoutPlacementMode.Bottom
            };
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(StickyMessage), new PropertyMetadata(string.Empty));



        public MessageType MessageType
        {
            get { return (MessageType)GetValue(MessageTypeProperty); }
            set { SetValue(MessageTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MessageType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageTypeProperty =
            DependencyProperty.Register("MessageType", typeof(MessageType), typeof(StickyMessage), new PropertyMetadata(MessageType.Info));



        public void Show(FrameworkElement placementTarget, string text, MessageType messageType)
        {
            MessageType = messageType;
            Text = text;

            _flyout.ShowAt(placementTarget);
        }
    }
}
