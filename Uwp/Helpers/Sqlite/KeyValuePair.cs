using Hubery.Lavcode.Uwp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hubery.Lavcode.Uwp.Helpers.Sqlite
{
    public partial class SqliteHelper
    {
        public async Task<List<Model.KeyValuePair>> GetKeyValuePairs(string passwordId)
        {
            List<Model.KeyValuePair> result = null;
            await TaskExtend.Run(() =>
            {
                result = Table<Model.KeyValuePair>().Where((item) => item.SourceId == passwordId).ToList();
            });
            return result;
        }
    }
}
