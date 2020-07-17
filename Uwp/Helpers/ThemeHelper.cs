using Microsoft.Toolkit.Uwp.UI.Helpers;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Hubery.Lavcode.Uwp.Helpers
{
    public static class ThemeHelper
    {
        public static Action ThemeChanged;

        public static void SetTheme(string color, bool isThemeDark)
        {
            SetAccentColor(GetThemeColor(color));
            SetElementTheme(isThemeDark);

            ThemeChanged?.Invoke();
        }

        private static Color GetThemeColor(string color)
        {
            if (string.IsNullOrEmpty(color))
            {
                color = "#2090e0";
            }

            Color themeColor;
            try
            {
                themeColor = ColorHelper.GetColor(color);
            }
            catch
            {
                color = "#2090e0";
                themeColor = ColorHelper.GetColor(color);
            }

            return themeColor;
        }

        private static void SetElementTheme(bool isThemeDark)
        {
            Frame frame = Window.Current.Content as Frame;
            if (isThemeDark)
            {
                if (frame.RequestedTheme == ElementTheme.Dark)
                {
                    frame.RequestedTheme = ElementTheme.Light;
                }
                frame.RequestedTheme = ElementTheme.Dark;
            }
            else
            {
                if (frame.RequestedTheme == ElementTheme.Light)
                {
                    frame.RequestedTheme = ElementTheme.Dark;
                }

                frame.RequestedTheme = ElementTheme.Light;
            }
        }

        private static void SetAccentColor(Color themeColor)
        {
            Application.Current.Resources["SystemAccentColor"] = themeColor;

            if (Application.Current.Resources.ContainsKey("ThemeForeground"))
            {
                Application.Current.Resources["ThemeForeground"] = ColorHelper.IsDarkColor(themeColor) ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black);
            }
        }

        public static ElementTheme ElementTheme
        {
            get
            {
                if (Window.Current.Content is FrameworkElement element)
                {
                    if (element.RequestedTheme != ElementTheme.Default)
                    {
                        return element.RequestedTheme;
                    }
                }

                return new ThemeListener().CurrentTheme == ApplicationTheme.Dark ? ElementTheme.Dark : ElementTheme.Light;
            }
        }
    }
}