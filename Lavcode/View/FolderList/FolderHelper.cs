using HTools;
using Lavcode.DAL;
using Lavcode.ViewModel;
using System.Collections.Generic;

namespace Lavcode.View.FolderList
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
