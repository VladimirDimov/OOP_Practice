using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NightlifeEntertainment
{
    public class VipTicket : Ticket
    {
        private const decimal PriceEnhancement = 1.5m;

        public VipTicket(IPerformance performance)
            : base(performance, TicketType.VIP)
        {
        }

        protected override decimal CalculatePrice()
        {
            return base.CalculatePrice() * PriceEnhancement;
        }
    }
}
