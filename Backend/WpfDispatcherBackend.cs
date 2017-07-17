using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace MooPromise.Backend
{
    internal class WpfDispatcherBackend : IBackend
    {
        private Dispatcher _dispatcher;

        public WpfDispatcherBackend(Dispatcher dispatcher)
        {
            this._dispatcher = dispatcher;
        }

        public void Add(Action action)
        {
            _dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    action();
                }
                catch (Exception error)
                {
                    Environment.FailFast("could not execute backend task", error);
                }
            }));
        }

        public void Add(Action action, int priority)
        {
            Add(action);
        }

        public void AddImmediately(Action action)
        {
            if (_dispatcher.Thread.ManagedThreadId == Thread.CurrentThread.ManagedThreadId)
            {
                try
                {
                    action();
                }
                catch (Exception error)
                {
                    Environment.FailFast("could not execute backend task", error);
                }
            }
            else
            {
                Add(action);
            }
        }

        public bool IsCurrentThreadManagedByBackend()
        {
            return Object.Equals(Dispatcher.CurrentDispatcher, _dispatcher);
        }

#if DEBUG
        public IEnumerable<int> ManagedThreadIds
        {
            get
            {
                return new int[] { };
            }
        }
#endif

        public void Dispose()
        {
            
        }

        public void WaitUntilDisposed()
        {
            
        }

        public bool WaitUntilDisposed(int waitMs)
        {
            return true;
        }
    }
}
