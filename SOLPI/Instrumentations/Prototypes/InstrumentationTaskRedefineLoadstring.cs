using Mono.Cecil;
using Mono.Cecil.Cil;
using SOLPI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLPI.Instrumentations.Prototypes
{
    public abstract class InstrumentationTaskRedefineLoadstring : InstrumentationTaskBase
    {

        public const string MethodNameConstructor = ".ctor";
        public const string MethodNameStaticConstructor = ".cctor";

        private String _typeName;
        private String _methodName;
        protected String _oldValue;
        protected String _newValue;

        public InstrumentationTaskRedefineLoadstring(string typeName, string methodName, string oldValue, string newValue)
        {
            _typeName = typeName;
            _methodName = methodName;

            _oldValue = oldValue;
            _newValue = newValue;
        }

        protected virtual bool Compare(string operand, string target)
        {
            return operand == target;
        }

        public override void Process(Instrumentor instrumentor, AssemblyDefinition assdef)
        {
            foreach (var typedef in assdef.MainModule.Types)
            {
                if (typedef.Name == _typeName)
                {
                    foreach (var metdef in typedef.Methods)
                    {
                        if (metdef.Name == _methodName)
                        {
                            foreach (Instruction ins in metdef.Body.Instructions)
                            {
                                if (ins.OpCode == OpCodes.Ldstr)
                                {
                                    String operand = ins.Operand as String;
                                    if (operand != null)
                                    {
                                        if (Compare(operand,_oldValue))
                                        {
                                            ins.Operand = GetNewValue(operand);
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            throw new InstrumentationFailureException("It was impossible to find replacement point for Loadstring redefinition!");
        }

        protected virtual string GetNewValue(string oldValue)
        {
            return _newValue;
        }


    }
}
