using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MooPromise.Backend.Moo
{
    internal class MooBackendRunner : IDisposable
    {
        private MooBackendContext _context;
        private volatile bool _shutdown;
        private volatile bool _busy;
        private Thread _thread;
        private object _syncRoot;
        private bool _disposed;

        public MooBackendRunner(MooBackendContext context)
        {
            this._context = context;
            this._shutdown = false;
            this._syncRoot = new object();
            this._disposed = false;
            this._busy = false;
            this._thread = new Thread(new ThreadStart(ThreadFunction));
            this._thread.IsBackground = true;
        }

        ~MooBackendRunner()
        {
            Dispose(false);
        }

        public void Start()
        {
            lock (_syncRoot)
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException("ThreadPoolRunner");
                }

                _thread.Start();
            }
        }

        public bool IsInsideRunnerThread()
        {
            lock (_syncRoot)
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException("ThreadPoolRunner");
                }

                return Thread.CurrentThread.ManagedThreadId == _thread.ManagedThreadId;
            }
        }

#if DEBUG
        public int ManagedThreadId
        {
            get
            {
                lock (_syncRoot)
                {
                    if (_disposed)
                    {
                        throw new ObjectDisposedException("ThreadPoolRunner");
                    }

                    return _thread.ManagedThreadId;
                }
            }
        }
#endif

        public bool IsBusy
        {
            get
            {
                lock (_syncRoot)
                {
                    if (_disposed)
                    {
                        throw new ObjectDisposedException("ThreadPoolRunner");
                    }

                    return _busy;
                }
            }
        }

        private void ThreadFunction()
        {
            while (!_shutdown)
            {
                MooBackendTask task = null;
                int taskPriority = 0;
                bool clearBusy = false;

                IList<Tuple<MooBackendFutureTask, int>> notDueList = new List<Tuple<MooBackendFutureTask, int>>();

                while (!_shutdown && task == null)
                {
                    if (clearBusy)
                    {
                        _busy = false;
                        clearBusy = false;
                    }

                    if (!_context.Queue.TryPop(out task, out taskPriority))
                    {
                        int minDue = int.MaxValue;

                        foreach (var notDue in notDueList)
                        {
                            _context.Queue.Add(notDue.Item1, notDue.Item2);
                            minDue = Math.Min(minDue, notDue.Item1.DueTickCount);
                        }
                        notDueList = new List<Tuple<MooBackendFutureTask, int>>();

                        int sleepToNextDue = Math.Max(0, minDue - Environment.TickCount);

                        if (_busy)
                        {
                            clearBusy = true;
                            _context.TaskAddedSignal.WaitOne(Math.Min(sleepToNextDue, 5000));
                        }
                        else
                        {
                            _context.TaskAddedSignal.WaitOne(Math.Min(sleepToNextDue, 1000));
                        }
                    }
                    else if ((task is MooBackendFutureTask))
                    {
                        var futureTask = task as MooBackendFutureTask;

                        if (Environment.TickCount < futureTask.DueTickCount)
                        {
                            notDueList.Add(Tuple.Create(futureTask, taskPriority));
                            task = null;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if (!_shutdown)
                {
                    foreach (var notDue in notDueList)
                    {
                        _context.Queue.Add(notDue.Item1, notDue.Item2);
                    }
                }
                notDueList = new List<Tuple<MooBackendFutureTask, int>>();

                if (task != null)
                {
                    _busy = true;
                    clearBusy = false;

                    try
                    {
                        task.Action();
                    }
                    catch (Exception error)
                    {
                        Environment.FailFast("Unhandled exception while executing moo backend task", error);
                    }
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void WaitUntilDisposed()
        {
            _thread.Join();
        }

        public bool WaitUntilDisposed(int waitMs)
        {
            return _thread.Join(waitMs);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                bool join = false;

                lock (_syncRoot)
                {
                    if (!_disposed)
                    {
                        bool isInside = IsInsideRunnerThread();
                        _disposed = true;
                        _shutdown = true;

                        if (!isInside)
                        {
                            join = true;
                        }
                    }
                }

                if (join)
                {
                    //_thread.Join();
                }
            }
            else
            {
                _disposed = true;
                _shutdown = true;
            }
        }
    }
}
