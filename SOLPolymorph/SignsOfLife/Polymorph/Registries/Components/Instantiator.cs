using SignsOfLife.Entities.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLPolymorph.SignsOfLife.Polymorph.Registries.Components
{
    internal class Instantiator
    {

        private Type _type;

        internal Instantiator(Type type)
        {
            _type = type;
        }

        public object CreateNew()
        {
            return Activator.CreateInstance(_type);
        }

    }
}
