using Lavcode.IService;
using Lavcode.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lavcode.Uwp.View.Sync
{
    public class Merge : IDisposable
    {
        private Service.Sqlite.SqliteHelper _localSqliteHelper;
        private Service.Sqlite.SqliteHelper _remoteSqliteHelper;

        private Merge() { }

        /// <summary>
        /// 自动合并，更新localDb
        /// </summary>
        public static async Task<Merge> OpenAsync(string localDbPath, string remoteDbPath)
        {
            var result = new Merge();
            result._localSqliteHelper = await Service.Sqlite.SqliteHelper.OpenAsync(localDbPath);
            result._remoteSqliteHelper = await Service.Sqlite.SqliteHelper.OpenAsync(remoteDbPath);
            
            return result;
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
            var remoteFolders = await _remoteSqliteHelper.FolderService.GetFolders();
            var localFolders = await _localSqliteHelper.FolderService.GetFolders();
            await UpdateFolders(localFolders, remoteFolders);

            // 更新密码
            var remotePasswords = await _remoteSqliteHelper.PasswordService.GetPasswords();
            var localPasswords = await _localSqliteHelper.PasswordService.GetPasswords();
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

                var remoteIcon = await _remoteSqliteHelper.IconService.GetIcon(remoteFolder.Id);
                await _localSqliteHelper.FolderService.AddFolder(remoteFolder, remoteIcon);
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

                var remoteIcon = await _remoteSqliteHelper.IconService.GetIcon(remotePassword.Id);
                var remoteKvp = await _remoteSqliteHelper.KeyValuePairService.GetKeyValuePairs(remotePassword.Id);
                await _localSqliteHelper.PasswordService.UpdatePassword(remotePassword, remoteIcon, remoteKvp);
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

                var remoteIcon = await _remoteSqliteHelper.IconService.GetIcon(remoteFolder.Id);
                await _localSqliteHelper.FolderService.UpdateFolder(remoteFolder, remoteIcon);
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

                var remoteIcon = await _remoteSqliteHelper.IconService.GetIcon(remotePassword.Id);
                var remoteKvp = await _remoteSqliteHelper.KeyValuePairService.GetKeyValuePairs(remotePassword.Id);
                await _localSqliteHelper.PasswordService.UpdatePassword(remotePassword, remoteIcon, remoteKvp);
            }
        }

        private async Task DeletePasswordItems(List<DelectedItem> delectedItems)
        {
            foreach (var delectedPassword in delectedItems.Where((item) => item.StorageType == StorageType.Password))
            {
                await _localSqliteHelper.PasswordService.DeletePassword(delectedPassword.Id, false);
                await _remoteSqliteHelper.PasswordService.DeletePassword(delectedPassword.Id, false);
            }
        }

        private async Task DeleteFolderItems(List<DelectedItem> delectedItems)
        {
            foreach (var delectedFolder in delectedItems.Where((item) => item.StorageType == StorageType.Folder))
            {
                await _localSqliteHelper.FolderService.DeleteFolder(delectedFolder.Id, false);
                await _remoteSqliteHelper.FolderService.DeleteFolder(delectedFolder.Id, false);
            }
        }

        /// <summary>
        /// 将删除记录全部合并放在一个表
        /// </summary>
        /// <returns></returns>
        private async Task<List<DelectedItem>> MergeDelectedItems()
        {
            var result = new List<DelectedItem>();
            var localDelectedItems = await _localSqliteHelper.DeletedService.GetDelectedItems();
            var remoteDelectedItems = await _remoteSqliteHelper.DeletedService.GetDelectedItems();

            var appendedItems = remoteDelectedItems.Where(
                (remoteDelectedItem) => localDelectedItems.Where(
                    (localDelectedItem) => localDelectedItem.Id == remoteDelectedItem.Id && localDelectedItem.StorageType == remoteDelectedItem.StorageType).Count() == 0)
                .ToList();

            result.AddRange(localDelectedItems);
            result.AddRange(appendedItems);

            await _localSqliteHelper.DeletedService.Add(appendedItems);

            return result;
        }

        //private async Task DeleteDeletedItems()
        //{
        //    await _localDb.DeleteDelectedItems();
        //}

        public void Dispose()
        {
            _localSqliteHelper?.Dispose();
            _remoteSqliteHelper?.Dispose();
        }
    }
}