using MooPromise.DataStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.TaskRunner.Moo
{
    internal class BoundTaskResult : ITaskResult
    {
        private ITaskResult _owner;
        private ITaskResult _current;

        public BoundTaskResult(ITaskResult owner, ITaskResult current)
        {
            this._owner = owner;
            this._current = current;
        }

        public ITaskResult Immediately
        {
            get
            {
                return new BoundTaskResult(_owner, _current.Immediately);
            }
        }

        public AsyncState State
        {
            get
            {
                return _owner.State;
            }
        }

        public Exception Error
        {
            get
            {
                return _current.Error;
            }
        }

        public object Result
        {
            get
            {
                return _current.Result;
            }
        }

        public bool HasResult
        {
            get
            {
                return _current.HasResult;
            }
        }

        public ITaskResult Then(Action action)
        {
            return _current.Then(action);
        }

        public ITaskResult Catch(Action<Exception> action)
        {
            return _current.Catch(action);
        }

        public ITaskResult Finally(Action action)
        {
            return _current.Finally(action);
        }

        public ITaskResult WithPriority(int priority)
        {
            return new BoundTaskResult(_owner, _current.WithPriority(priority));
        }

        public void Start()
        {
            _owner.Start();
        }

        public ITaskResult Then(Func<ITaskResult> action)
        {
            return _current.Then(action);
        }

        public bool Cancel()
        {
            return _owner.Cancel();
        }

        public ITaskResult Then(Action<NullableResult<object>> action)
        {
            return _current.Then(action);
        }

        public ITaskResult Then(Func<NullableResult<object>, ITaskResult> action)
        {
            return _current.Then(action);
        }

        public ITaskResult Catch(Action action)
        {
            return _current.Then(action);
        }

        public ITaskResult Then(Func<object> action)
        {
            return _current.Then(action);
        }

        public ITaskResult Then(Func<NullableResult<object>, object> action)
        {
            return _current.Then(action);
        }

        public ITaskResult Finally(Action<Exception> action)
        {
            return _current.Finally(action);
        }
    }
}
