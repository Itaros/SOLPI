using SOLPI.Instrumentations.Prototypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLPI.Instrumentations
{
    public class RedefineGameLocalStorage : InstrumentationTaskRedefineLoadstring
    {

        public RedefineGameLocalStorage() : base("IOHandler", InstrumentationTaskRedefineLoadstring.MethodNameStaticConstructor, "/Signs of Life", "/Signs of Life - SOLP") { 
            
        }


    }
}
