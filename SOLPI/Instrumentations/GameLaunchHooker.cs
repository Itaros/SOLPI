using SOLPI.Instrumentations.Prototypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SOLPI.Instrumentations
{
    public class GameLaunchHooker : InstrumentationTaskHookTo
    {
        public GameLaunchHooker()
        {

        }


        public override string TargetType
        {
            get { return "SpaceGame"; }
        }

        public override string TargetMethod
        {
            get { return "Initialize"; }
        }

        protected override MethodInfo GetPolymorphReferenceStart()
        {
            return typeof(SOLPolymorph.SignsOfLife.Polymorph.Hooks.SOLRuntimeHooks).GetMethod("OnSpaceGameLaunch");
        }

        protected override bool RequiresEnd
        {
            get { return true; }
        }

        protected override MethodInfo GetPolymorphReferenceEnd()
        {
            return typeof(SOLPolymorph.SignsOfLife.Polymorph.Hooks.SOLRuntimeHooks).GetMethod("OnSpaceGameLaunchPost");
        }
    }
}
