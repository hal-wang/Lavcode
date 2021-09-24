using Lavcode.Model;
using HTools;
using System.Threading.Tasks;

namespace Lavcode.DAL
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
