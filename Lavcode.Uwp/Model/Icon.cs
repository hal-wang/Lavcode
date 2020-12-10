using Hubery.Tools;
using SQLite;

namespace Lavcode.Uwp.Model
{
    public class Icon : ICloneable<Icon>
    {
        [PrimaryKey]
        public string Id { get; set; }

        /// <summary>
        /// 图标类型
        /// </summary>
        public IconType IconType { get; set; }

        /// <summary>
        /// 字体：1字符+字体名称
        /// 图片：Base64字符串
        /// </summary>
        [MaxLength(1024 * 1024 * 10)]
        public string Value { get; set; }

        [Ignore]
        public bool IsDefault
        {
            get
            {
                if (Id != default)
                {
                    return false;
                }
                if (IconType != Global.DefaultIconType)
                {
                    return false;
                }
                if (Value != Global.DefaultFolderIcon && Value != Global.DefaultPasswordIcon)
                {
                    return false;
                }

                return true;
            }
        }

        public static Icon GetDefault(StorageType storageType)
        {
            return new Icon()
            {
                IconType = Global.DefaultIconType,
                Value = storageType == StorageType.Folder ? Global.DefaultFolderIcon : Global.DefaultPasswordIcon
            };
        }

        public Icon DeepClone()
        {
            return new Icon()
            {
                Id = Id,
                IconType = IconType,
                Value = Value,
            };
        }

        public Icon ShallowClone()
        {
            return DeepClone();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Icon icon))
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
    }
}