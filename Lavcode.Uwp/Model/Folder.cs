using Hubery.Tools;
using SQLite;
using System;

namespace Lavcode.Uwp.Model
{
    public class Folder : UniqueItem, ICloneable<Folder>
    {
        [MaxLength(100)]
        public string Name { get; set; }

        public int Order { get; set; }

        public DateTime LastEditTime { get; set; }

        public Folder DeepClone()
        {
            return new Folder()
            {
                Id = Id,
                LastEditTime = LastEditTime,
                Name = Name,
                Order = Order
            };
        }

        public Folder ShallowClone()
        {
            return DeepClone();
        }
    }
}