using Mono.Cecil.Cil;
using SOLPI.Instrumentations.Prototypes;
using SOLPolymorph.SignsOfLife.Polymorph.Hooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLPI.Instrumentations
{
    class StaticPrefabGetNewStaticPrefabByStaticPrefabTypeOverride : InstrumentationTaskHookTo
    {
        public override string TargetType
        {
            get { return "StaticPrefab"; }
        }

        public override string TargetMethod
        {
            get { return "GetNewStaticPrefabByStaticPrefabType"; }
        }

        protected override System.Reflection.MethodInfo GetPolymorphReferenceStart()
        {
            return typeof(StaticPrefabsHandlerHooks).GetMethod("GetNewStaticPrefabByStaticPrefabType"); ;
        }

        protected override System.Reflection.MethodInfo GetPolymorphReferenceEnd()
        {
            throw new NotImplementedException();
        }

        protected override bool RequiresEnd
        {
            get { return false; }
        }

        //protected override void InjectAdditionalInstructionBeforeStartCall(Mono.Cecil.Cil.MethodBody body, Mono.Cecil.Cil.Instruction call)
        //{
        //    Instruction pickMyself = body.GetILProcessor().Create(OpCodes.Ldarg_0);
        //    body.GetILProcessor().InsertBefore(call, pickMyself);
        //}

        protected override void InjectAdditionalInstructionAfterStartCall(Mono.Cecil.Cil.MethodBody body, Mono.Cecil.Cil.Instruction call)
        {
            var ret = body.GetILProcessor().Create(OpCodes.Ret);
            body.GetILProcessor().InsertAfter(call, ret);
        }
    }
}
