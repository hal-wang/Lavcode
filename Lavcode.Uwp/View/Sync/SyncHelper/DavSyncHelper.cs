using Lavcode.Uwp.Helpers;
using Hubery.Tools.Uwp.Helpers;
using System;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Lavcode.Uwp.View.Sync.SyncHelper
{
    internal class DavSyncHelper : BaseSyncHelper, ISyncHelper
    {
        private DavSyncHelper() { }

        private const int _timeOut = 20 * 2000; // 20s

        private readonly string _folderUrl = Global.RemoteBaseUrl + "/" + Global.RemoteRootFolder;
        private readonly string _fileUrl = Global.RemoteBaseUrl + "/" + Global.RemoteRootFolder + "/" + Global.RemoteFileName;


        #region 初始化
        public async static Task<ISyncHelper> Create()
        {
            var result = new DavSyncHelper();

            if (!await result.Init())
            {
                return null;
            }

            return result;
        }


        /// <summary>
        /// 远程连接前必须调用。
        /// 创建文件夹，以此验证登录信息正确性。
        /// </summary>
        /// <returns></returns>
        private async Task<bool> Init()
        {
            if (!string.IsNullOrEmpty(SettingHelper.Instance.DavAccount) && !string.IsNullOrEmpty(SettingHelper.Instance.DavPassword))
            {
                try
                {
                    await GetWebRequest("MKCOL", _folderUrl, _timeOut).GetResponseAsync();
                    return true;
                }
                catch (WebException ex)
                {
                    if (HandleException(ex) != HttpStatusCode.Unauthorized)
                    {
                        return false;
                    }
                }
            }


            var cdr = await new LoginDialog().ShowAsync();
            if (cdr != Windows.UI.Xaml.Controls.ContentDialogResult.Primary)
            {
                return false;
            }

            return await Init();
        }
        #endregion


        private WebRequest GetWebRequest(string method, string url, int timeOut = Timeout.Infinite)
        {
            WebRequest req = WebRequest.Create(url);
            req.Credentials = new NetworkCredential(SettingHelper.Instance.DavAccount, SettingHelper.Instance.DavPassword);
            req.Method = method;
            req.Timeout = timeOut;
            return req;
        }

        private HttpStatusCode? HandleException(WebException ex)
        {
            switch (ex.Status)
            {
                case WebExceptionStatus.Timeout:
                    MessageHelper.ShowDanger("连接超时，请检查网络设置");
                    return null;
                case WebExceptionStatus.ProtocolError:
                    var statusCode = ((HttpWebResponse)ex.Response).StatusCode;
                    switch (statusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            MessageHelper.ShowDanger("账号或密码不正确，请修改后重试");
                            break;
                        case HttpStatusCode.Forbidden:
                            MessageHelper.ShowDanger("服务器拒绝请求，请检查网盘设置");
                            break;
                        case HttpStatusCode.NotFound:
                            MessageHelper.ShowDanger("云端未找到备份文件");
                            break;
                        default:
                            MessageHelper.ShowDanger(ex.Message);
                            break;
                    }
                    return statusCode;
                case WebExceptionStatus.NameResolutionFailure:
                    MessageHelper.ShowDanger("当前网络不可用，请检查网络设置");
                    return null;
                case WebExceptionStatus.ConnectFailure:
                    MessageHelper.ShowDanger("连接失败，请检查网络设置");
                    return null;
                case WebExceptionStatus.UnknownError:
                    MessageHelper.ShowDanger("未知网络错误，请检查网络设置");
                    return null;
                default:
                    MessageHelper.ShowDanger(ex.Message);
                    return null;
            }
        }

        /// <summary>
        /// 从云端获取文件
        /// </summary>
        /// <returns></returns>
        public async Task<StorageFile> GetFile()
        {
            HttpWebResponse res;
            try
            {
                res = (HttpWebResponse)await GetWebRequest("GET", _fileUrl).GetResponseAsync();
            }
            catch (WebException ex)
            {
                HandleException(ex);
                return null;
            }

            var remoteStream = res.GetResponseStream();
            var tempFile = await (await GetTempFolder()).CreateFileAsync(Global.SyncTempEncryptedFileName, CreationCollisionOption.ReplaceExisting);
            using (IRandomAccessStream localStream = await tempFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                int length;
                do
                {
                    var data = new byte[EachSize];
                    length = await remoteStream.ReadAsync(data, 0, data.Length);
                    await localStream.WriteAsync(data.AsBuffer(0, length));
                } while (length > 0);
            }

            return await DecryptFile(tempFile);
        }


        /// <summary>
        /// 上传文件到云端
        /// </summary>
        /// <param name="source"></param>
        /// <param name="repickFileCallback"></param>
        /// <returns></returns>
        public async Task<bool> SetFile(StorageFile source, Action<StorageFile> repickFileCallback = null)
        {
            WebRequest req = GetWebRequest("PUT", _fileUrl);
            var encryptedFile = await EncryptFile(source);
            if (encryptedFile == null)
            {
                return false;
            }

            using var fileStream = await encryptedFile.OpenReadAsync();
            var reqStream = await req.GetRequestStreamAsync();
            while (fileStream.Position < fileStream.Size)
            {
                var data = new byte[Math.Min(EachSize, fileStream.Size - fileStream.Position)];
                await fileStream.ReadAsync(data.AsBuffer(), (uint)data.Length, InputStreamOptions.None);
                await reqStream.WriteAsync(data, 0, data.Length);
            }

            try
            {
                await req.GetResponseAsync();
                return true;
            }
            catch (WebException ex)
            {
                HandleException(ex);
                return false;
            }
        }
    }
}
