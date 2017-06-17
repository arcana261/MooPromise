using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise
{
    public enum PromisePriority
    {
        Low = -2,
        BelowNormal = -1,
        Normal = 0,
        AboveNormal = 1,
        High = 2,
        AboveHigh = 3,
        Immediate = int.MaxValue
    }
}
