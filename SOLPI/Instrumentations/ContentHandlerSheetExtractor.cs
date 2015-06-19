using Mono.Cecil;
using Mono.Cecil.Cil;
using SOLPolymorph.SignsOfLife.Polymorph.Hooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SOLPI.Instrumentations
{
    class ContentHandlerSheetExtractor : InstrumentationTaskBase
    {

        public ContentHandlerSheetExtractor()
        {
            
        }

        public static readonly string TargetType = "ContentHandler";
        public static readonly string TargetMethod = "Initialize";

        public override void Process(Instrumentor instrumentor, AssemblyDefinition assdef)
        {
            GetPolymorphReference();
            SetImports(assdef);
            foreach (TypeDefinition typedef in assdef.MainModule.Types)
            {
                if (typedef.Name != "<Module>")
                {
                    if (typedef.Name == TargetType)
                    {
                        foreach (MethodDefinition methoddef in typedef.Methods)
                        {
                            if (methoddef.Name == TargetMethod)
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

        private void SetImports(AssemblyDefinition assdef)
        {
            _referencedTo = assdef.MainModule.Import(_delegateTo);
            _referencedTo2 = assdef.MainModule.Import(_delegateTo2);
        }

        private MethodInfo _delegateTo, _delegateTo2;
        private MethodReference _referencedTo, _referencedTo2;

        private void GetPolymorphReference()
        {
            _delegateTo = typeof(ContentHandlerHooks).GetMethod("InitializeOnStart");
            _delegateTo2 = typeof(ContentHandlerHooks).GetMethod("InitializeOnEnd");
        }

        private void ModifyMethod(MethodDefinition methoddef)
        {
            var body = methoddef.Body;
            var worker = body.GetILProcessor();

            Instruction pickContentManager = worker.Create(OpCodes.Ldarg_1);

            Instruction call = worker.Create(OpCodes.Call,_referencedTo);

            Instruction start = body.Instructions[0];
            body.GetILProcessor().InsertBefore(start, call);
            body.GetILProcessor().InsertBefore(call, pickContentManager);


            Instruction call2 = worker.Create(OpCodes.Call,_referencedTo2);
            Instruction end = body.Instructions.Last();
            body.GetILProcessor().InsertBefore(end, call2);
        }

    }
}
