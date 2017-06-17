using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.TaskRunner
{
    internal class NullableResult<T>
    {
        private T _result;
        private bool _hasResult;

        public NullableResult()
        {
            _hasResult = false;
        }

        public NullableResult(T result)
        {
            _result = result;
            _hasResult = true;
        }

        public T Result
        {
            get
            {
                lock (this)
                {
                    if (!_hasResult)
                    {
                        throw new InvalidProgramException("there is no result associated");
                    }

                    return _result;
                }
            }
        }

        public bool HasResult
        {
            get
            {
                lock (this)
                {
                    return _hasResult;
                }
            }
        }
    }
}
