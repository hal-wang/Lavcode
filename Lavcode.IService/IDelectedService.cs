using Lavcode.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lavcode.IService
{
    public interface IDelectedService : IDataService
    {
        public Task<List<DelectedItem>> GetDelectedItems();
        public Task Add(DelectedItem delectedItem);
        public Task Add(IList<DelectedItem> delectedItems);
    }
}
