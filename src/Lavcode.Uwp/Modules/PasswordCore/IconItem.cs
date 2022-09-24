using Lavcode.Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Diagnostics;
using Windows.UI.Core;

namespace Lavcode.Uwp.Modules.PasswordCore
{
    public abstract class IconItem : ObservableObject
    {
        private IconModel _icon = null;
        public IconModel Icon
        {
            get { return _icon; }
            set { SetProperty(ref _icon, value); }
        }

        internal async void SetIcon(IconModel icon)
        {
            try
            {
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
