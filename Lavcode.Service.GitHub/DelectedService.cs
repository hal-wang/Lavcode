using Lavcode.IService;
using Lavcode.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lavcode.Service.GitHub
{
    public class DelectedService : IDelectedService
    {
        private readonly ConService _con;

        public DelectedService(IConService cs)
        {
            _con = cs as ConService;
        }

        public Task<List<DelectedItem>> GetDelectedItems()
        {
            throw new NotImplementedException();
        }

        public Task Add(DelectedItem delectedItem)
        {
            throw new NotImplementedException();
        }

        public Task Add(IList<DelectedItem> delectedItems)
        {
            throw new NotImplementedException();
        }
    }
}
