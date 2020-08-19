using System.Net.Http;
using System.Threading.Tasks;

namespace Hubery.Lavcode.Uwp.Helpers.Api
{
    public class ApiHelper
    {
        #region Base
        private readonly RequestHelper _requestHelper;
        private ApiHelper(string url, string controller = null)
        {
            _requestHelper = new RequestHelper(url ?? Global.GiteeBaseApiUrl, controller);
        }

        public async Task<HttpResponseMessage> Post(string funcName, object param = null) => await _requestHelper.Post(funcName, param);
        public async Task<HttpResponseMessage> Get(string funcName) => await _requestHelper.Get(funcName);
        #endregion

        #region Instances
        public static ApiHelper Repos { get; } = new ApiHelper(Global.GiteeBaseApiUrl, nameof(Repos).ToLower());
        #endregion
    }
}
