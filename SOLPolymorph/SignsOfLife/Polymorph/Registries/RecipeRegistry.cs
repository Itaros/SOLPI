using SignsOfLife.Crafting;
using SignsOfLife.UI;
using SignsOfLife.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLPolymorph.SignsOfLife.Polymorph.Registries
{
    public class RecipeRegistry
    {

        private static RecipeRegistry _instance = new RecipeRegistry();
        public static RecipeRegistry Instance { get { return _instance; } }

        public void ForceInject(Recipe recipe)
        {
            Config.GetRecipes().Add(recipe);
            AddRecipesToHud(recipe);
        }

        private static void AddRecipesToHud(Recipe recipe)
        {
            Hud._allRecipes.Add(recipe);
            if (!Hud._recipeByNameDict.ContainsKey(recipe.Name))
            {
                Hud._recipeByNameDict.Add(recipe.Name, recipe);
            }
        }

        public void ForceInject(SmeltingRecipe recipe)
        {
            Config.SmeltingRecipes.Add(recipe);
        }

        public void ForceInject(Blueprint recipe)
        {
            Config.GetBlueprints().Add(recipe);
        }

    }
}
