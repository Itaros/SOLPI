using ExampleMod.Items;
using SOLPolymorph.SignsOfLife.Polymorph.Registries;
using SOLPolymorph.SignsOfLife.Polymorph.Scaffolding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleMod
{
    public class ChickenStatue : SoLMod
    {
        public override void InitItems(ItemRegistry registry)
        {
            registry.Register<GoldenChicken>(50000);
        }

        public override void InitRecipes(RecipeRegistry registry)
        {
            
        }

        public override void InitProjects(ProjectRegistry registry)
        {
            
        }
    }
}
