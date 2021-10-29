using HTools;
using HTools.Uwp.Helpers;
using Lavcode.Uwp.SqliteSync;
using Lavcode.Uwp.SqliteSync.View;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Lavcode.Uwp.View.Sync.SyncHelper
{
    internal abstract class BaseSyncHelper
    {
        protected const int EachSize = 1024 * 128; // 128KB

        protected async Task<StorageFolder> GetTempFolder() =>
            await ApplicationData.Current.TemporaryFolder.CreateFolderAsync(Constant.SyncTempFolderName, CreationCollisionOption.OpenIfExists);

        protected string Password => string.IsNullOrEmpty(SettingHelper.Instance.SyncFilePassword) ? null : MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(SettingHelper.Instance.SyncFilePassword)).ToX2().ToUpper();

        public bool IsAutoVerified { get; set; }

        #region 加密/解密
        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="encryptedFile"></param>
        /// <returns></returns>
        protected async Task<StorageFile> DecryptFile(StorageFile encryptedFile)
        {
            var tempFile = await (await GetTempFolder()).CreateFileAsync(Constant.SyncTempUnencryptedFileName, CreationCollisionOption.ReplaceExisting);
            using (IRandomAccessStream readStream = await encryptedFile.OpenReadAsync(), writeStream = await tempFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                #region 验证密码
                using (IInputStream stream = readStream.GetInputStreamAt(0))
                {
                    using DataReader reader = new DataReader(stream);
                    if (readStream.Size <= (uint)(Constant.SyncFileHeaderLength))
                    {
                        MessageHelper.ShowDanger("未能正确解析文件");
                        return null;
                    }
                    await reader.LoadAsync((uint)(Constant.SyncFileHeaderLength));
                    var header = reader.ReadString((uint)Constant.SyncFileHeaderLength);
                    if (!Regex.IsMatch(header, $"^{Constant.SyncFileHeader}[0-9A-F]{{{Constant.Sha2ByteLength}}}$"))
                    {
                        MessageHelper.ShowDanger("未能正确解析文件");
                        return null;
                    }

                    if (!await Verify(header.Substring(Constant.SyncFileHeader.Length, Constant.Sha2ByteLength)))
                    {
                        return null;
                    }
                }
                #endregion

                using IOutputStream outputStream = writeStream.GetOutputStreamAt(0);
                using IInputStream inputStream = readStream.GetInputStreamAt((uint)Constant.SyncFileHeaderLength);

                using CryptoStream cryptoStream = new CryptoStream(inputStream.AsStreamForRead(), GetCryptoTransform(SqliteSync.Crypto.OperationType.Decrypt), CryptoStreamMode.Read, false);
                await cryptoStream.CopyToAsync(outputStream.AsStreamForWrite(), (int)readStream.Size - Constant.SyncFileHeaderLength);
            }
            return tempFile;
        }

        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="tempFile"></param>
        /// <returns></returns>
        protected async Task<StorageFile> EncryptFile(StorageFile tempFile)
        {
            if (!await SetPassword())
            {
                return null;
            }

            var encryptedFile = await (await GetTempFolder()).CreateFileAsync(Constant.SyncTempEncryptedFileName, CreationCollisionOption.ReplaceExisting);
            using (IRandomAccessStream writeStream = await encryptedFile.OpenAsync(FileAccessMode.ReadWrite), readStream = await tempFile.OpenReadAsync())
            {
                //写入头部
                using (IOutputStream stream = writeStream.GetOutputStreamAt(0))
                {
                    using DataWriter dataWriter = new DataWriter(stream);
                    dataWriter.WriteString(Constant.SyncFileHeader);
                    dataWriter.WriteString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(Password)).ToX2().ToUpper());
                    await dataWriter.StoreAsync();
                }

                using IOutputStream outputStream = writeStream.GetOutputStreamAt((uint)Constant.SyncFileHeaderLength);
                using IInputStream inputStream = readStream.GetInputStreamAt(0);

                using CryptoStream cryptoStream = new CryptoStream(outputStream.AsStreamForWrite(), GetCryptoTransform(SqliteSync.Crypto.OperationType.Encrypt), CryptoStreamMode.Write, false);
                await inputStream.AsStreamForRead().CopyToAsync(cryptoStream, (int)readStream.Size);
            }
            return encryptedFile;
        }

        private ICryptoTransform GetCryptoTransform(SqliteSync.Crypto.OperationType operationType)
            => SqliteSync.Crypto.Aes.GetCryptoTransform(Encoding.UTF8.GetBytes(Password), Constant.AesIv, operationType);

        #region 密码
        private async Task<bool> SetPassword()
        {
            if (!string.IsNullOrEmpty(Password))
            {
                return true;
            }

            return await new Validator().QueueAsync() == Windows.UI.Xaml.Controls.ContentDialogResult.Primary;
        }

        private async Task<bool> Verify(string password)
        {
            IsAutoVerified = false;
            if (!string.IsNullOrEmpty(Password))
            {
                if (Validator.IsMd5PasswordTrue(password, Password))
                {
                    IsAutoVerified = true;
                    return true;
                }

                MessageHelper.ShowDanger("密码错误");
            }

            return await new Validator(password).QueueAsync() == Windows.UI.Xaml.Controls.ContentDialogResult.Primary;
        }
        #endregion
        #endregion


        /// <summary>
        /// 拷贝一份本地数据库文件，用于同步操作
        /// </summary>
        /// <returns></returns>
        public async Task<StorageFile> GetTempLocalFile()
        {
            var dbFile = await ApplicationData.Current.LocalFolder.GetFileAsync(Global.SqliteFileName);
            var tempFolder = await GetTempFolder();
            return await dbFile.CopyAsync(tempFolder, Constant.SyncTempLocalFileName, NameCollisionOption.ReplaceExisting);
        }

        /// <summary>
        /// 替换本地数据库，并将之前的数据库移至历史文件夹
        /// </summary>
        /// <param name="storageFile"></param>
        /// <returns></returns>
        public async Task ReplaceDbFile(StorageFile storageFile)
        {
            var historyFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(Constant.SyncHistoryFolder, CreationCollisionOption.OpenIfExists);
            var dbFile = await ApplicationData.Current.LocalFolder.GetFileAsync(Global.SqliteFileName);

            // 将数据库文件复制到历史记录
            await dbFile.CopyAsync(historyFolder, DateTime.Now.ToString("yyMMddHHmmssff"), NameCollisionOption.FailIfExists);

            // 再将文件覆盖本地数据库文件
            await storageFile.CopyAndReplaceAsync(dbFile);
        }
    }
}
