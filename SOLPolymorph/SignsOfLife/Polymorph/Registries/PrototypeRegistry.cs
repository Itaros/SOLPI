using SignsOfLife.Prefabs.StaticPrefabs;
using SOLPolymorph.SignsOfLife.Polymorph.Registries.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLPolymorph.SignsOfLife.Polymorph.Registries
{
    public class ProjectRegistry
    {
        private static ProjectRegistry _instance = new ProjectRegistry();
        public static ProjectRegistry Instance { get { return _instance; } }

        private SortedList<int, Instantiator> _index = new SortedList<int, Instantiator>();

        public StaticPrefab CreateNew(int id)
        {
            if (_index.ContainsKey(id))
            {
                return _index[id].CreateNew() as StaticPrefab;
            }
            else
            {
                throw new IndexOutOfRangeException("No such Item ID");
            }
        }

        public string GetStatusString()
        {
            return "ProjectRegistry, id range: " + _index.Count;
        }

        public void Register<T>(int id)
        {
            Register(typeof(T), id);
        }

        public void Register(Type type, int id)
        {
            var instantiator = new Instantiator(type);
            _index.Add(id, instantiator);
        }

    }
}
