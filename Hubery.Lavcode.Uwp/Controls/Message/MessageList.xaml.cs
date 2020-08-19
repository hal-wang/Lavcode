using System.Collections.ObjectModel;
using Windows.UI.Xaml;

namespace Hubery.Lavcode.Uwp.Controls.Message
{
    internal sealed partial class MessageList : PopupLayout
    {
        public MessageList()
        {
            this.InitializeComponent();

            Messages.CollectionChanged += Messages_CollectionChanged;
        }

        private void Messages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (Messages.Count > 0)
            {
                IsOpen = true;
            }
            else
            {
                IsOpen = false;
            }
        }

        public ObservableCollection<MessageItem> Messages { get; } = new ObservableCollection<MessageItem>();

        public void ShowMessage(string content, MessageType messageType, int duration)
        {
            var item = new MessageItem(content, messageType, duration);
            Messages.Add(item);

            if (duration > 0)
            {
                DispatcherTimer timer = new DispatcherTimer()
                {
                    Interval = item.Duration
                };
                timer.Tick += (ss, ee) =>
                {
                    if (Messages.Contains(item))
                    {
                        Messages.Remove(item);
                    }
                    timer.Stop();
                };
                timer.Start();
            }
        }

        private void Hide_Click(object sender, RoutedEventArgs e)
        {
            MessageItem item = (sender as FrameworkElement)?.DataContext as MessageItem;
            if (item == null)
            {
                return;
            }

            if (Messages.Contains(item))
            {
                Messages.Remove(item);
            }
        }
    }
}
