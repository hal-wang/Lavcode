using Lavcode.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lavcode.Service.Api.Dtos
{
    public class GetPasswordDto
    {
        public string Id { get; set; }
        public string FolderId { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string Remark { get; set; }
        public int Order { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public GetIconDto Icon { get; set; }
        public IList<GetKeyValuePairDto> KeyValuePairs { get; set; }

        public PasswordModel ToModel()
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
                Icon = Icon.ToModel(),
                KeyValuePairs = KeyValuePairs.Select(item => item.ToModel()).ToList()
            };
        }
    }
}
