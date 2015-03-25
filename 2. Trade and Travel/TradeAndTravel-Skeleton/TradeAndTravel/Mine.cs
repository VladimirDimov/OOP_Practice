﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeAndTravel
{
    class Mine : Location, IGatheringLocation
    {
        public Mine(string name)
            :base(name, LocationType.Mine)
        {
        }

        public ItemType GatheredType
        {
            get { return ItemType.Iron; }
        }

        public ItemType RequiredItem
        {
            get { return ItemType.Iron; }
        }

        public Item ProduceItem(string name)
        {
            return new Iron(name, this);
        }
    }
}
