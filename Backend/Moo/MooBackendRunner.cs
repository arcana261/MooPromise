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
                MooThreadPoolTask task = null;
                bool clearBusy = false;

                while (!_shutdown && task == null)
                {
                    if (clearBusy)
                    {
                        _busy = false;
                        clearBusy = false;
                    }

                    if (!_context.Queue.TryPop(out task))
                    {
                        if (_busy)
                        {
                            clearBusy = true;
                            _context.TaskAddedSignal.WaitOne(5000);
                        }
                        else
                        {
                            _context.TaskAddedSignal.WaitOne(1000);
                        }
                    }
                    else
                    {
                        break;
                    }
                }

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
