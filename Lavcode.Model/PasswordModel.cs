using System;

namespace Lavcode.Model
{
    public class PasswordModel
    {
        public string Id { get; set; }
        public string FolderId { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string Remark { get; set; }
        public int Order { get; set; }
        public DateTime LastEditTime { get; set; }
    }
}