using MooPromise.DataStructure;
using MooPromise.DataStructure.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MooPromise.Backend.Moo
{
    internal class MooBackendContext
    {
        public MooBackendContext()
        {
            Queue = new ConcurrentPriorityQueue<MooBackendTask>();
            TaskAddedSignal = new AutoResetEvent(false);
        }

        public IPriorityQueue<MooBackendTask> Queue
        {
            get;
            private set;
        }

        public AutoResetEvent TaskAddedSignal
        {
            get;
            private set;
        }
    }
}
