using Lavcode.IService;
using Lavcode.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Lavcode.Service.BaseGit
{
    public class IconService : IIconService
    {
        private readonly BaseGitConService _con;

        public IconService(IConService cs)
        {
            _con = cs as BaseGitConService;
        }

        public Task<Icon> GetIcon(string sourceId)
        {
            var result = _con.IconIssue.Comments.Where(item => item.Value.Id == sourceId).Select(item => item.Value).FirstOrDefault();
            return Task.FromResult(result);
        }
    }
}
