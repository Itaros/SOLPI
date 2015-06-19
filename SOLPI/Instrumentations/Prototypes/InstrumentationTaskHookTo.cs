using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SOLPI.Instrumentations.Prototypes
{
    public abstract class InstrumentationTaskHookTo : InstrumentationTaskBase
    {

        public abstract string TargetType { get; }
        public abstract string TargetMethod { get; }

        private TypeReference _baseType;

        public override void Process(Instrumentor instrumentor, AssemblyDefinition assdef)
        {
            var referenceStart = GetPolymorphReferenceStart();
            _referencedToStart = SetImports(assdef, referenceStart);
            if (RequiresEnd)
            {
                var referenceEnd = GetPolymorphReferenceEnd();
                _referencedToEnd = SetImports(assdef, referenceEnd);
            }
            foreach (TypeDefinition typedef in assdef.MainModule.Types)
            {
                if (typedef.Name != "<Module>")
                {
                    if (typedef.Name == TargetType)
                    {
                        _baseType = typedef.BaseType;
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
 

            Instruction call = worker.Create(OpCodes.Call, _referencedToStart);
            
            Instruction start = body.Instructions[0];
            Instruction end = FindLastBase(body);
            body.GetILProcessor().InsertBefore(start, call);
            InjectAdditionalInstructionBeforeStartCall(body,call);
            InjectAdditionalInstructionAfterStartCall(body, call);

            if (RequiresEnd)
            {
                Instruction callEnd = worker.Create(OpCodes.Call, _referencedToEnd);
                body.GetILProcessor().InsertBefore(end, callEnd);
            }
        }

        protected virtual void InjectAdditionalInstructionBeforeStartCall(Mono.Cecil.Cil.MethodBody body, Instruction call)
        {
            Instruction pickMyself = body.GetILProcessor().Create(OpCodes.Ldarg_0);
            body.GetILProcessor().InsertBefore(call, pickMyself);
        }

        protected virtual void InjectAdditionalInstructionAfterStartCall(Mono.Cecil.Cil.MethodBody body, Instruction call)
        {

        }

        private Instruction FindLastBase(Mono.Cecil.Cil.MethodBody body)
        {
            foreach (var instruction in body.Instructions)
            {
                if (instruction.OpCode == OpCodes.Call)
                {
                    MethodReference operand = instruction.Operand as MethodReference;
                    if (operand == null) { continue; }
                    if (operand.FullName.Contains(_baseType.Name+"::"+TargetMethod))
                    {
                        return instruction;
                    }
                    //if (operand == null)
                    //{
                    //    Console.WriteLine("wow");
                    //    //return instruction;
                    //}
                }
            }
            return body.Instructions.Last();
        }

        private MethodReference _referencedToStart;
        private MethodReference _referencedToEnd;

        private MethodReference SetImports(AssemblyDefinition assdef, MethodInfo reference)
        {
            return assdef.MainModule.Import(reference);
        }

        /// <summary>
        /// Method Info of target in SOLPolymorph.SignsOfLife.Polymorph.Hooks
        /// Usually: typeof(GroupNameHooks).GetMethod("PolymorphMethodName");
        /// </summary>
        /// <returns></returns>
        protected abstract MethodInfo GetPolymorphReferenceStart();
        protected abstract MethodInfo GetPolymorphReferenceEnd();

        protected abstract bool RequiresEnd { get; }

    }
}
