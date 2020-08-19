using Hubery.Lavcode.Uwp;
using Hubery.Lavcode.Uwp.Model;
using System.Threading.Tasks;

namespace Hubery.Lavcode.Uwp.Helpers.Sqlite
{
    public partial class SqliteHelper
    {
        public async Task<Icon> GetIcon(string sourceId)
        {
            Icon result = null;
            await TaskExtend.Run(() =>
            {
                result = Table<Icon>().Where((item) => item.Id == sourceId).FirstOrDefault();
            });
            return result;
        }
    }
}
