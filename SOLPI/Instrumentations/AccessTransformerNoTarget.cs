using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLPI.Instrumentations
{
    public class AccessTransformerNoTarget : InstrumentationTaskBase
    {

        public AccessTransformerNoTarget()
        {

        }

        public override void Process(Instrumentor instrumentor, AssemblyDefinition assdef)
        {
            var module = assdef.MainModule;
            foreach (var type in module.Types)
            {
                type.IsPublic = true;
            }
        }
    }
}
