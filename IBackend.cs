using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise
{
    public interface IBackend : IDisposable
    {
        void Add(Action action);
        void Add(Action action, int priority);
        void AddImmediately(Action action);
    }
}
