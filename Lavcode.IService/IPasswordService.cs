using Lavcode.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lavcode.IService
{
    public interface IPasswordService : IDataService
    {
        public Task DeletePassword(string passwordId, bool record = true);
        public Task AddPassword(Password password, Icon icon, List<KeyValuePair> keyValuePairs = null);
        public Task UpdatePassword(Password password, Icon icon = null, List<KeyValuePair> keyValuePairs = null);
        public Task<List<Password>> GetPasswords(string folderId);
        public Task<List<Password>> GetPasswords();
        public Task<List<Model.KeyValuePair>> GetKeyValuePairs(string passwordId);
    }
}
