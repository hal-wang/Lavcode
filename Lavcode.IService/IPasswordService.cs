using Lavcode.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lavcode.IService
{
    public interface IPasswordService : IDataService
    {
        public Task DeletePassword(string passwordId, bool record = true);
        public Task AddPassword(PasswordModel password, List<KeyValuePairModel> keyValuePairs = null);
        public Task UpdatePassword(PasswordModel password, bool skipIcon, List<KeyValuePairModel> keyValuePairs = null);
        public Task<List<PasswordModel>> GetPasswords(string folderId);
        public Task<List<PasswordModel>> GetPasswords();
        public Task<List<KeyValuePairModel>> GetKeyValuePairs(string passwordId);
    }
}
