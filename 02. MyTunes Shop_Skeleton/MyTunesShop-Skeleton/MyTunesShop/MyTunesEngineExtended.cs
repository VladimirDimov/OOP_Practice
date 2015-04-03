using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTunesShop
{
    class MyTunesEngineExtended : MyTunesEngine
    {
        protected override string GetSongReport(ISong song)
        {
            var songSalesInfo = this.mediaSupplies[song];
            StringBuilder songInfo = new StringBuilder();
            songInfo.AppendFormat("{0} ({1}) by {2}", song.Title, song.Year, song.Performer.Name).AppendLine()
                .AppendFormat("Genre: {0}, Price: ${1:F2}", song.Genre, song.Price).AppendLine()
                .AppendFormat("Rating: {0}", song.Ratings.Average())
                .AppendFormat("Supplies: {0}, Sold: {1}", songSalesInfo.Supplies, songSalesInfo.QuantitySold);
            return songInfo.ToString();
        }

        protected override void ExecuteInsertMediaCommand(string[] commandWords)
        {
            switch (commandWords[2])
            {
                case "album":
                    var performer = this.performers.FirstOrDefault(p => p.Name == commandWords[5]) as Performer;
                    if (performer == null)
                    {
                        this.Printer.PrintLine("The performer does not exist in the database.");
                        return;
                    }

                    var album = new Album(commandWords[3], decimal.Parse(commandWords[4]), performer,
                        commandWords[6],
                        int.Parse(commandWords[7]));
                    this.InsertAlbum(album, performer);
                    break;
                default:
                    break;
            }
        }

        private void InsertAlbum(IAlbum album, IPerformer performer)
        {
            this.media.Add(album);
            this.mediaSupplies.Add(album, new SalesInfo());
            this.Printer.PrintLine("Album {0} by {1} added successfully", album.Title, performer.Name);
        }

        protected override void ExecuteInsertCommand(string[] commandWords)
        {
            {
                switch (commandWords[1])
                {
                    case "song_to_album":
                        this.ExecuteInsertSongToAlbum(commandWords);                        
                        break;
                    case"member_to_band":
                        this.ExecuteInsertMemberToBand(commandWords);
                        break;
                    default:
                        base.ExecuteInsertCommand(commandWords);
                        break;
                }
            }
        }

        private void ExecuteInsertMemberToBand(string[] commandWords)
        {
            var band = this.performers.FirstOrDefault(p => p.Name == commandWords[2]) as IBand;
            if (band == null)
            {
                this.Printer.PrintLine("The band does not exist in the database.");
                return;
            }

            band.AddMember(commandWords[3]);
            this.Printer.PrintLine("The member <member_name> has been added to the band {0}.",
                band.Name);
        }

        private void ExecuteInsertSongToAlbum(string[] commandWords)
        {
            var album = this.media.FirstOrDefault(p => p.Title == commandWords[2]) as Album;
            if (album != null)
            {
                this.Printer.PrintLine("The album does not exist in the database.");
                return;
            }

            var song = this.media.FirstOrDefault(p => p.Title == commandWords[2]) as Song;
            if (song != null)
            {
                this.Printer.PrintLine("The song does not exist in the database.");
                return;
            }

            album.AddSong(song);

            this.Printer.PrintLine("The song {0} has been added to the album {1}.", 
                song.Title, album.Title);
        }

        protected override void ExecuteReportMediaCommand(string[] commandWords)
        {
            switch (commandWords[2])
            {
                case "album":
                    var album = this.media.FirstOrDefault(s => s is IAlbum && s.Title == commandWords[3]) as IAlbum;
                    if (album == null)
                    {
                        this.Printer.PrintLine("The album does not exist in the database.");
                        return;
                    }

                    this.Printer.PrintLine(this.GetAlbumReport(album));
                    break;
                default:
                    base.ExecuteReportMediaCommand(commandWords);
                    break;
            }

        }

        private string GetAlbumReport(IAlbum album)
        {
            var albumSalesInfo = this.mediaSupplies[album];
            StringBuilder albumInfo = new StringBuilder();
            albumInfo.AppendFormat("{0} ({1}) by {2}", album.Title, album.Year, album.Performer.Name).AppendLine()
                .AppendFormat("Genre: {0}, Price: ${1:F2}", album.Genre, album.Price).AppendLine()
                .AppendFormat("Supplies: {0}, Sold: {1}", albumSalesInfo.Supplies, albumSalesInfo.QuantitySold)
                .AppendFormat(album.Songs.Count == 0 ? "No Songs" : string.Join(Environment.NewLine, album.Songs))
                .Append("...");

            return albumInfo.ToString();
        }

        protected override void ExecuteSupplyCommand(string[] commandWords)
        {
            switch (commandWords[1])
            {
                case "album":
                    var album = this.media.FirstOrDefault(s => s is IAlbum && s.Title == commandWords[2]) as IAlbum;
                    if (album == null)
                    {
                        this.Printer.PrintLine("The album does not exist in the database.");
                        return;
                    }

                    int quantity = int.Parse(commandWords[3]);
                    this.mediaSupplies[album].Supply(quantity);
                    this.Printer.PrintLine("{0} items of album {1} successfully supplied.", quantity, album.Title);
                    break;
                default:
                    base.ExecuteSupplyCommand(commandWords);
                    break;
            }
        }

        protected override void ExecuteSellCommand(string[] commandWords)
        {
            switch (commandWords[1])
            {
                case "album":
                    var album = this.media.FirstOrDefault(s => s is IAlbum && s.Title == commandWords[2]) as IAlbum;
                    if (album == null)
                    {
                        this.Printer.PrintLine("The album does not exist in the database.");
                        return;
                    }

                    int quantity = int.Parse(commandWords[3]);
                    this.mediaSupplies[album].Sell(quantity);
                    this.Printer.PrintLine("{0} items of album {1} successfully sold.", quantity, album.Title);
                    break;
                default:
                    base.ExecuteSellCommand(commandWords);
                    break;
            }
        }

        protected override void ExecuteReportPerformerCommand(string[] commandWords)
        {
            switch (commandWords[2])
            {
                case "band":
                    var band = this.performers.FirstOrDefault(p => p is Band && p.Name == commandWords[3]) as Band;
                    if (band == null)
                    {
                        this.Printer.PrintLine("The band does not exist in the database.");
                        return;
                    }
                    this.Printer.PrintLine(this.GetBandReport(band));
                    break;
                case "performer":
                    var performer = this.performers.FirstOrDefault(p => p is Performer && p.Name == commandWords[3]) as Performer;
                    if (performer == null)
                    {
                        this.Printer.PrintLine("The performer does not exist in the database.");
                        return;
                    }
                    this.Printer.PrintLine(this.GetPerformerReport(performer));
                    break;
                default:
                    break;
            }

        }

        private string GetPerformerReport(Performer performer)
        {
            StringBuilder performerInfo = new StringBuilder();
            performerInfo.AppendFormat("{0} ({1}) by {2}", song.Title, song.Year, song.Performer.Name).AppendLine()
                .AppendFormat("Genre: {0}, Price: ${1:F2}", song.Genre, song.Price).AppendLine()
                .AppendFormat("Supplies: {0}, Sold: {1}", songSalesInfo.Supplies, songSalesInfo.QuantitySold);
            return performerInfo.ToString();
        }

        private string GetBandReport(Band band)
        {            
            return string.Format("{0} ({1}): {2}", 
                band.Name,
                string.Join(", ", band.Members), 
                band.Songs.Count == 0 ? "no songs" : string.Join("; ", band.Songs));
        }
    }
}
