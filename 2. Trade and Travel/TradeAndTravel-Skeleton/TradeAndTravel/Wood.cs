using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeAndTravel
{
    class Wood : Item
    {
        private const int WoodValue = 2;

        public Wood(string name, Location location)
            : base(name, WoodValue, ItemType.Wood, location)
        {
        }

        public override void UpdateWithInteraction(string interaction)
        {
            if (interaction == "drop")
            {
                if (this.Value > 0)
                {
                    this.Value--;
                }
            }
        }
    }
}
