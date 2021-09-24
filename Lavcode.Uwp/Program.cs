using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;

namespace Lavcode
{
    class Program
    {
        static void Main(string[] args)
        {
            if (AppInstance.GetActivatedEventArgs() is FileActivatedEventArgs fileArgs)
            {
                IStorageItem file = fileArgs.Files.FirstOrDefault();
                if (file != default)
                {
                    var instance = AppInstance.FindOrRegisterInstanceForKey(file.Path);
                    if (instance.IsCurrentInstance)
                    {
                        Windows.UI.Xaml.Application.Start((p) => new App());
                    }
                    else
                    {
                        instance.RedirectActivationTo();
                    }

                    return;
                }
            }

            var singleton = AppInstance.FindOrRegisterInstanceForKey("Singleton");
            if (singleton.IsCurrentInstance)
            {
                Windows.UI.Xaml.Application.Start((p) => new App());
            }
            else
            {
                singleton.RedirectActivationTo();
            }
        }
    }
}
