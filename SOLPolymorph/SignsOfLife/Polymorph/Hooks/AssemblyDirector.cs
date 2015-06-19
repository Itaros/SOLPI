using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLPolymorph.SignsOfLife.Polymorph.Hooks
{
    public sealed class AssemblyDirector
    {

        private static AssemblyDirector _instance=null;
        public static AssemblyDirector Instance { get { if (_instance == null) { _instance = new AssemblyDirector(); } return _instance; } }

        public AssemblyDirector()
        {

        }

        public Type HostType { get; private set; }

        /// <summary>
        /// Used by SOL Runtime Hook to establish master Type
        /// </summary>
        /// <param name="hostType"></param>
        internal void DefineHostType(Type hostType)
        {
            HostType = hostType;
        }

    }
}
