using Hubery.Lavcode.Uwp;
using Hubery.Lavcode.Uwp.Helpers.Sqlite;
using Hubery.Lavcode.Uwp.Model;
using System.Collections.Generic;

namespace Hubery.Lavcode.Uwp.View.FolderList
{
    public static class FolderHelper
    {
        public static async IAsyncEnumerable<FolderItem> GetFolderItems()
        {
            using var helper = new SqliteHelper();

            foreach (var folder in await helper.GetFolders())
            {
                FolderItem folderItem = null;
                await TaskExtend.Run(() =>
                {
                    folderItem = new FolderItem(folder);
                });
                yield return folderItem;
            }
        }
    }
}
