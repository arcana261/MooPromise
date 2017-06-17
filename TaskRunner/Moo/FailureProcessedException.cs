using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MooPromise.TaskRunner.Moo
{

    [Serializable]
    public class FailureProcessedException : Exception
    {
        public FailureProcessedException() { }
        public FailureProcessedException(string message) : base(message) { }
        public FailureProcessedException(string message, Exception inner) : base(message, inner) { }
        protected FailureProcessedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
