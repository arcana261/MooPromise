using MooPromise.DataStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise
{
    public class ControlValue<T>
    {
        private ControlState _state;
        private NullableResult<T> _result;

        internal static ControlValue<T> Break
        {
            get
            {
                return new ControlValue<T>(ControlState.Break);
            }
        }

        internal static ControlValue<T> Continue
        {
            get
            {
                return new ControlValue<T>(ControlState.Continue);
            }
        }

        internal static ControlValue<T> Next
        {
            get
            {
                return new ControlValue<T>(ControlState.Next);
            }
        }

        internal static ControlValue<T> Return(T value)
        {
            return new ControlValue<T>(value);
        }

        internal static ControlValue<T> Return()
        {
            return new ControlValue<T>();
        }

        private ControlValue(ControlState state, NullableResult<T> result)
        {
            this._state = state;
            this._result = result;
        }

        internal ControlValue(ControlState state)
            : this(state, new NullableResult<T>())
        {

        }

        internal ControlValue(T result)
            : this(ControlState.Return, new NullableResult<T>(result))
        {

        }

        internal ControlValue()
            : this(ControlState.Return)
        {

        }

        public ControlState State
        {
            get
            {
                return _state;
            }
        }

        public bool HasValue
        {
            get
            {
                return _result.HasResult;
            }
        }

        public T Value
        {
            get
            {
                if (!HasValue)
                {
                    throw new NullReferenceException();
                }

                return _result.Result;
            }
        }
    }
}
