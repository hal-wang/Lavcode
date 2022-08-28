using SQLite;
using System;

namespace Lavcode.Model
{
    public class Folder : UniqueItem
    {
        [MaxLength(100)]
        public string Name { get; set; }

        public int Order { get; set; }

        public DateTime LastEditTime { get; set; }
    }
}