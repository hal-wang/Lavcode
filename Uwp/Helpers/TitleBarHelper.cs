using System;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace Hubery.Lavcode.Uwp.Helpers
{
    public class TitleBarHelper
    {
        public static Action<Color?> TitleBarColorChanging;

        public static void SetTitleBar(Color? titlebarColor = null)
        {
            bool isBtnTransparent = titlebarColor == null || titlebarColor.Value.R == 0;
            if (titlebarColor == null)
            {
                titlebarColor = Colors.Transparent;
            }

            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;

            Color backColor = ColorHelper.MergeAlpha(titlebarColor.Value);
            bool isBackDark = ColorHelper.IsDarkColor(backColor);

            try
            {
                TitleBarColorChanging?.Invoke(backColor);
            }
            catch { }

            titleBar.ButtonForegroundColor =
                titleBar.ButtonHoverForegroundColor =
                titleBar.ButtonInactiveForegroundColor =
                titleBar.ButtonPressedForegroundColor =
                titleBar.ForegroundColor =
                titleBar.InactiveForegroundColor = isBackDark ? Colors.White : Colors.Black;
            titleBar.BackgroundColor =
            titleBar.ButtonBackgroundColor =
            titleBar.ButtonInactiveBackgroundColor = isBtnTransparent ? Colors.Transparent : backColor;

            Color lightButtonColor = backColor;
            Color lighterButtonColor = backColor;
            if (isBackDark)
            {
                lightButtonColor.R = Calc(lightButtonColor.R, 20);
                lightButtonColor.G = Calc(lightButtonColor.G, 20);
                lightButtonColor.B = Calc(lightButtonColor.B, 20);


                lighterButtonColor.R = Calc(lighterButtonColor.R, 30);
                lighterButtonColor.G = Calc(lighterButtonColor.G, 30);
                lighterButtonColor.B = Calc(lighterButtonColor.B, 30);
            }
            else
            {
                lightButtonColor.R = Calc(lightButtonColor.R, -20);
                lightButtonColor.G = Calc(lightButtonColor.G, -20);
                lightButtonColor.B = Calc(lightButtonColor.B, -20);


                lighterButtonColor.R = Calc(lighterButtonColor.R, -30);
                lighterButtonColor.G = Calc(lighterButtonColor.G, -30);
                lighterButtonColor.B = Calc(lighterButtonColor.B, -30);
            }
            titleBar.ButtonHoverBackgroundColor = lightButtonColor;
            titleBar.ButtonPressedBackgroundColor = lighterButtonColor;
        }

        private static byte Calc(byte value, int num)
        {
            if (num > 0)
            {
                if (value + (byte)num >= 255)
                {
                    return 255;
                }
                else
                {
                    return (byte)(value + (byte)num);
                }
            }
            else
            {
                if (value - (byte)-num <= 0)
                {
                    return 0;
                }
                else
                {
                    return (byte)(value - (byte)-num);
                }
            }
        }
    }
}