using Lavcode.IService;
using Lavcode.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Lavcode.Service.GitHub
{
    public class IconService : IIconService
    {
        private readonly ConService _con;

        public IconService(IConService cs)
        {
            _con = cs as ConService;
        }

        public Task<Icon> GetIcon(string sourceId)
        {
            var result = _con.IconIssue.Comments.Where(item => item.Value.Id == sourceId).Select(item => item.Value).FirstOrDefault();
            return Task.FromResult(result);
        }
    }
}
