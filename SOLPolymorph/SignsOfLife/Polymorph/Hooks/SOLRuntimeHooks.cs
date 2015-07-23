using SignsOfLife;
using SOLPolymorph.SignsOfLife.Polymorph.Registries;
using SOLPolymorph.SignsOfLife.Polymorph.Replacements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLPolymorph.SignsOfLife.Polymorph.Hooks
{
    public static class SOLRuntimeHooks
    {

        public static void OnEntryPoint(object obj)
        {
            Console.WriteLine("Entry point!");
            SpaceGame game = new SpaceGame();
            try
            {
                game.Run();
            }
            finally
            {
                game.Dispose();
            }
        }

        public static void OnSpaceGameLaunch(object obj)
        {
            Console.WriteLine("Game Launches!");
            AssemblyDirector.Instance.DefineHostType(obj.GetType());
        }

        public static void OnSpaceGameLaunchPost()
        {
            var indexBuilder = new OnGameLaunchItemIndexBuilder();
            indexBuilder.Execute();
            Console.WriteLine("Vanilla Items Index is configured!");

            ModRegistry.Instance.Collect();
            ModRegistry.Instance.Inject();
            ModRegistry.Instance.BootAll(ModRegistry.BootStage.ITEMS);
            ModRegistry.Instance.BootAll(ModRegistry.BootStage.PROJECTS);

        }

        public static void OnSpaceGameLoadContent(object obj)
        {
            Console.WriteLine("Game loads AGIS ^_^!");
        }

        public static void OnSpaceGameLoadContentPost()
        {
            ModRegistry.Instance.BootAll(ModRegistry.BootStage.RECIPES);
        }

    }
}
