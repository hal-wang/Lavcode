using HTools;
using HTools.Uwp.Helpers;
using Lavcode.Uwp.Helpers;
using Microsoft.UI.Xaml.Controls;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Lavcode.Uwp.Modules.Guide
{
    public class GuideHandler
    {
        private static readonly GuideSettingMapItem[] SettingMaps = new GuideSettingMapItem[]
        {
            new GuideSettingMapItem()
            {
                Field= nameof(SettingHelper.AddFolderTaught),
                Children=new string[] {
                    nameof(SettingHelper.AddPasswordTaught),
                    nameof(SettingHelper.PasswordListTaught),
                },
            },
            new GuideSettingMapItem()
            {
                Field=nameof(SettingHelper.AddPasswordTaught),
                Children=new string[] {
                    nameof(SettingHelper.PasswordListTaught),
                },
            },
        };

        public int Total { get; set; }
        public string SettingField { get; set; }
        public string Type { get; set; }
        public int Index { get; set; } = 1;

        private Queue<OneOf<Func<bool>, Func<Task<bool>>>> Actions { get; } = new();

        public GuideHandler Add(GuideItem gi)
        {
            var index = Index;
            Index++;
            Actions.Enqueue(new Func<Task<bool>>(async () =>
             {
                 if (SettingHelper.Instance.Get(false, SettingField))
                 {
                     return false;
                 }

                 var teachingTip = new TeachingTip()
                 {
                     Title = $"{gi.Title}（{Type} {index}/{Total}）",
                     Subtitle = gi.Content,
                     ActionButtonContent = index == Total ? null : "跳过",
                     CloseButtonContent = (index == Total ? "完成" : "下一步") + $" {index}/{Total}",
                     CloseButtonStyle = ResourcesHelper.GetResource<Style>("AccentButtonStyle"),
                     PreferredPlacement = gi.Placement,
                 };
                 teachingTip.ActionButtonClick += (ss, ee) =>
                 {
                     SettingHelper.Instance.Set(true, SettingField);

                     if (SettingMaps.Any(map => map.Field == SettingField))
                     {
                         foreach (var field in SettingMaps.First(map => map.Field == SettingField).Children)
                         {
                             SettingHelper.Instance.Set(true, field);
                         }
                     }

                     teachingTip.IsOpen = false;
                 };
                 var result = await PopupHelper.ShowTeachingTipAsync(gi.Target, teachingTip);
                 return result.Reason == TeachingTipCloseReason.CloseButton;
             }));

            return this;
        }

        public GuideHandler Add(Func<bool> action)
        {
            Actions.Enqueue(new Func<bool>(() =>
            {
                if (SettingHelper.Instance.Get(false, SettingField))
                {
                    return false;
                }

                return action.Invoke();
            }));
            return this;
        }

        public GuideHandler Add(Action action)
        {
            Add(() =>
            {
                action.Invoke();
                return true;
            });
            return this;
        }

        public GuideHandler Add(Func<Task<bool>> action)
        {
            Actions.Enqueue(new Func<Task<bool>>(async () =>
            {
                if (SettingHelper.Instance.Get(false, SettingField))
                {
                    return false;
                }

                return await action.Invoke();
            }));
            return this;
        }

        public GuideHandler Add(Func<Task> action)
        {
            Add(async () =>
            {
                await action.Invoke();
                return true;
            });
            return this;
        }

        public GuideHandler End()
        {
            Actions.Enqueue(new Func<bool>(() =>
            {
                if (!SettingHelper.Instance.Get(false, SettingField))
                {
                    SettingHelper.Instance.Set(true, SettingField);
                }
                return false;
            }));
            return this;
        }

        public async Task RunAsync()
        {
            do
            {
                var action = Actions.Dequeue();
                if (action.IsT0)
                {
                    if (!action.AsT0.Invoke())
                    {
                        break;
                    }
                }
                if (action.IsT1)
                {
                    if (!await action.AsT1.Invoke())
                    {
                        break;
                    }
                }
            } while (Actions.Count > 0);
        }

        public async Task RunAsync(Func<bool> isRunEnable)
        {
            if (isRunEnable())
            {
                await RunAsync();
            }
        }

        public async Task RunAsync(Func<Task<bool>> isRunEnable)
        {
            if (await isRunEnable())
            {
                await RunAsync();
            }
        }
    }
}
