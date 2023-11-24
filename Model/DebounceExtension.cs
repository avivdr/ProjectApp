using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectApp.Model
{
    public static class DebounceExtension
    {
        //public static Action Debounce(this Action func, int milliseconds = 300000)
        //{
        //    CancellationTokenSource? cancelTokenSource = null;

        //    return () =>
        //    {
        //        MainThread.BeginInvokeOnMainThread(() =>
        //        {
        //            cancelTokenSource?.Cancel();
        //            cancelTokenSource = new CancellationTokenSource();

        //            Task.Delay(milliseconds, cancelTokenSource.Token)
        //                .ContinueWith(t =>
        //                {
        //                    if (t.IsCompletedSuccessfully)
        //                    {
        //                        func();
        //                    }
        //                }, TaskScheduler.Default);
        //        });                
        //    };
        //}

        public static Action Debounce(this Action func, int milliseconds = 1000)
        {
            var last = 0;
            return () =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    var current = Interlocked.Increment(ref last);
                    Task.Delay(milliseconds).ContinueWith(task =>
                    {
                        if (current == last) func();
                        task.Dispose();
                    });
                });
            };
        }
    }
}
