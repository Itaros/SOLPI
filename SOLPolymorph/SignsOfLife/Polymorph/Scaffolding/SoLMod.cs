using SOLPolymorph.SignsOfLife.Polymorph.Registries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLPolymorph.SignsOfLife.Polymorph.Scaffolding
{
    public abstract class SoLMod
    {

        public abstract void InitItems(ItemRegistry registry);

        public abstract void InitRecipes(RecipeRegistry registry);

        public abstract void InitProjects(ProjectRegistry registry);

    }
}
