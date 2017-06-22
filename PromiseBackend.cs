using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise
{
    public enum PromiseBackend
    {
        Default, DotNet, MooThreadPool, Custom
    }
}
