using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTunesShop
{
    class Album : Media , IAlbum
    {
        private static readonly int MinYear = DateTime.MinValue.Year;
        private static readonly int MaxYear = DateTime.Now.Year;

        private string title;
        private decimal price;
        private IPerformer performer;
        private string genre;
        private int year;
        private IList<ISong> songs;

        public Album(string title, decimal price, Performer performer, string genre, int year)
        {
            this.Title = title;
            this.Price = price;
            this.Performer = performer;
            this.Genre = genre;
            this.Year = year;
            this.songs = new List<ISong>();
        }

        public new string Title
        {
            get
            {
                return this.title;
            }

            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("The title of an album is required.");
                }

                this.title = value;
            }
        }

        public new decimal Price
        {
            get
            {
                return this.price;
            }

            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("The price of an album must be non-negative.");
                }

                this.price = value;
            }
        }

        public string Genre
        {
            get
            {
                return this.genre;
            }

            private set
            {
                this.genre = value;
            }
        }

        public int Year
        {
            get
            {
                return this.year;
            }

            private set
            {
                if (value < MinYear || value > MaxYear)
                {
                    throw new ArgumentException(string.Format("The year of an album must be between {0} and {1}.", MinYear, MaxYear));
                }

                this.year = value;
            }
        }


        public IPerformer Performer
        {
            get
            {
                return this.performer;
            }

            private set
            {
                this.performer = value;
            }
        }

        public IList<ISong> Songs
        {
            get
            {
                return this.songs;
            }
        }

        public void AddSong(ISong song)
        {
            this.Songs.Add(song);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
