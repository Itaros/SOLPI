using SOLPolymorph.SignsOfLife.Polymorph.Registries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLPolymorph.SignsOfLife.Polymorph.Hooks
{
    public static class StaticPrefabsHandlerHooks
    {

        public static object GetNewStaticPrefabByStaticPrefabType(int prefabID)
        {
            return ProjectRegistry.Instance.CreateNew(prefabID);
        }

    }
}
