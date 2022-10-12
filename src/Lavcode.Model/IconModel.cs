namespace Lavcode.Model
{
    public class IconModel
    {
        #region static
        public static string DefaultPasswordIcon { get; } = "M571.3 703.6c3.9-53.1-10.1-105.9-39.4-149.7l60-60 87.2 87.2c18.1 18.1 45.3 18.1 63.3 0 18.1-18.1 18.1-45.3 0-63.3l-87.2-87.2 106.3-106.3 87.2 87.2c18.1 18.1 45.3 18.1 63.3 0 18.1-18.1 18.1-45.3 0-63.3L824.8 261l28.1-28.1c17.5-17.5 16.9-45.4-1.2-63.6-18.2-18.2-46.2-18.8-63.7-1.4L465.3 490.6c-43.7-30.8-97-45.8-151.2-42.4-58 3.7-111.7 28.4-151.1 69.6-22.8 22.8-40.5 49-52.8 78.1C98.1 624.6 92 654.6 92 685.1c0 30.4 6.1 60.3 18.2 89.1 12.3 29.1 30 55.4 52.8 78.2l3.3 3.3c44.2 44.2 102.5 68.7 164.6 69.2 63.2 0.4 123.7-24 170.3-68.9l0.2-0.1c40.9-41 65.7-95.1 69.9-152.3z m-133 85.6C379 848.5 284 850 226.5 792.6c-29.7-29.7-45.7-68.5-45.1-109.2 0.6-39 16.6-75.3 45-102.1l0.2-0.2c56.7-56.7 151.7-56.7 211.8 0.1 28.1 28.1 44.2 66 44.2 104 0 37.9-16.2 75.9-44.3 104z";
        public static string DefaultFolderIcon { get; } = "M510.4 243.2c8 27.2 33.6 44.8 60.8 44.8h259.2c0-35.2-28.8-64-64-64H504l6.4 19.2zM484.8 160h281.6c70.4 0 128 57.6 128 128v25.6c30.4 24 51.2 60.8 51.2 102.4v384c0 70.4-57.6 128-128 128H208c-70.4 0-128-57.6-128-128V224c0-70.4 57.6-128 128-128h164.8c46.4 0 89.6 25.6 112 64z m-112 0H208c-35.2 0-64 28.8-64 64v576c0 35.2 28.8 64 64 64h608c35.2 0 64-28.8 64-64V416c0-35.2-28.8-64-64-64H574.4c-56 0-105.6-36.8-121.6-89.6l-19.2-57.6c-8-27.2-32-44.8-60.8-44.8zM272 704h256c17.6 0 32 14.4 32 32s-14.4 32-32 32H272c-17.6 0-32-14.4-32-32s14.4-32 32-32z";
        public static IconType DefaultIconType { get; } = IconType.Path;

        public static IconModel GetDefault(StorageType storageType)
        {
            return new IconModel()
            {
                IconType = DefaultIconType,
                Value = storageType == StorageType.Folder ? DefaultFolderIcon : DefaultPasswordIcon
            };
        }
        #endregion

        public string Id { get; set; }
        public IconType IconType { get; set; }
        public string Value { get; set; }

        public bool IsDefault
        {
            get
            {
                if (Id != default)
                {
                    return false;
                }
                if (IconType != DefaultIconType)
                {
                    return false;
                }
                if (Value != DefaultFolderIcon && Value != DefaultPasswordIcon)
                {
                    return false;
                }

                return true;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is not IconModel icon)
            {
                return false;
            }

            if (icon.Value == Value && icon.IconType == IconType)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public IconModel DeepClone()
        {
            return new IconModel()
            {
                IconType = IconType,
                Id = Id,
                Value = Value,
            };
        }
    }
}