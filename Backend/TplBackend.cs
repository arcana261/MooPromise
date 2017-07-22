using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MooPromise.Backend
{
    internal class TplBackend : IBackend
    {
        public void Add(Action action)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    Environment.FailFast("Unhandled exception while executing task in backend", e);
                }
            });
        }

        public void Add(Action action, int priority)
        {
            Add(action);
        }

        public void AddImmediately(Action action)
        {
            Add(action);
        }

        public bool IsCurrentThreadManagedByBackend()
        {
            return false;
        }

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
                Timer timer = new Timer(tick);
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

#if DEBUG
        public IEnumerable<int> ManagedThreadIds
        {
            get
            {
                return new int[] { };
            }
        }
#endif
    }
}
