using System;
using System.Threading.Tasks;

namespace Lavcode.IService
{
    public interface IConService : IDisposable
    {
        public Task<bool> Connect(object args);
    }
}
