using Lavcode.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lavcode.IService
{
    public interface IKeyValuePairService : IDataService
    {
        public Task<List<KeyValuePair>> GetKeyValuePairs(string passwordId);
    }
}
