using Mono.Cecil;
using Mono.Cecil.Cil;
using SOLPI.Instrumentations.Prototypes;
using SOLPolymorph.SignsOfLife.Polymorph.Hooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SOLPI.Instrumentations
{
    class ItemHandlerGetNewItemOverride : InstrumentationTaskBase
    {

        public ItemHandlerGetNewItemOverride()
        {

        }

        private MethodReference _referencedToStart;

        public override void Process(Instrumentor instrumentor, AssemblyDefinition assdef)
        {
            var referenceStart = GetPolymorphReferenceStart();
            _referencedToStart = SetImports(assdef, referenceStart);
            foreach (TypeDefinition typedef in assdef.MainModule.Types)
            {
                if (typedef.Name != "<Module>")
                {
                    if (typedef.Name == TargetType)
                    {
                        foreach (MethodDefinition methoddef in typedef.Methods)
                        {
                            if (methoddef.Name == TargetMethod && methoddef.Parameters[0].ParameterType.Name == "InventoryItemType")
                            {
                                ModifyMethod(methoddef);
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }

        private void ModifyMethod(MethodDefinition methoddef)
        {
            var body = methoddef.Body;
            var worker = body.GetILProcessor();

            Instruction pickArgument = worker.Create(OpCodes.Ldarg_0);
            Instruction retr = worker.Create(OpCodes.Ret);

            Instruction call = worker.Create(OpCodes.Call, _referencedToStart);

            Instruction start = body.Instructions[0];
            body.GetILProcessor().InsertBefore(start, call);
            body.GetILProcessor().InsertBefore(call, pickArgument);
            body.GetILProcessor().InsertAfter(call, retr);

        }

        private MethodReference SetImports(AssemblyDefinition assdef, MethodInfo reference)
        {
            return assdef.MainModule.Import(reference);
        }

        public string TargetType
        {
            get { return "InventoryItemHandler"; }
        }

        public string TargetMethod
        {
            get { return "GetNewItemByItemType"; }
        }

        protected System.Reflection.MethodInfo GetPolymorphReferenceStart()
        {
            return typeof(ItemHandlerHooks).GetMethod("GetNewItemByItemType");
        }


    }
}
