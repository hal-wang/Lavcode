using HTools;
using Lavcode.IService;
using Lavcode.Model;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lavcode.Service.Sqlite
{
    public class KeyValuePairService : IKeyValuePairService
    {
        private readonly SQLiteConnection _con;

        public KeyValuePairService(IConService cs)
        {
            _con = (cs as ConService).Connection;
        }

        public async Task<List<KeyValuePair>> GetKeyValuePairs(string passwordId)
        {
            List<KeyValuePair> result = null;
            await TaskExtend.Run(() =>
            {
                result = _con.Table<KeyValuePair>().Where((item) => item.SourceId == passwordId).ToList();
            });
            return result;
        }
    }
}
