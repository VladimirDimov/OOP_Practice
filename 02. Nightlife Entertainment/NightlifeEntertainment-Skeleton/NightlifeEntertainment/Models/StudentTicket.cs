using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NightlifeEntertainment
{
    public class StudentTicket : Ticket
    {
        private const decimal PriceEnhancement = 0.8m;
        public StudentTicket(IPerformance performance)
            : base(performance, TicketType.Student)
        {
        }

        protected override decimal CalculatePrice()
        {
            return base.CalculatePrice() * PriceEnhancement;
        }
    }
}
