using Lavcode.Model;

namespace Lavcode.Service.Api.Dtos
{
    public class GetIconDto
    {
        public string Id { get; set; }
        public IconType IconType { get; set; }
        public string Value { get; set; }
    }
}
