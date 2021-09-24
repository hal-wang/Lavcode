using HTools;
using SQLite;
using System;

namespace Lavcode.Uwp.Model
{
    public class Password : UniqueItem, ICloneable<Password>
    {
        public string FolderId { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [MaxLength(100)]
        public string Value { get; set; }

        [MaxLength(500)]
        public string Remark { get; set; }

        public int Order { get; set; }

        public DateTime LastEditTime { get; set; }

        public Password DeepClone()
        {
            return new Password()
            {
                FolderId = FolderId,
                Id = Id,
                LastEditTime = LastEditTime,
                Order = Order,
                Remark = Remark,
                Title = Title,
                Value = Value
            };
        }

        public Password ShallowClone()
        {
            return DeepClone();
        }
    }
}