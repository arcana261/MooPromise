using MooPromise.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.ThreadPool
{
    internal interface IThreadPool : IDisposable
    {
        IThreadPoolResult Begin(Action action);
        IThreadPoolResult Begin(Action action, int priority);
        IThreadPoolResult BeginImmediately(Action action);
        IThreadPoolResult Create(Action action);
        IThreadPoolResult Create(Action action, int priority);
        IThreadPoolResult CreateImmediately(Action action);
        IBackend Backend { get; }
    }
}
