using System;

namespace Lavcode.Model
{
    public class FolderModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public DateTime LastEditTime { get; set; }

        public IconModel Icon { get; set; }
    }
}