using Lavcode.IService;
using Lavcode.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Diagnostics;
using Windows.UI.Core;

namespace Lavcode.Uwp.Modules.PasswordCore
{
    public abstract class IconItem : ObservableObject
    {
        private Icon _icon = null;
        public Icon Icon
        {
            get { return _icon; }
            set { SetProperty(ref _icon, value); }
        }

        internal async void SetIcon(string sourceId)
        {
            try
            {
                var iconService = ServiceProvider.Services.GetService<IIconService>();
                var icon = await iconService.GetIcon(sourceId);

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
