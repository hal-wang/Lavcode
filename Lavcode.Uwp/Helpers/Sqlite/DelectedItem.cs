using Lavcode.Uwp.Model;
using Hubery.Tools;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lavcode.Uwp.Helpers.Sqlite
{
    public partial class SqliteHelper
    {
        public async Task<List<DelectedItem>> GetDelectedItems()
        {
            List<DelectedItem> result = null;
            await TaskExtend.Run(() =>
            {
                result = Table<DelectedItem>().ToList();
            });
            return result;
        }

        public async Task DeleteDelectedItems()
        {
            await TaskExtend.Run(() =>
            {
                Table<DelectedItem>().Delete();
            });
        }
    }
}
