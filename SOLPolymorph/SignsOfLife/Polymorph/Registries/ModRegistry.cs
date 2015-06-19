using SOLPolymorph.SignsOfLife.Polymorph.Exceptions;
using SOLPolymorph.SignsOfLife.Polymorph.Scaffolding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SOLPolymorph.SignsOfLife.Polymorph.Registries
{
    public class ModRegistry
    {

        private static ModRegistry _instance = new ModRegistry();
        public static ModRegistry Instance { get { return _instance; } }

        private static readonly string ModsDir = "Polymorph" + Path.DirectorySeparatorChar + "Mods";

        private List<ModDefinition> _mods = new List<ModDefinition>();

        internal void Collect()
        {
            _mods.Clear();
            DirectoryInfo modsdir = Directory.CreateDirectory(ModsDir);

            foreach (FileInfo file in modsdir.EnumerateFiles())
            {
                if (file.Extension == ".dll")
                {
                    Console.WriteLine("Mod found: "+file.Name);
                    _mods.Add(new ModDefinition(file));
                }
            }
            
        }


        public class ModDefinition
        {
            private FileInfo _modfile;
            private Assembly _assembly;

            private SoLMod _masterClass;

            public ModDefinition(FileInfo modfile)
            {
                _modfile = modfile;
            }


            internal void Inject()
            {
                try
                {
                    Console.WriteLine("Loading Mod: " + _modfile.Name);
                    _assembly = Assembly.LoadFrom(_modfile.FullName);

                    _masterClass = FindSoLMod();
                }
                catch (Exception e)
                {
                    throw new ModLoadingSequenceFailureException("Loading failed at initialization stage", e);
                }
            }

            private SoLMod FindSoLMod()
            {
                foreach (Type t in _assembly.GetTypes())
                {
                    if (t.BaseType == typeof(SoLMod))
                    {
                        return Activator.CreateInstance(t) as SoLMod;
                    }
                }
                return null;
            }

            internal void BootUp(BootStage stage)
            {
                Console.WriteLine("Booting up "+stage.ToString().ToLowerInvariant()+" for "+_modfile.Name);
                switch (stage)
                {
                    case BootStage.ITEMS:
                        _masterClass.InitItems(ItemRegistry.Instance);
                        break;
                    case BootStage.RECIPES:
                        _masterClass.InitRecipes(RecipeRegistry.Instance);
                        break;
                    case BootStage.PROJECTS:
                        _masterClass.InitProjects(ProjectRegistry.Instance);
                        break;
                    default:
                        throw new InvalidOperationException("Requested modload stage is unknown");
                }
            }
        }


        internal void Inject()
        {
            foreach (ModDefinition mod in _mods)
            {
                mod.Inject();
            }
        }

        public enum BootStage
        {
            UNKNOWN,
            ITEMS,
            RECIPES,
            PROJECTS,
            FINALIZATION
        }

        internal void BootAll(BootStage stage)
        {
            try
            {
                foreach (ModDefinition mod in _mods)
                {
                    mod.BootUp(stage);
                }
            }
            catch (Exception e)
            {
                throw new ModLoadingSequenceFailureException("Loading failed at "+stage.ToString(),e);
            }
        }
    }
}
