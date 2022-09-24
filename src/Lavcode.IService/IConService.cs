using System;
using System.Threading.Tasks;

namespace Lavcode.IService
{
    public interface IConService : IDisposable
    {
        public Func<bool> UseProxy { get; }
        public Task<bool> Connect(object args);
        public Task<bool> Refresh();
        public void SetProxy(Func<bool> useProxy);
    }
}
