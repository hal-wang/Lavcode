using GalaSoft.MvvmLight;
using Lavcode.Uwp.Helpers.Sqlite;
using System;
using System.Diagnostics;
using Windows.UI.Core;

namespace Lavcode.Uwp.Model
{
    public abstract class IconItem : ObservableObject
    {
        private Icon _icon = null;
        public Icon Icon
        {
            get { return _icon; }
            set { Set(ref _icon, value); }
        }

        internal async void SetIcon(string sourceId)
        {
            try
            {
                using var helper = new SqliteHelper();
                var icon = await helper.GetIcon(sourceId);

                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    try
                    {
                        Icon = icon;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                });

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
