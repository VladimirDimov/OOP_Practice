using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightlifeEntertainment
{
    class CinemaEngineExtended : CinemaEngine
    {
        private IList<Ticket>soldTickets = new List<Ticket>();
        protected override void ExecuteInsertVenueCommand(string[] commandWords)
        {
            switch (commandWords[2])
            {
                case "opera":
                    var opera = new OperaHall(commandWords[3], commandWords[4], int.Parse(commandWords[5]));
                    this.Venues.Add(opera);
                    break;
                case "sports_hall":
                    var sportsHall = new SportHall(commandWords[3], commandWords[4], int.Parse(commandWords[5]));
                    this.Venues.Add(sportsHall);
                    break;
                case "concert_hall":
                    var concertHall = new ConcertHall(commandWords[3], commandWords[4], int.Parse(commandWords[5]));
                    this.Venues.Add(concertHall);
                    break;
                default:
                    base.ExecuteInsertVenueCommand(commandWords);
                    break;
            }
        }

        protected override void ExecuteInsertPerformanceCommand(string[] commandWords)
        {
            switch (commandWords[2])
            {
                case "theatre":
                    var venue = this.GetVenue(commandWords[5]);
                    var theater = new Theater(commandWords[3], decimal.Parse(commandWords[4]), venue, DateTime.Parse(commandWords[6] + " " + commandWords[7]));
                    this.Performances.Add(theater);
                    break;
                case "concert":
                    venue = this.GetVenue(commandWords[5]);
                    var concert = new Concert(commandWords[3], decimal.Parse(commandWords[4]), venue, DateTime.Parse(commandWords[6] + " " + commandWords[7]));
                    this.Performances.Add(concert);
                    break;

                default:
                    base.ExecuteInsertPerformanceCommand(commandWords);
                    break;
            }
        }

        protected override void ExecuteSupplyTicketsCommand(string[] commandWords)
        {
            var venue = this.GetVenue(commandWords[2]);
            var performance = this.GetPerformance(commandWords[3]);
            switch (commandWords[1])
            {
                case "student":
                    for (int i = 0; i < int.Parse(commandWords[4]); i++)
                    {
                        performance.AddTicket(new StudentTicket(performance));
                    }
                    break;
                case "vip":
                    for (int i = 0; i < int.Parse(commandWords[4]); i++)
                    {
                        performance.AddTicket(new VipTicket(performance));
                    }
                    break;
                default:
                    base.ExecuteSupplyTicketsCommand(commandWords);
                    break;
            }
        }

        protected override void ExecuteReportCommand(string[] commandWords)
        {
            var report = new List<string>();
            var performance = base.GetPerformance(commandWords[1]);
            PrintPerformanceReport(performance);
        }
        

        private decimal TicketsTotalPrice(IPerformance performance)
        {
            decimal totalPrice = 0;
            var soldTickets = performance.Tickets.Where(x => x.Status == TicketStatus.Sold);
            foreach (var ticket in soldTickets)
            {
                totalPrice += ticket.Price;
            }

            return totalPrice;
        }

        private List<ITicket> GetTicketsSold(IPerformance performance)
        {
            return  performance.Tickets.Where(x => x.Status == TicketStatus.Sold).ToList();
        }

        private string PrintPerformanceReport(IPerformance performance)
        {
            // <name>: <tickets_sold> ticket(s), total: $<total_price>
            // Venue: <venue_name> (<venue_location>)
            // Start time: <start_time>
            var report = new List<string>();

            report.Add(string.Format("{0}: {1} ticket(s), total: ${2}",
            performance.Name, GetTicketsSold(performance).Count, TicketsTotalPrice(performance)));
            report.Add(string.Format("Venue: <venue_name> (<venue_location>)",
                performance.Venue.Name, performance.Venue.Location));
            report.Add(string.Format("Start time: {0}", performance.StartTime));

            return string.Join(Environment.NewLine, report);
        }

        protected override void ExecuteFindCommand(string[] commandWords)
        {
            DateTime fromDate = DateTime.Parse(commandWords[2] + " " + commandWords[3]);
            var selectedPerformances = base.Performances.Where(x => x.StartTime >= fromDate);
            foreach (var performance in selectedPerformances)
            {
                performance.ToString();
            }
        }
    }
}
