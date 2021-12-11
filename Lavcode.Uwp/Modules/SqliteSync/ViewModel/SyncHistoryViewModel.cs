using GalaSoft.MvvmLight;
using HTools;
using HTools.Uwp.Helpers;
using Lavcode.Model;
using Lavcode.Service.Sqlite;
using Lavcode.Uwp.View.Sync.SyncHelper;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Windows.Storage;

namespace Lavcode.Uwp.Modules.SqliteSync.ViewModel
{
    public class SyncHistoryViewModel : ViewModelBase
    {
        private bool _isLoading = false;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { Set(ref _isLoading, value); }
        }

        public ObservableCollection<SyncHistoryItem> HistoryItems { get; } = new();

        private StorageFolder _folder = null;
        public async void Init()
        {
            if (_folder != null) return;

            IsLoading = true;
            await TaskExtend.SleepAsync();

            try
            {
                _folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(SqliteSyncConstant.SyncHistoryFolder, CreationCollisionOption.OpenIfExists);
                await foreach (var historyItem in GetHistoryItems())
                {
                    HistoryItems.Add(historyItem);
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async IAsyncEnumerable<SyncHistoryItem> GetHistoryItems()
        {
            var files = await _folder.GetFilesAsync();
            foreach (var file in files)
            {
                using var sqliteHelper = await SqliteHelper.OpenAsync(file.Path);
                var fileInfo = new FileInfo(file.Path);
                var item = new SyncHistoryItem()
                {
                    FileName = file.Name,
                    Size = Math.Round((double)fileInfo.Length / 1024, 2),
                    FolderCount = (sqliteHelper.ConService as ConService).Connection.Table<Folder>().Count(),
                    PasswordCount = (sqliteHelper.ConService as ConService).Connection.Table<Password>().Count(),
                    LastEditTime = sqliteHelper.ConfigService.LastEditTime
                };
                yield return item;
            }
        }

        public async void Delete(string fileName)
        {
            IsLoading = true;
            await TaskExtend.SleepAsync();

            try
            {
                if (await PopupHelper.ShowDialog("确认删除该记录？此操作不可恢复！", "确认删除", "确定", "点错了", false) != Windows.UI.Xaml.Controls.ContentDialogResult.Primary)
                {
                    return;
                }

                await (await _folder.GetFileAsync(fileName)).DeleteAsync();

                var item = HistoryItems.Where((item) => item.FileName == fileName).FirstOrDefault();
                HistoryItems.Remove(item);

                MessageHelper.ShowPrimary("删除完成");
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async void Export(string fileName)
        {
            IsLoading = true;
            await TaskExtend.SleepAsync();

            try
            {
                var file = await _folder.GetFileAsync(fileName);

                ISyncHelper syncHelper = FileSyncHelper.Create();
                if (!await syncHelper.SetFile(file))
                {
                    return;
                }

                MessageHelper.ShowPrimary("导出完成");
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
