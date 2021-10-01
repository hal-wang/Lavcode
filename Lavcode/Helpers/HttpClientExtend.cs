using HTools;
using HTools.Uwp.Helpers;
using System;
using System.Net.Http;

namespace Lavcode.Helpers
{
    public static class HttpClientExtend
    {
        public static bool IsResErr(this HttpResponseMessage httpResponse)
        {
            if (httpResponse.IsSuccessStatusCode) return false;

            new Action(async () => MessageHelper.ShowDanger(await httpResponse.GetErrorMessage())).Invoke();
            return true;
        }
    }
}
