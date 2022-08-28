using SQLite;
using System;

namespace Lavcode.Model
{
    public class Password : UniqueItem
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
    }
}