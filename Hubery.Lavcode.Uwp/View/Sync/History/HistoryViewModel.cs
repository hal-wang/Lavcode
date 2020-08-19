using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Hubery.Lavcode.Uwp;
using Hubery.Lavcode.Uwp.Helpers;
using Hubery.Lavcode.Uwp.View.Sync.SyncHelper;
using Hubery.Lavcode.Uwp.Helpers.Sqlite;
using Hubery.Lavcode.Uwp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Windows.Storage;

namespace Hubery.Lavcode.Uwp.View.Sync.History
{
    public class HistoryViewModel : ViewModelBase
    {
        private bool _isLoading = false;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { Set(ref _isLoading, value); }
        }

        public ObservableCollection<HistoryItem> HistoryItems { get; } = new ObservableCollection<HistoryItem>();

        private StorageFolder _folder = null;
        public async void Init()
        {
            IsLoading = true;
            await TaskExtend.SleepAsync();

            try
            {
                _folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(Global.SyncHistoryFolder, CreationCollisionOption.OpenIfExists);
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

        private async IAsyncEnumerable<HistoryItem> GetHistoryItems()
        {
            var files = await _folder.GetFilesAsync();
            foreach (var file in files)
            {
                HistoryItem item = null;
                await TaskExtend.Run(() =>
                {
                    using SqliteHelper sqliteHelper = new SqliteHelper(file.Path);
                    FileInfo fileInfo = new FileInfo(file.Path);
                    item = new HistoryItem()
                    {
                        FileName = file.Name,
                        Size = Math.Round((double)fileInfo.Length / 1024, 2),
                        FolderCount = sqliteHelper.Table<Folder>().Count(),
                        PasswordCount = sqliteHelper.Table<Password>().Count(),
                        LastEditTime = sqliteHelper.LastEditTime
                    };
                });
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
