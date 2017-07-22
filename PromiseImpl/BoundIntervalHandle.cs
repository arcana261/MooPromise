using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.PromiseImpl
{
    internal class BoundIntervalHandle : BaseIntervalImpl
    {
        private IntervalHandleBase _owner;

        public BoundIntervalHandle(IntervalHandleBase owner)
        {
            this._owner = owner;
        }

        public override PromiseFactory Factory
        {
            get
            {
                return _owner.Factory;
            }
        }

        public override bool IsCanceled
        {
            get
            {
                return _owner.IsCanceled;
            }
        }

        public override PromisePriority Priority
        {
            get
            {
                return _owner.Priority;
            }
        }

        public override int Timeout
        {
            get
            {
                return _owner.Timeout;
            }
        }

        public override void Cancel()
        {
            _owner.Cancel();
        }
    }

    internal class BoundIntervalHandle<T> : BaseIntervalImpl<T>
    {
        private IntervalHandleBase _owner;

        public BoundIntervalHandle(IntervalHandleBase owner)
        {
            this._owner = owner;
        }

        public override PromiseFactory Factory
        {
            get
            {
                return _owner.Factory;
            }
        }

        public override bool IsCanceled
        {
            get
            {
                return _owner.IsCanceled;
            }
        }

        public override PromisePriority Priority
        {
            get
            {
                return _owner.Priority;
            }
        }

        public override int Timeout
        {
            get
            {
                return _owner.Timeout;
            }
        }

        public override void Cancel()
        {
            _owner.Cancel();
        }
    }
}
