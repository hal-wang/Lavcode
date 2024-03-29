﻿using Microsoft.Toolkit.Mvvm.ComponentModel;
using Windows.UI.Xaml.Media;

namespace Lavcode.Uwp.Modules.PasswordCore.IconCtl
{
    internal class IconControlViewModel : ObservableObject
    {
        private Geometry _pathIcon = null;
        public Geometry PathIcon
        {
            get { return _pathIcon; }
            set { SetProperty(ref _pathIcon, value); }
        }

        private ImageSource _imgIcon = null;
        public ImageSource ImgIcon
        {
            get { return _imgIcon; }
            set { SetProperty(ref _imgIcon, value); }
        }

        private string _segoeMDL2Icon = string.Empty;
        public string SegoeMDL2Icon
        {
            get { return _segoeMDL2Icon; }
            set { SetProperty(ref _segoeMDL2Icon, value); }
        }
    }
}
