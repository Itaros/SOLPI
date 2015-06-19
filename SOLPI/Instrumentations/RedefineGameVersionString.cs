using SOLPI.Instrumentations.Prototypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLPI.Instrumentations
{
    public class RedefineGameVersionString : InstrumentationTaskRedefineLoadstring
    {

        public RedefineGameVersionString()
            : base("SpaceGame", "get_VersionString", "*", " (SOLP Modded)")
        {

        }

        protected override bool Compare(string operand, string target)
        {
            return true;//First occurence
        }

        protected override string GetNewValue(string oldValue)
        {
            return oldValue+_newValue;
        }

    }
}
