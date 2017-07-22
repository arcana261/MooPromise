using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MooPromise.Backend
{
    internal class SynchronizationContextBackend : IBackend
    {
        private SynchronizationContext _context;

        public SynchronizationContextBackend(SynchronizationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this._context = context;
        }

        public void Add(Action action)
        {
            _context.Post(new SendOrPostCallback(state =>
            {
                try
                {
                    action();
                }
                catch (Exception error)
                {
                    Environment.FailFast("could not execute backend task", error);
                }
            }), null);
        }

        public void Add(Action action, int priority)
        {
            Add(action);
        }

        public void AddImmediately(Action action)
        {
            if (Object.Equals(SynchronizationContext.Current, _context))
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
            return Object.Equals(SynchronizationContext.Current, _context);
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

        public void AddFuture(int dueTickTime, Action action)
        {
            int tick = dueTickTime - Environment.TickCount;

            if (tick <= 0)
            {
                Add(action);
            }
            else
            {
                System.Timers.Timer timer = new System.Timers.Timer(tick);
                timer.AutoReset = false;
                timer.Elapsed += (sender, args) =>
                {
                    AddImmediately(action);
                };

                timer.Start();
            }
        }

        public void AddFuture(int dueTickTime, Action action, int priority)
        {
            AddFuture(dueTickTime, action);
        }
    }
}
