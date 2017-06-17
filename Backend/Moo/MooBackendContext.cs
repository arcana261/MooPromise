using MooPromise.DataStructure;
using MooPromise.DataStructure.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Backend.Moo
{
    internal class MooBackendContext
    {
        public MooBackendContext()
        {
            Queue = new ConcurrentPriorityQueue<MooThreadPoolTask>();
        }

        public IPriorityQueue<MooThreadPoolTask> Queue
        {
            get;
            private set;
        }
    }
}
