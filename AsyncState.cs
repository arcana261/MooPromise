using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise
{
    public enum AsyncState
    {
        Stopped, Pending, Running, Completed, Failed, Canceled
    }
}
