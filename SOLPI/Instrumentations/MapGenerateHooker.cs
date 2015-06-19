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
    public class MapGenerateHooker : InstrumentationTaskBase
    {

        public static readonly string TargetType = "Map";
        public static readonly string TargetMethod = "Generate";

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

        private void ModifyMethod(MethodDefinition methoddef)
        {
            var body = methoddef.Body;
            var worker = body.GetILProcessor();

            Instruction pickContentManager = worker.Create(OpCodes.Ldarg_0);

            Instruction call = worker.Create(OpCodes.Call, _referencedTo);

            Instruction start = body.Instructions[0];
            body.GetILProcessor().InsertBefore(start, call);
            body.GetILProcessor().InsertBefore(call, pickContentManager);

            //Instruction force = worker.Create(OpCodes.Ret);
            //body.GetILProcessor().InsertAfter(call, force);
        }

        private void SetImports(AssemblyDefinition assdef)
        {
            _referencedTo = assdef.MainModule.Import(_delegateTo);
        }

        private MethodInfo _delegateTo;
        private MethodReference _referencedTo;

        private void GetPolymorphReference()
        {
            _delegateTo = typeof(MapHooks).GetMethod("GenerateOnStart");
        }


    }
}
