using System;
using System.Threading.Tasks;

namespace Lavcode.IService
{
    public interface IConService : IDisposable
    {
        public Task Connect(object args);
        public Task Reconnect(object args);
    }
}
