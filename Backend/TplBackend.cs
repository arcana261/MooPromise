using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
