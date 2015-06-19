using Mono.Cecil;
using Mono.Cecil.Cil;
using SOLPI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLPI.Instrumentations
{
    public class ExtractAllVanillaProjects : InstrumentationTaskBase
    {

        private SortedList<int, string> _enumIndex = new SortedList<int, string>();

        public override void Process(Instrumentor instrumentor, Mono.Cecil.AssemblyDefinition assdef)
        {
            //Lets find some stuff
            var module = assdef.MainModule;
            //Stage 1: Acquiring vanilla enum index
            foreach (var typedef in module.Types)
            {
                if (typedef.Name == "StaticPrefabType")
                {
                    foreach (var fieldef in typedef.Fields)
                    {
                        if (fieldef.Name == "value__") { continue; }
                        string val = fieldef.Name.Replace("_","").ToUpperInvariant();
                        int key = (int)fieldef.Constant;//No idea if this will work, derp ^_^
                        _enumIndex.Add(key, val);
                    }
                }
            }
            //Stage 2: Collecting index
            foreach (var typedef in module.Types)
            {
                if (typedef.Name == "StaticPrefab")
                {
                    var instantiator = FindEntryToInstantiator(typedef);
                    List<String> typelist = CollectAllCTORCalls(instantiator);
                    instrumentor.Workspace.SaveAllLines(typelist.ToArray(),"vanillaprojects");
                    return;//No need to look for others
                }
            }
            throw new InstrumentationFailureException("SP Registry type is unreachable!");
        }

        private List<string> CollectAllCTORCalls(MethodDefinition instantiator)
        {
            var instructions = instantiator.Body.Instructions;
            List<string> list = new List<string>();
            foreach (var instruction in instructions)
            {
                if (instruction.OpCode == OpCodes.Newobj)
                {
                    if (instruction.Operand != null)
                    {
                        MethodDefinition cctordef = instruction.Operand as MethodDefinition;
                        if (cctordef != null)
                        {
                            string collect = cctordef.DeclaringType.FullName;//Getting name
                            collect += ":";//Adding separator. Such a waste actually, but well, it is going to be cached

                            string compareAgainst = cctordef.DeclaringType.Name;
                            int id = GetIdFor(compareAgainst);
                            collect += id.ToString();

                            list.Add(collect);
                        }
                    }
                }
            }
            return list;
        }

        private int GetIdFor(string compareAgainst)
        {
            var idarray = _enumIndex.Where(o => "SP_"+o.Value == compareAgainst.ToUpperInvariant()).Select(o => o.Key).ToArray();
            if (idarray.Length != 1) { throw new InstrumentationFailureException("Failed to establish vanilla SP index for "+compareAgainst); }
            return idarray[0];
        }

        private MethodDefinition FindEntryToInstantiator(Mono.Cecil.TypeDefinition typedef)
        {
            foreach (var metdef in typedef.Methods)
            {
                if (metdef.Name == "GetNewStaticPrefabByStaticPrefabType")
                {
                    return metdef;//No need to go check further(no parameters overload expected)
                }
            }
            throw new InstrumentationFailureException("SP Registry Instantiator method is unavailable!");
        }
    }
}
