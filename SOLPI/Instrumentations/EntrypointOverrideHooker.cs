using Mono.Cecil.Cil;
using SOLPI.Instrumentations.Prototypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLPI.Instrumentations
{
    public class EntrypointOverrideHooker : InstrumentationTaskHookTo
    {

        public override string TargetType
        {
            get { return "Program"; }
        }

        public override string TargetMethod
        {
            get { return "Main"; }
        }

        protected override System.Reflection.MethodInfo GetPolymorphReferenceStart()
        {
            return typeof(SOLPolymorph.SignsOfLife.Polymorph.Hooks.SOLRuntimeHooks).GetMethod("OnEntryPoint");
        }

        protected override System.Reflection.MethodInfo GetPolymorphReferenceEnd()
        {
            throw new NotImplementedException();
        }

        protected override bool RequiresEnd
        {
            get { return false; ; }
        }
        protected override void InjectAdditionalInstructionAfterStartCall(Mono.Cecil.Cil.MethodBody body, Mono.Cecil.Cil.Instruction call)
        {
            Instruction retr = body.GetILProcessor().Create(OpCodes.Ret);
            body.GetILProcessor().InsertAfter(call, retr);
        }
    }
}
