using Lavcode.Uwp.Helpers.Sqlite;
using Lavcode.Uwp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lavcode.Uwp.View.Sync
{
    public class Merge : IDisposable
    {
        private readonly SqliteHelper _localDb = null;
        private readonly SqliteHelper _remoteDb = null;

        /// <summary>
        /// 自动合并，更新localDb
        /// </summary>
        public Merge(string localDbPath, string remoteDbPath)
        {
            _localDb = new SqliteHelper(localDbPath);
            _remoteDb = new SqliteHelper(remoteDbPath);
        }

        /// <summary>
        /// 自动合并。
        /// 先删除云端和本地删除过的。
        /// 再根据时间修改成最新的。
        /// 最后添加没有的。
        /// 最终本地版为最新
        /// </summary>
        /// <returns></returns>
        public async Task AutoMerge()
        {
            // 删除
            var delectedItems = await MergeDelectedItems();
            await DeleteFolderItems(delectedItems);
            await DeletePasswordItems(delectedItems);

            // 更新文件夹
            var remoteFolders = await _remoteDb.GetFolders();
            var localFolders = await _localDb.GetFolders();
            await UpdateFolders(localFolders, remoteFolders);

            // 更新密码
            var remotePasswords = await _remoteDb.GetPasswords();
            var localPasswords = await _localDb.GetPasswords();
            await UpdatePasswords(localPasswords, remotePasswords);

            // 添加
            await AddFolders(localFolders, remoteFolders);
            await AddPasswords(localPasswords, remotePasswords, localFolders);

            // 删除“删除记录”
            //await DeleteDeletedItems();
        }

        private async Task AddFolders(List<Folder> localFolders, List<Folder> remoteFolders)
        {
            foreach (var remoteFolder in remoteFolders)
            {
                var localFolder = localFolders.Where((item) => item.Id == remoteFolder.Id).FirstOrDefault();
                if (localFolder != default)
                {
                    continue;
                }

                var remoteIcon = await _remoteDb.GetIcon(remoteFolder.Id);
                await _localDb.AddFolder(remoteFolder, remoteIcon);
            }
        }

        private async Task AddPasswords(List<Password> localPasswords, List<Password> remotePasswords, List<Folder> localFolders)
        {
            foreach (var remotePassword in remotePasswords)
            {
                var localPassword = localPasswords.Where((item) => item.Id == remotePassword.Id).FirstOrDefault();
                if (localPassword != default || localFolders.Where((item) => item.Id == remotePassword.FolderId).Count() == 0)
                {
                    continue;
                }

                var remoteIcon = await _remoteDb.GetIcon(remotePassword.Id);
                var remoteKvp = await _remoteDb.GetKeyValuePairs(remotePassword.Id);
                await _localDb.UpdatePassword(remotePassword, remoteIcon, remoteKvp);
            }
        }

        private async Task UpdateFolders(List<Folder> localFolders, List<Folder> remoteFolders)
        {
            foreach (var localFolder in localFolders)
            {
                var remoteFolder = remoteFolders.Where((item) => item.Id == localFolder.Id).FirstOrDefault();
                if (remoteFolder == default || remoteFolder.LastEditTime <= localFolder.LastEditTime)
                {
                    continue;
                }

                var remoteIcon = await _remoteDb.GetIcon(remoteFolder.Id);
                await _localDb.UpdateFolder(remoteFolder, remoteIcon);
            }
        }

        private async Task UpdatePasswords(List<Password> localPasswords, List<Password> remotePasswords)
        {
            foreach (var localPassword in localPasswords)
            {
                var remotePassword = remotePasswords.Where((item) => item.Id == localPassword.Id).FirstOrDefault();
                if (remotePassword == default || remotePassword.LastEditTime <= localPassword.LastEditTime)
                {
                    continue;
                }

                var remoteIcon = await _remoteDb.GetIcon(remotePassword.Id);
                var remoteKvp = await _remoteDb.GetKeyValuePairs(remotePassword.Id);
                await _localDb.UpdatePassword(remotePassword, remoteIcon, remoteKvp);
            }
        }

        private async Task DeletePasswordItems(List<DelectedItem> delectedItems)
        {
            foreach (var delectedPassword in delectedItems.Where((item) => item.StorageType == StorageType.Password))
            {
                await _localDb.DeletePassword(delectedPassword.Id, false);
                await _remoteDb.DeletePassword(delectedPassword.Id, false);
            }
        }

        private async Task DeleteFolderItems(List<DelectedItem> delectedItems)
        {
            foreach (var delectedFolder in delectedItems.Where((item) => item.StorageType == StorageType.Folder))
            {
                await _localDb.DeleteFolder(delectedFolder.Id, false);
                await _remoteDb.DeleteFolder(delectedFolder.Id, false);
            }
        }

        /// <summary>
        /// 将删除记录全部合并放在一个表
        /// </summary>
        /// <returns></returns>
        private async Task<List<DelectedItem>> MergeDelectedItems()
        {
            var result = new List<DelectedItem>();
            var localDelectedItems = await _localDb.GetDelectedItems();
            var remoteDelectedItems = await _remoteDb.GetDelectedItems();

            var appendedItems = remoteDelectedItems.Where(
                (remoteDelectedItem) => localDelectedItems.Where(
                    (localDelectedItem) => localDelectedItem.Id == remoteDelectedItem.Id && localDelectedItem.StorageType == remoteDelectedItem.StorageType).Count() == 0);

            result.AddRange(localDelectedItems);
            result.AddRange(appendedItems);

            foreach (var item in appendedItems)
            {
                _localDb.Insert(item);
            }

            return result;
        }

        //private async Task DeleteDeletedItems()
        //{
        //    await _localDb.DeleteDelectedItems();
        //}

        public void Dispose()
        {
            _localDb?.Dispose();
            _remoteDb?.Dispose();
        }
    }
}