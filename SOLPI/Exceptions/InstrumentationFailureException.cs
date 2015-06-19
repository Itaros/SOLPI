using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLPI.Exceptions
{
    public class InstrumentationFailureException : Exception
    {

        public InstrumentationFailureException() : base() { }
        public InstrumentationFailureException(string message) : base(message) { }
        public InstrumentationFailureException(string message, Exception innerException) : base(message, innerException) { }

    }
}
