using System;
using System.Collections.Generic;

namespace Lavcode.Model
{
    public class PasswordModel
    {
        public PasswordModel()
        {
            Icon = IconModel.GetDefault(StorageType.Password);
            KeyValuePairs = new List<KeyValuePairModel>();
        }

        public string Id { get; set; }
        public string FolderId { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string Remark { get; set; }
        public int Order { get; set; }
        public DateTime LastEditTime { get; set; }

        public IconModel Icon { get; set; }
        public IList<KeyValuePairModel> KeyValuePairs { get; set; }
    }
}