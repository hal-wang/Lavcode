using Lavcode.Uwp.Model;
using Hubery.Tools;
using System.Threading.Tasks;

namespace Lavcode.Uwp.Helpers.Sqlite
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
