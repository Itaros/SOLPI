using Microsoft.Xna.Framework;
using SignsOfLife;
using SignsOfLife.Entities.Items;
using SignsOfLife.Entities.Items.Furniture;
using SOLPolymorph.SignsOfLife.Polymorph.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleMod.Items
{
    public class GoldenChicken : InventoryItem
    {

        private static int _texid = TextureLoader.Instance.LoadTextureAsPNG("goldenchicken.png");

        public GoldenChicken()
            : base(0, 0, TextureLoader.Instance.GetTexture(_texid), new Rectangle(17,10,32,44))
            //: base("tech_chair")//ContentHandler.GetCompiledRectangle("SpriteSheets/InventorySpriteSheetMap", "tech_chair")
        {
            base.Name = "Golden Chicken";
            base.Description = "Looks like this poor thing was punished.";
            base.Category = "Decor";
            base.IsFlippable = true;
            base.NewInventoryItemType = (InventoryItemType)50000;
        }

    }
}
