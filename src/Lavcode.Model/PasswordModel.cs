using System;
using System.Collections.Generic;
using System.Linq;

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
        public DateTimeOffset UpdatedAt { get; set; }

        public IconModel Icon { get; set; }
        public IList<KeyValuePairModel> KeyValuePairs { get; set; }

        public PasswordModel DeepClone()
        {
            return new PasswordModel()
            {
                Id = Id,
                FolderId = FolderId,
                Title = Title,
                Value = Value,
                Remark = Remark,
                Order = Order,
                UpdatedAt = UpdatedAt,
                Icon = Icon?.DeepClone(),
                KeyValuePairs = KeyValuePairs?.Select(item => item.DeepClone())?.ToList()
            };
        }
    }
}