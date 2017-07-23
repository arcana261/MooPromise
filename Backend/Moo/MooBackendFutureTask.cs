using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Backend.Moo
{
    internal class MooBackendFutureTask : MooBackendTask
    {
        public MooBackendFutureTask(int dueTickCount, Action action) : base(action)
        {
            this.DueTickCount = dueTickCount;
        }

        public int DueTickCount
        {
            get;
            private set;
        }
    }
}
