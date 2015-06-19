using SignsOfLife;
using SignsOfLife.Entities.Items;
using SOLPIBase;
using SOLPolymorph.SignsOfLife.Polymorph.Hooks;
using SOLPolymorph.SignsOfLife.Polymorph.Registries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLPolymorph.SignsOfLife.Polymorph.Replacements
{
    public class OnGameLaunchItemIndexBuilder
    {

        private Workspace _workspace;

        public OnGameLaunchItemIndexBuilder()
        {
            _workspace = new Workspace();
            _workspace.MakeReady();
        }

        public void Execute()
        {
            #region Items
            var itemNames = _workspace.ReadAllLines("vanillaitems");

            //ContentHandler.GetTexture("eoe");

            var ass = AssemblyDirector.Instance.HostType.Assembly;

            Console.WriteLine("Starting item probing...");

            foreach (string itemDef in itemNames)
            {
                var components = itemDef.Split(':');
                var itemName = components[0];
                var id = int.Parse(components[1]);
                Type type = ass.GetType(itemName);
                ItemRegistry.Instance.Register(id, type);
                //ProbeAndDefine(type);
            }

            Console.WriteLine(ItemRegistry.Instance.GetStatusString()); 
            #endregion
            #region Projects
            var projNames = _workspace.ReadAllLines("vanillaprojects");

            Console.WriteLine("Starting projects probing...");
            foreach (string projDef in projNames)
            {
                var components = projDef.Split(':');
                var itemName = components[0];
                var id = int.Parse(components[1]);
                Type type = ass.GetType(itemName);
                ProjectRegistry.Instance.Register(type, id);
            }

            Console.WriteLine(ProjectRegistry.Instance.GetStatusString());
            #endregion

        }



    }
}
