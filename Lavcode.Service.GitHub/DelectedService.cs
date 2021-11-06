using Lavcode.IService;
using Lavcode.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lavcode.Service.GitHub
{
    public class DelectedService : IDelectedService
    {
        private readonly ConService _con;

        public DelectedService(IConService cs)
        {
            _con = cs as ConService;
        }

        public Task<List<DelectedItem>> GetDelectedItems()
        {
            var result = _con.DelectedItemIssue.Comments.Select(item => item.Value).ToList();
            return Task.FromResult(result);
        }

        public async Task Add(DelectedItem delectedItem)
        {
            await _con.CreateComment(delectedItem);
        }

        public async Task Add(IList<DelectedItem> delectedItems)
        {
            foreach (var item in delectedItems)
            {
                await Add(item);
            }
        }
    }
}
