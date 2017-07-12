using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.ThreadPool.Moo
{
    internal abstract class BaseThreadPoolResult : IThreadPoolResult
    {
        private AsyncState _state;
        private Exception _error;
        private IList<Action> _completedHandlers;
        private IList<Action<Exception>> _failedHandlers;

        public BaseThreadPoolResult()
        {
            _state = AsyncState.Stopped;
            _error = null;
            _completedHandlers = null;
            _failedHandlers = null;
            SyncRoot = new object();
        }

        public AsyncState State
        {
            get
            {
                lock (SyncRoot)
                {
                    return _state;
                }
            }

            private set
            {
                lock (SyncRoot)
                {
                    _state = value;
                }
            }
        }

        public Exception Error
        {
            get
            {
                lock (SyncRoot)
                {
                    if (State != AsyncState.Completed && State != AsyncState.Failed && State != AsyncState.Canceled)
                    {
                        throw new InvalidOperationException("AsyncThreadPoolResult is not completed/failed yet");
                    }

                    return _error;
                }
            }

            private set
            {
                lock (SyncRoot)
                {
                    _error = value;
                }
            }
        }

        protected abstract void DoStart();

        protected object SyncRoot
        {
            get;
            private set;
        }

        protected void SetCompleted()
        {
            lock (SyncRoot)
            {
                if (State == AsyncState.Running || State == AsyncState.Pending)
                {
                    State = AsyncState.Completed;

                    if (_completedHandlers != null)
                    {
                        try
                        {
                            foreach (var handler in _completedHandlers)
                            {
                                handler();
                            }
                        }
                        catch (Exception e)
                        {
                            if (!(e is ObjectDisposedException))
                            {
                                Environment.FailFast("Error while calling completed handlers", e);
                            }
                        }

                        _completedHandlers = null;
                    }
                }
            }
        }

        public void OnCompleted(Action action)
        {
            lock (SyncRoot)
            {
                if (State == AsyncState.Completed)
                {
                    try
                    {
                        action();
                    }
                    catch (Exception e)
                    {
                        if (!(e is ObjectDisposedException))
                        {
                            Environment.FailFast("Error while calling completed handlers", e);
                        }
                    }
                }
                else
                {
                    if (_completedHandlers == null)
                    {
                        _completedHandlers = new List<Action>();
                    }

                    _completedHandlers.Add(action);
                }
            }
        }

        protected void SetFailed(Exception error, AsyncState state)
        {
            lock (SyncRoot)
            {
                if (State == AsyncState.Running || State == AsyncState.Pending || (error is OperationCanceledException && State == AsyncState.Stopped))
                {
                    if (error == null)
                    {
                        throw new ArgumentException("error can not be null");
                    }

                    State = state;
                    Error = error;

                    if (_failedHandlers != null)
                    {
                        try
                        {
                            foreach (var handler in _failedHandlers)
                            {
                                handler(error);
                            }
                        }
                        catch (Exception e)
                        {
                            if (!(e is ObjectDisposedException))
                            {
                                Environment.FailFast("Error while calling failed handlers", e);
                            }
                        }

                        _failedHandlers = null;
                    }
                }
            }
        }

        protected void SetRunning()
        {
            lock (SyncRoot)
            {
                if (State == AsyncState.Pending)
                {
                    State = AsyncState.Running;
                }
            }
        }

        protected void SetFailed(Exception error)
        {
            SetFailed(error, AsyncState.Failed);
        }

        public void OnFailed(Action<Exception> action)
        {
            lock (SyncRoot)
            {
                if (State == AsyncState.Failed)
                {
                    try
                    {
                        action(Error);
                    }
                    catch (Exception e)
                    {
                        Environment.FailFast("Error while calling failed handlers", e);
                    }
                }
                else
                {
                    if (_failedHandlers == null)
                    {
                        _failedHandlers = new List<Action<Exception>>();
                    }

                    _failedHandlers.Add(action);
                }
            }
        }

        public void Start()
        {
            lock (SyncRoot)
            {
                if (State == AsyncState.Stopped)
                {
                    State = AsyncState.Pending;
                    DoStart();
                }
            }
        }

        public bool Cancel()
        {
            lock (SyncRoot)
            {
                if (State == AsyncState.Pending || State == AsyncState.Stopped)
                {
                    SetFailed(new OperationCanceledException(), AsyncState.Canceled);
                    return true;
                }
            }

            return false;
        }

#if DEBUG
        public virtual bool IsManual
        {
            get
            {
                return false;
            }
        }
#endif
    }
}
