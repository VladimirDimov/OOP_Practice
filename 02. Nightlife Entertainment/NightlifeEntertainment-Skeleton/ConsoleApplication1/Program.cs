using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading;


    public class Cinema : Venue
    {
        public Cinema(string name, string location, int numberOfSeats)
            : base(name, location, numberOfSeats, new List<PerformanceType> { PerformanceType.Movie, PerformanceType.Concert })
        {
        }
    }


    public class CinemaEngine
    {
        private StringBuilder output;

        public CinemaEngine()
        {
            this.output = new StringBuilder();
            this.Venues = new List<IVenue>();
            this.Performances = new List<IPerformance>();
        }

        public StringBuilder Output
        {
            get
            {
                return this.output;
            }
        }

        protected IList<IVenue> Venues { get; private set; }

        protected IList<IPerformance> Performances { get; private set; }

        public void ParseCommand(string command)
        {
            string[] commandWords = command.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                this.DispatchCommand(commandWords);
            }
            catch (Exception e)
            {
                this.Output.AppendLine(e.Message);
            }
        }

        protected virtual void ExecuteInsertCommand(string[] commandWords)
        {
            switch (commandWords[1])
            {
                case "venue":
                    this.ExecuteInsertVenueCommand(commandWords);
                    break;
                case "performance":
                    this.ExecuteInsertPerformanceCommand(commandWords);
                    break;
                default:
                    break;
            }
        }

        protected virtual void ExecuteSellTicketCommand(string[] commandWords)
        {
            var performance = this.GetPerformance(commandWords[1]);
            var type = (TicketType)Enum.Parse(typeof(TicketType), commandWords[2], true);
            this.output.Append(performance.SellTicket(type));
        }

        protected virtual void ExecuteReportCommand(string[] commandWords)
        {
            throw new NotImplementedException();
        }

        protected virtual void ExecuteSupplyTicketsCommand(string[] commandWords)
        {
            var venue = this.GetVenue(commandWords[2]);
            var performance = this.GetPerformance(commandWords[3]);
            switch (commandWords[1])
            {
                case "regular":
                    for (int i = 0; i < int.Parse(commandWords[4]); i++)
                    {
                        performance.AddTicket(new RegularTicket(performance));
                    }

                    break;
                default:
                    break;
            }
        }

        protected virtual void ExecuteInsertVenueCommand(string[] commandWords)
        {
            switch (commandWords[2])
            {
                case "cinema":
                    var cinema = new Cinema(commandWords[3], commandWords[4], int.Parse(commandWords[5]));
                    this.Venues.Add(cinema);
                    break;
                default:
                    break;
            }
        }

        protected virtual void ExecuteInsertPerformanceCommand(string[] commandWords)
        {
            switch (commandWords[2])
            {
                case "movie":
                    var venue = this.GetVenue(commandWords[5]);
                    var movie = new Movie(commandWords[3], decimal.Parse(commandWords[4]), venue, DateTime.Parse(commandWords[6] + " " + commandWords[7]));
                    this.Performances.Add(movie);
                    break;
                default:
                    break;
            }
        }

        protected IVenue GetVenue(string name)
        {
            var venue = this.Venues.FirstOrDefault(v => v.Name == name);
            if (venue == null)
            {
                throw new InvalidOperationException("There is no venue with the given name in the database.");
            }

            return venue;
        }

        protected IPerformance GetPerformance(string name)
        {
            var performance = this.Performances.FirstOrDefault(v => v.Name == name);
            if (performance == null)
            {
                throw new InvalidOperationException("There is no performance with the given name in the database.");
            }

            return performance;
        }

        protected virtual void ExecuteFindCommand(string[] commandWords)
        {
            throw new NotImplementedException();
        }

        private void DispatchCommand(string[] commandWords)
        {
            switch (commandWords[0])
            {
                case "insert":
                    this.ExecuteInsertCommand(commandWords);
                    break;
                case "supply_tickets":
                    this.ExecuteSupplyTicketsCommand(commandWords);
                    break;
                case "sell_ticket":
                    this.ExecuteSellTicketCommand(commandWords);
                    break;
                case "report":
                    this.ExecuteReportCommand(commandWords);
                    break;
                case "find":
                    this.ExecuteFindCommand(commandWords);
                    break;
                default:
                    break;
            }
        }
    }


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


    public class Concert : Performance
    {
        public Concert(string name, decimal basePrice, IVenue venue, DateTime startTime)
            : base(name, basePrice, venue, startTime, PerformanceType.Concert)
        {
        }

        protected override void ValidateVenue()
        {
            if (!this.Venue.AllowedTypes.Contains(PerformanceType.Concert) || !this.Venue.AllowedTypes.Contains(PerformanceType.Opera))
            {
                throw new InvalidOperationException(
                    string.Format("The venue {0} does not support the type of performance {1}", this.Venue.Name, this.Type));
            }
        }
    }


    public class ConcertHall : Venue
    {
        public ConcertHall(string name, string location, int numberOfSeats)
            : base(name, location, numberOfSeats, new List<PerformanceType> { PerformanceType.Opera, PerformanceType.Theatre, PerformanceType.Concert })
        {            
        }
    }


    public interface IPerformance
    {
        string Name { get; }

        decimal BasePrice { get; }
        
        IVenue Venue { get; }

        DateTime StartTime { get; }

        PerformanceType Type { get; }

        IList<ITicket> Tickets { get; }

        void AddTicket(ITicket ticket);

        string SellTicket(TicketType type);
    }


    public interface ITicket
    {
        IPerformance Performance { get; }

        decimal Price { get; }

        int Seat { get; }

        TicketStatus Status { get; }

        TicketType Type { get; }

        void AssignSeat(int seat);

        void Sell();
    }


    public interface IVenue
    {
        string Name { get; }

        string Location { get; }

        int Seats { get; }

        IList<PerformanceType> AllowedTypes { get; }
    }


    public class Movie : Performance
    {
        public Movie(string name, decimal basePrice, IVenue venue, DateTime startTime)
            : base(name, basePrice, venue, startTime, PerformanceType.Movie)
        {
        }

        protected override void ValidateVenue()
        {
            if (!this.Venue.AllowedTypes.Contains(PerformanceType.Movie) &&
                !this.Venue.AllowedTypes.Contains(PerformanceType.Theatre))
            {
                throw new InvalidOperationException(
                    string.Format("The venue {0} does not support the type of performance {1}", this.Venue.Name, this.Type));
            }
        }
    }


    public class NightlifeEntertainmentProgram
    {
        public static void Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            CinemaEngine engine = new CinemaEngineExtended();
            StartOperations(engine);
        }

        private static void StartOperations(CinemaEngine engine)
        {
            string line = Console.ReadLine();
            while (line != "end")
            {
                engine.ParseCommand(line);
                line = Console.ReadLine();
            }

            Console.Write(engine.Output);
        }
    }


    public class OperaHall : Venue
    {
        public OperaHall(string name, string location, int numberOfSeats)
            : base(name, location, numberOfSeats, new List<PerformanceType>{ PerformanceType.Opera, PerformanceType.Theatre})
        {
        }
    }

    public abstract class Performance : IPerformance
    {
        private string name;
        private decimal basePrice;
        private IVenue venue;
        private IList<ITicket> tickets;

        public Performance(string name, decimal basePrice, IVenue venue, DateTime startTime, PerformanceType type)
        {
            this.Name = name;
            this.BasePrice = basePrice;
            this.Venue = venue;
            this.ValidateVenue();
            this.StartTime = startTime;
            this.Type = type;
            this.tickets = new List<ITicket>();
        }

        public string Name
        {
            get
            {
                return this.name;
            }

            protected set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("The performance name is required.");
                }

                this.name = value;
            }
        }

        public decimal BasePrice
        {
            get
            {
                return this.basePrice;
            }

            protected set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("The performance base price should be positive.");
                }

                this.basePrice = value;
            }
        }

        public IVenue Venue
        {
            get
            {
                return this.venue;
            }

            protected set
            {
                if (value == null)
                {
                    throw new ArgumentException("The performance venue should be positive.");
                }

                this.venue = value;
            }
        }

        public DateTime StartTime { get; protected set; }

        public PerformanceType Type { get; protected set; }

        public IList<ITicket> Tickets
        {
            get
            {
                return this.tickets;
            }
        }

        public void AddTicket(ITicket ticket)
        {
            if (this.tickets.Count == this.Venue.Seats)
            {
                throw new InvalidOperationException("There are no seats left for this performance.");
            }

            ticket.AssignSeat(this.tickets.Count + 1);
            this.tickets.Add(ticket);
        }

        public string SellTicket(TicketType type)
        {
            var ticket = this.tickets.FirstOrDefault(t => t.Status == TicketStatus.Unsold && t.Type == type);
            if (ticket == null)
            {
                throw new ArgumentException("There is no unsold ticket of the specified type.");
            }

            ticket.Sell();
            return ticket.ToString();
        }

        protected abstract void ValidateVenue();
    }

   
    public enum PerformanceType
    {
        Movie,
        Opera,
        Theatre,
        Sport,
        Concert
    }


    public class RegularTicket : Ticket
    {
        public RegularTicket(IPerformance performance)
            : base(performance, TicketType.Regular)
        {
        }
    }


    public class SportHall : Venue
    {
        public SportHall(string name, string location, int numberOfSeats)
            : base(name, location, numberOfSeats, new List<PerformanceType> {PerformanceType.Sport, PerformanceType.Concert})
        {
        }
    }


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





    public class Theater : Performance
    {
        public Theater(string name, decimal basePrice, IVenue venue, DateTime startTime)
            : base(name, basePrice, venue, startTime, PerformanceType.Theatre)
        {
        }

        protected override void ValidateVenue()
        {
            if (!this.Venue.AllowedTypes.Contains(PerformanceType.Theatre))
            {
                throw new InvalidOperationException(
                    string.Format("The venue {0} does not support the type of performance {1}", this.Venue.Name, this.Type));
            }
        }
    }


    public abstract class Ticket : ITicket
    {
        private decimal price;
        private IPerformance performance;
        private int seat;
        private TicketStatus status;

        public Ticket(IPerformance performance, TicketType type)
        {
            this.Performance = performance;
            this.Price = this.CalculatePrice();
            this.status = TicketStatus.Unsold;
            this.Type = type;
        }

        public decimal Price
        {
            get
            {
                return this.price;
            }

            protected set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("The ticket price must be positive.");
                }

                this.price = value;
            }
        }

        public IPerformance Performance
        {
            get
            {
                return this.performance;
            }

            protected set
            {
                if (value == null)
                {
                    throw new ArgumentException("The performance is required.");
                }

                this.performance = value;
            }
        }

        public int Seat
        {
            get
            {
                return this.seat;
            }

            protected set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("The seat number must be positive.");
                }

                if (this.Performance == null)
                {
                    throw new InvalidOperationException("The performance must be initialized before assigning a seat.");
                }

                if (value > this.Performance.Venue.Seats)
                {
                    throw new ArgumentException("The seat number must not exceed the capacity of the venue.");
                }

                this.seat = value;
            }
        }

        public TicketStatus Status
        {
            get
            {
                return this.status;
            }
        }

        public TicketType Type { get; private set; }

        public void AssignSeat(int seat)
        {
            this.Seat = seat;
        }

        public void Sell()
        {
            this.status = TicketStatus.Sold;
        }

        public override string ToString()
        {
            var ticket = new StringBuilder();
            ticket.AppendFormat("{0} {1} {0}", new string('=', 5), this.Performance.Name).AppendLine()
                .AppendFormat("At {0} ({1})", this.Performance.Venue.Name, this.Performance.Venue.Location).AppendLine()
                .AppendFormat("On {0}", this.Performance.StartTime).AppendLine()
                .AppendFormat("Seat: {0}", this.Seat).AppendLine()
                .AppendFormat("Price: ${0:F2}", this.Price).AppendLine()
                .AppendLine(new string('=', 15));

            return ticket.ToString();
        }

        protected virtual decimal CalculatePrice()
        {
            if (this.Performance == null)
            {
                throw new ArgumentException("The price cannot be calculated because there is no performance for this ticket.");
            }

            return this.Performance.BasePrice;
        }
    }


    public enum TicketStatus
    {
        Sold,
        Unsold
    }


    public enum TicketType
    {
        Regular,
        Student,
        VIP
    }


    public abstract class Venue : IVenue
    {
        private const int MinNumberOfSeats = 10;

        private string name;
        private string location;
        private int numberOfSeats;
        private IList<PerformanceType> allowedTypes;

        public Venue(string name, string location, int numberOfSeats, IList<PerformanceType> allowedTypes)
        {
            this.Name = name;
            this.Location = location;
            this.Seats = numberOfSeats;
            this.AllowedTypes = allowedTypes;
        }

        public string Name
        {
            get
            {
                return this.name;
            }

            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("The venue name is required.");
                }

                this.name = value;
            }
        }

        public string Location
        {
            get
            {
                return this.location;
            }

            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("The venue location is required.");
                }

                this.location = value;
            }
        }

        public int Seats
        {
            get
            {
                return this.numberOfSeats;
            }

            private set
            {
                if (value <= MinNumberOfSeats)
                {
                    throw new ArgumentException(string.Format("The seats must be at least {0}.", MinNumberOfSeats));
                }

                this.numberOfSeats = value;
            }
        }

        public IList<PerformanceType> AllowedTypes
        {
            get
            {
                return this.allowedTypes;
            }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentException("The allowed performance types are required.");
                }

                this.allowedTypes = value;
            }
        }
    }


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