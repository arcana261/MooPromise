using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.Backend.Moo
{
    internal class MooThreadPoolTask
    {
        public MooThreadPoolTask(Action action)
        {
            this.Action = action;
        }

        public Action Action
        {
            get;
            private set;
        }
    }
}
