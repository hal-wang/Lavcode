using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Storage;

namespace Hubery.Lavcode.Uwp.Helpers.Logger
{
    public class LogHelper
    {
        private static readonly string _path = Path.Combine(ApplicationData.Current.LocalFolder.Path, SystemInformation.ApplicationName + _extendName);
        public static LogHelper Instance = new LogHelper();

        protected const string _extendName = ".log1";

        public bool Log(Log log)
        {
            try
            {
                using (SqliteHelper<Log> sqliteHelper = new SqliteHelper<Log>(_path, storeDateTimeAsTicks: false))
                {
                    sqliteHelper.Insert(log);
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        public void Log(Exception ex, [CallerMemberName] string source = null)
        {
            Log(ex?.ToString(), source);
        }

        public void Log(string content, [CallerMemberName] string source = null)
        {
            Log(new Log()
            {
                Source = source,
                Content = content,
                Time = DateTime.Now,
                Version = $"{SystemInformation.ApplicationVersion.Major}.{SystemInformation.ApplicationVersion.Minor}.{SystemInformation.ApplicationVersion.Build}",
                OperatingSystemVersion = SystemInformation.OperatingSystemVersion.ToString(),
                DeviceModel = SystemInformation.DeviceModel,
                AppName = SystemInformation.ApplicationName,
                Platform = Platform.UWP.ToString(),
            });
        }

        public List<Log> GetLastLogs(int num)
        {
            using SqliteHelper<Log> sqliteHelper = new SqliteHelper<Log>(_path, storeDateTimeAsTicks: false);
            return sqliteHelper.Table<Log>().OrderByDescending((item) => item.ID).Take(num).ToList();
        }

        public async Task Export()
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker
            {
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop,
                SuggestedFileName = SystemInformation.ApplicationName
            };
            savePicker.FileTypeChoices.Add("log", new List<string>() { ".log" });
            StorageFile targetFile = await savePicker.PickSaveFileAsync();
            if (targetFile == null)
            {
                return;
            }

            StorageFile storageFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(SystemInformation.ApplicationName + _extendName, CreationCollisionOption.OpenIfExists);
            await storageFile.CopyAndReplaceAsync(targetFile);
        }
    }
}