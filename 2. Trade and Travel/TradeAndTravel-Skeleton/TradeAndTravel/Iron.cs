using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeAndTravel
{
    class Iron : Item
    {
        private const int IronValue = 3;

        public Iron(string name, Location location)
            : base(name, IronValue, ItemType.Iron, location)
        {            
        }
    }
}
