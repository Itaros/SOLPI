using SOLPI.Instrumentations.Prototypes;
using SOLPolymorph.SignsOfLife.Polymorph.Hooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLPI.Instrumentations
{
    public class GameOnResloadHooker : InstrumentationTaskHookTo
    {

        public GameOnResloadHooker()
            : base()
        {

        }

        public override string TargetType
        {
            get { return "SpaceGame"; }
        }

        public override string TargetMethod
        {
            get { return "LoadContent"; }
        }

        protected override System.Reflection.MethodInfo GetPolymorphReferenceStart()
        {
            return typeof(SOLRuntimeHooks).GetMethod("OnSpaceGameLoadContent");
        }

        protected override System.Reflection.MethodInfo GetPolymorphReferenceEnd()
        {
            return typeof(SOLRuntimeHooks).GetMethod("OnSpaceGameLoadContentPost");
        }

        protected override bool RequiresEnd
        {
            get { return true; }
        }
    }
}
