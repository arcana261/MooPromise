using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.ThreadPool
{
    internal interface IThreadPoolResult
    {
        AsyncState State { get; }
        void Start();
        bool Cancel();
        void OnCompleted(Action action);
        void OnFailed(Action<Exception> error);
        Exception Error { get; }

#if DEBUG
        bool IsManual { get; }
#endif
    }
}
