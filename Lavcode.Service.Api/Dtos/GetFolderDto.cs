using System;

namespace Lavcode.Service.Api.Dtos
{
    public class GetFolderDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public DateTime LastEditTime { get; set; }

        public GetIconDto Icon { get; set; }
    }
}
