using Mono.Cecil;
using Mono.Cecil.Cil;
using SOLPIBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLPI.Instrumentations
{
    public class ExtractAllVanillaItems : InstrumentationTaskBase
    {

        private static readonly string BaseMethodName = "InventoryItem";

        public override void Process(Instrumentor instrumentor, AssemblyDefinition assdef)
        {
            var module = assdef.MainModule;
            TypeDefinition inventoryItemType=null;
            inventoryItemType = PickBaseMethod(module);
            if (inventoryItemType == null)
            {
                throw new BadImageFormatException("Unable to obtain: "+BaseMethodName);
            }
            else
            {
                var list = CollectDerrived(inventoryItemType, module.Types.ToArray());
                int before = list.Count;
                list = SelectWithArgumentlessCTOR(list);
                var idlist = TryExtractIdsFromCTOR(list);
                int after = list.Count;
                int abstr = before - after;
                Console.WriteLine("Items identified: "+before+"["+"Concrete: "+after+", Abstract: "+abstr+"]");
                DumpAsCache(list,idlist,instrumentor.Workspace);
            }
        }

        private List<int> TryExtractIdsFromCTOR(List<TypeDefinition> list)
        {
            List<int> idlist = new List<int>(list.Count);
            foreach (TypeDefinition typedef in list)
            {
                int id = 0;
                foreach (MethodDefinition method in typedef.Methods)
                {
                    if (method.Name == ".ctor" && !method.HasParameters)
                    {
                        //Trying to find assigment
                        foreach (Instruction instr in method.Body.Instructions)
                        {
                            if (instr.OpCode == OpCodes.Call)
                            {
                                var op = instr.Operand as MethodReference;
                                if (op != null)
                                {
                                    if (op.FullName.Contains("set_NewInventoryItemType"))
                                    {
                                        if (instr.Previous.Operand is int)
                                        {
                                            id = (int)instr.Previous.Operand;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (method.Name == ".cctor")
                    {
                        //Trying to find assigment
                        foreach (Instruction instr in method.Body.Instructions)
                        {
                            if (instr.OpCode == OpCodes.Stsfld)
                            {
                                FieldDefinition def = instr.Operand as FieldDefinition;
                                if (def != null)
                                {
                                    if (def.Name == "_itemType")
                                    {
                                        id = (int)instr.Previous.Operand;
                                    }
                                }
                            }
                        }
                    }
                }
                if (id == 0)
                {
                    id = TrySelectFromExceptions(typedef);
                }
                idlist.Add(id);
            }
            return idlist;
        }

        private int TrySelectFromExceptions(TypeDefinition typedef)
        {
            switch (typedef.FullName)
            {
                default:
                    return 0;
                case "SignsOfLife.Entities.Items.Materials.PlayerBackpack":
                    return 101;//14000;
                case "SignsOfLife.Entities.Items.Weapons.Melee.Fists":
                    return 100;
                case "SignsOfLife.Entities.Items.Consumables.Potions.ConsumableHealPotion":
                    return 21501;
                case "SignsOfLife.Entities.Items.Containers.KeyItemSatchel":
                    return 3010;

            }
        }

        private void DumpAsCache(List<TypeDefinition> list, List<int> idlist, Workspace workspace)
        {
            var strlist = Enumerable.Range(0, list.Count).Select(i => list[i].FullName+":"+idlist[i].ToString()).ToArray();
            workspace.SaveAllLines(strlist,"vanillaitems");
        }

        private static List<TypeDefinition> SelectWithArgumentlessCTOR(List<TypeDefinition> list)
        {
            List<TypeDefinition> emptyctors = new List<TypeDefinition>();
            foreach (TypeDefinition typedef in list)
            {
                var ctors = typedef.Methods.Where(o => o.Name == ".ctor").ToArray();
                foreach (MethodDefinition ctor in ctors)
                {
                    if (!ctor.HasParameters)
                    {
                        emptyctors.Add(typedef);
                    }
                }
            }
            return emptyctors;
        }

        private List<TypeDefinition> CollectDerrived(TypeDefinition inventoryItemType, TypeDefinition[] types)
        {
            var list = new List<TypeDefinition>();

            foreach (var type in types)
            {
                if (CheckRecursiveBase(type, inventoryItemType))
                {
                    //Console.WriteLine("Derrived: "+type.Name);
                    if (IsNotBlacklisted(type.FullName))
                    {
                        list.Add(type);
                    }
                }
            }
            return list;
        }

        private bool IsNotBlacklisted(string fullname)
        {
            if (fullname == "SignsOfLife.Entities.Items.Materials.MaterialBrownEgg")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private static bool CheckRecursiveBase(TypeDefinition type, TypeDefinition inventoryItemType)
        {
            var basetypeRef = type.BaseType;
            if (basetypeRef == null) { return false; }
            else
            {
                var basetype = basetypeRef.Resolve();
                if (basetype == inventoryItemType)
                {
                    return true;
                }
                else
                {
                    return CheckRecursiveBase(basetype, inventoryItemType);
                }
            }
        }

        private static TypeDefinition PickBaseMethod(ModuleDefinition module)
        {
            foreach (var type in module.Types)
            {
                if (type.Name == BaseMethodName)
                {
                    return type;
                }
            }
            return null;
        }
    }
}
