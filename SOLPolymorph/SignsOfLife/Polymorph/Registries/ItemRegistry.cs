using SignsOfLife.Entities.Items;
using SOLPolymorph.SignsOfLife.Polymorph.Registries.Components;
using System;
using System.Collections.Generic;

namespace SOLPolymorph.SignsOfLife.Polymorph.Registries
{
    public class ItemRegistry
    {

        private static ItemRegistry _instance = new ItemRegistry();
        public static ItemRegistry Instance { get { return _instance; } }

        //private ArrayList _index = new ArrayList();
        private SortedList<int,Instantiator> _index = new SortedList<int,Instantiator>();

        public void Register<T>(int id) where T : InventoryItem
        {
            var instantiator = new Instantiator(typeof(T));
            _index.Add(id, instantiator);
        }

        public void Register(int id, Type type)
        {
            if (id == 0) { Console.WriteLine("Skipped(NO ID): "+type.FullName); return; }
            if (!_index.ContainsKey(id))
            {
                var instantiator = new Instantiator(type);
                _index.Add(id, instantiator);
            }
            else
            {
                Console.WriteLine("Skipped(CLASH["+id+"]): " + type.FullName);
            }
        }

        public string GetStatusString()
        {
            return "ItemRegistry, id range: " + _index.Count;
        }

        public InventoryItem CreateNew(int id)
        {
            if (id == 0) { return null; }
            if (_index.ContainsKey(id))
            {
                return _index[id].CreateNew() as InventoryItem;
            }
            else
            {
                Console.WriteLine("Attempt to acquire item with faulty id: "+id);
                throw new IndexOutOfRangeException("No such Item ID");
            }
        }


    }
}
