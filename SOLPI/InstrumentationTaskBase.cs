using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLPI
{
    public abstract class InstrumentationTaskBase
    {

        public abstract void Process(Instrumentor instrumentor, AssemblyDefinition assdef);

    }
}
