using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLPolymorph.SignsOfLife.Polymorph.Exceptions
{
    public class ModLoadingSequenceFailureException : Exception
    {
        public ModLoadingSequenceFailureException(string message) : base(message) { }
        public ModLoadingSequenceFailureException(string message, Exception innerException) : base(message, innerException) { }
    }
}
