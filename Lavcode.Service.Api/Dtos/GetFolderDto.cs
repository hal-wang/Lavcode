using Lavcode.Model;
using System;

namespace Lavcode.Service.Api.Dtos
{
    public class GetFolderDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public long UpdatedAt { get; set; }

        public GetIconDto Icon { get; set; }

        public FolderModel ToModel()
        {
            return new FolderModel()
            {
                Id = Id,
                Name = Name,
                Order = Order,
                UpdatedAt = DateTimeOffset.FromUnixTimeMilliseconds(UpdatedAt),
                Icon = Icon.ToModel()
            };
        }
    }
}
