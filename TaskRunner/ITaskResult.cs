using MooPromise.DataStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.TaskRunner
{
    internal interface ITaskResult
    {
        ITaskResult Then(Action action);
        ITaskResult Then(Func<ITaskResult> action);
        ITaskResult Then(Action<NullableResult<object>> action);
        ITaskResult Then(Func<NullableResult<object>, ITaskResult> action);
        ITaskResult Then(Func<object> action);
        ITaskResult Then(Func<NullableResult<object>, object> action);
        ITaskResult Catch(Action<Exception> action);
        ITaskResult Catch(Action action);
        ITaskResult Finally(Action action);
        ITaskResult Finally(Action<Exception> action);
        ITaskResult Immediately { get; }
        ITaskResult WithPriority(int priority);
        AsyncState State { get; }
        void Start();
        bool Cancel();
        Exception Error { get; }
        object Result { get; }
        bool HasResult { get; }
    }
}
