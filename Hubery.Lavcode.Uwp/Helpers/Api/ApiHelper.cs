using Hubery.Tools;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hubery.Lavcode.Uwp.Helpers.Api
{
    public class ApiHelper
    {
        #region Base
        private readonly RequestBase _requestBase;
        private ApiHelper(string url, string controller = null)
        {
            _requestBase = new RequestBase(url ?? Global.GiteeBaseApiUrl, controller);
        }

        public async Task<HttpResponseMessage> Post(string funcName, object param = null) => await _requestBase.Post(funcName, param);
        public async Task<HttpResponseMessage> Get(string funcName) => await _requestBase.Get(funcName);
        #endregion

        #region Instances
        public static ApiHelper Repos { get; } = new ApiHelper(Global.GiteeBaseApiUrl, nameof(Repos).ToLower());
        #endregion
    }
}
