using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SOLPolymorph.SignsOfLife.Polymorph.Hooks
{
    public static class MapHooks
    {

        public static void GenerateOnStart(object map)
        {
            Console.WriteLine("Generator is catched!");

            MethodInfo[] methods = map.GetType().GetMethods();

            foreach (MethodInfo method in methods)
            {
                if (method.Name == "Generate" && method.GetParameters().Length == 2)//Generate(MapGenType worldGenType, int seed)
                {
                    Console.WriteLine("Coregen method identified");
                    //method.Invoke(map, new object[] { 2,1});
                    break;
                }
            }

        }

    }
}
