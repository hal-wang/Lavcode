using System;
using System.Threading;
using System.Threading.Tasks;

/**
 *                    .::::.
 *                  .::::::::.
 *                 :::::::::::
 *             ..:::::::::::'
 *           '::::::::::::'
 *             .::::::::::
 *        '::::::::::::::..
 *             ..::::::::::::.
 *           ``::::::::::::::::                                              
 *            ::::``:::::::::'        .:::.
 *           ::::'   ':::::'       .::::::::.
 *         .::::'      ::::     .:::::::'::::.
 *        .:::'       :::::  .:::::::::' ':::::.
 *       .::'        :::::.:::::::::'      ':::::.
 *      .::'         ::::::::::::::'         ``::::.
 *  ...:::           ::::::::::::'              ``::.
 * ```` ':.          ':::::::::'                  ::::..
 *                    '.:::::'                    ':'````..
 */

namespace Hubery.Lavcode.Uwp
{
    public static class TaskExtend
    {
        /// <summary>
        /// 限时的Task，有返回值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="task"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
        {
            using var timeoutCancellationTokenSource = new CancellationTokenSource();
            var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
            if (completedTask == task)
            {
                timeoutCancellationTokenSource.Cancel();
                var result = await task;
                return result;
            }
            else
            {
                throw new TimeoutException("操作超时");
            }
        }

        /// <summary>
        /// 限时的Task，无返回值
        /// </summary>
        /// <param name="task"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static async Task TimeoutAfter(this Task task, TimeSpan timeout)
        {
            using var timeoutCancellationTokenSource = new CancellationTokenSource();
            var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
            if (completedTask == task)
            {
                timeoutCancellationTokenSource.Cancel();
                await task;
            }
            else
            {
                throw new TimeoutException("操作超时");
            }
        }

        public static async Task Run(Action action)
        {
            Exception exception = null;
            await Task.Run(() =>
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            });
            if (exception != null)
            {
                throw exception;
            }
        }

        /// <summary>
        /// 用于界面提示，比如 Loading
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static Task SleepAsync(int interval = 100)
        {
            var task = new Task(() =>
            {
                Thread.Sleep(interval);
            });
            task.Start();
            return task;
        }
    }
}