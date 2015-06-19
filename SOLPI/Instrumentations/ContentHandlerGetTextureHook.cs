using SOLPI.Instrumentations.Prototypes;
using SOLPolymorph.SignsOfLife.Polymorph.Hooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLPI.Instrumentations
{
    class ContentHandlerGetTextureHook : InstrumentationTaskHookTo
    {

        public ContentHandlerGetTextureHook()
        {

        }


        //protected override void InjectAdditionalInstructionBeforeStartCall(Mono.Cecil.Cil.MethodBody body, Mono.Cecil.Cil.Instruction call)
        //{
        //    base.InjectAdditionalInstructionBeforeStartCall(body, call);
        //}

        public override string TargetType
        {
            get { return "ContentHandler"; }
        }

        public override string TargetMethod
        {
            get { return "GetTexture"; }
        }

        protected override System.Reflection.MethodInfo GetPolymorphReferenceStart()
        {
            return typeof(ContentHandlerHooks).GetMethod("GetTextureOnStart");
        }

        protected override System.Reflection.MethodInfo GetPolymorphReferenceEnd()
        {
            return typeof(ContentHandlerHooks).GetMethod("GetTextureOnEnd");
        }

        protected override bool RequiresEnd
        {
            get { return true; }
        }
    }
}
