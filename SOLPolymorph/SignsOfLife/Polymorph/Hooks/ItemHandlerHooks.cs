using SOLPolymorph.SignsOfLife.Polymorph.Registries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLPolymorph.SignsOfLife.Polymorph.Hooks
{
    public static class ItemHandlerHooks
    {

        public static object GetNewItemByItemType(int inventoryItemType)
        {
            return ItemRegistry.Instance.CreateNew(inventoryItemType);
        }

    }
}
