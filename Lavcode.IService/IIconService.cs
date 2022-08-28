using Lavcode.Model;
using System.Threading.Tasks;

namespace Lavcode.IService
{
    public interface IIconService : IDataService
    {
        public Task<IconModel> GetIcon(string sourceId);
    }
}
