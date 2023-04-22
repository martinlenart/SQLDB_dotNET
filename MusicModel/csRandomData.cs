using System;
using System.Collections.Generic;

namespace MusicModel
{
    public class GoodQuote
    {
        public string Quote { get; set; }
        public string Author { get; set; }

        public GoodQuote() {}

        public GoodQuote(string qoute, string author)
        {
            Quote = qoute;
            Author = author;
        }
    }


    public class csRandomData : Random
    {

        string[] _firstnames = "Harry, Lord, Hermione, Albus, Severus, Ron, Draco, Frodo, Gandalf, Sam, Peregrin, Saruman".Split(", ");
        string[] _lastnames = "Potter, Voldemort, Granger, Dumbledore, Snape, Malfoy, Baggins, the Gray, Gamgee, Took, the White".Split(", ");


        string[] _petnames = "Max, Charlie, Cooper, Milo, Rocky, Wanda, Teddy, Duke, Leo, Max, Simba".Split(", ");

        string[][] _city =
            {
                "Stockholm, Göteborg, Malmö, Uppsala, Linköping, Örebro".Split(", "),
                "Oslo, Bergen, Trondheim, Stavanger, Dramen".Split(", "),
                "Köpenhamn, Århus, Odense, Aahlborg, Esbjerg".Split(", "),
                "Helsingfors, Espoo, Tampere, Vaanta, Oulu".Split(", "),
             };

        string[][] _adress =
            {
                "Svedjevägen, Ringvägen, Vasagatan, Odenplan, Birger Jarlsgatan, Äppelviksvägen, Kvarnbacksvägen".Split(','),
                "Bygdoy alle, Frognerveien, Pilestredet, Vidars gate, Sågveien, Toftes gate, Gardeveiend".Split(','),
                "Rolighedsvej, Fensmarkgade, Svanevej, Gröndalsvej, Githersgade, Classensgade, Moltekesvej".Split(','),
                "Arkandiankatu, Liisankatu, Ruoholahdenkatu, Pohjoistranta, Eerikinkatu, Vauhtitie, Itainen Vaideki".Split(',')
            };

        string[] _country = "Sweden, Norway, Denmark, Finland".Split(", ");


        string[] _domains = "icloud.com, me.com, mac.com, hotmail.com, gmail.com".Split(", ");


        GoodQuote[] _quotes = {

            //About Love
            new GoodQuote("Would I rather be feared or loved? Easy. Both. I want people to be afraid of how much they love me.", "Michael Scott, The Office"),
            new GoodQuote("All you need is love. But a little chocolate now and then doesn’t hurt.", "Charles M. Schulz"),
            new GoodQuote("Before you marry a person, you should first make them use a computer with slow Internet to see who they really are.", "Will Ferrell"),
            new GoodQuote("I love being married. It’s so great to find one special person you want to annoy for the rest of your life.", "Rita Rudner"),
            new GoodQuote("If love is the answer, can you please rephrase the question?", "Lily Tomlin"),
            new GoodQuote("Love can change a person the way a parent can change a baby—awkwardly, and often with a great deal of mess.", "Lemony Snicket"),
            new GoodQuote("Love is a fire. But whether it is going to warm your hearth or burn down your house, you can never tell.", "Joan Crawford"),
            new GoodQuote("A successful marriage requires falling in love many times, always with the same person.", "Mignon McLaughlin"),
            new GoodQuote("I love you with all my belly. I would say my heart, but my belly is bigger.", "Unknown"),
            new GoodQuote("The four most important words in any marriage—I’ll do the dishes.", "Unknown"),
            new GoodQuote("I love you more than coffee but not always before coffee.", "Unknown"),
            new GoodQuote("You know that tingly little feeling you get when you like someone? That’s your common sense leaving your body.", "Unknown"),

            //About Work
            new GoodQuote("I choose a lazy person to do a hard job, because a lazy person will find an easy way to do it.", "Bill Gates"),
            new GoodQuote("Doing nothing is very hard to do… you never know when you’re finished.", "Leslie Nielsen"),
            new GoodQuote("It takes less time to do a thing right, than it does to explain why you did it wrong.", "Henry Wadsworth Longfellow"),
            new GoodQuote("Most of what we call management consists of making it difficult for people to get their work done.", "Peter Drucker"),
            new GoodQuote("It is better to have one person working with you than three people working for you.", "Dwight D. Eisenhower"),
            new GoodQuote("The best way to appreciate your job is to imagine yourself without one.", "Oscar Wilde"),
            new GoodQuote("I hate when I lose things at work, like pens, papers, sanity and dreams.", "Unknown"),
            new GoodQuote("Creativity is allowing yourself to make mistakes. Art is knowing which ones to keep.", "Scott Adams"),
            new GoodQuote("My keyboard must be broken, I keep hitting the escape key, but I’m still at work.", "Unknown"),
            new GoodQuote("Work is against human nature. The proof is that it makes us tired.", "Michel Tournier"),
            new GoodQuote("The reward for good work is more work.", "Francesca Elisia"),
            new GoodQuote("Executive ability is deciding quickly and getting somebody else to do the work.", "Earl Nightingale"),

        };


        string[] _musicbands = ("Led, Zeppelin, Queen, Pink, Floyd, Creedence, Clearwater, Revival, " +
            "Arosmith, Who, AC/DC, Rolling, Stones, Eagles, Deep, Purple, Prince, Dylan").Split(", ");
        string[] _musicalbums = ("Heaven, Rock, Moon, Cosmos, Walk, Hunky, Blue, Highway" +
            "Satisfaction, Californnia, Stairway, Purple, Senor").Split(", ");

        public string MusicBand => "The " + _musicbands[this.Next(0, _musicbands.Length)] + " " + _musicbands[this.Next(0, _musicbands.Length)];
        public string MusicAlbum => _musicalbums[this.Next(0, _musicalbums.Length)] + " " + _musicalbums[this.Next(0, _musicalbums.Length)];

        public string PetName => _petnames[this.Next(0, _petnames.Length)];

        public string FirstName => _firstnames[this.Next(0, _firstnames.Length)];
        public string LastName => _lastnames[this.Next(0, _lastnames.Length)];
        public string FullName => $"{FirstName} {LastName}";

        public DateTime getDateTime(int? fromYear = null, int? toYear = null)
        {
            bool dateOK = false;
            DateTime _date = default;
            while (!dateOK)
            {
                fromYear ??= DateTime.Today.Year;
                toYear ??= DateTime.Today.Year + 1;

                try
                {
                    int year = this.Next(Math.Min(fromYear.Value, toYear.Value),
                        Math.Max(fromYear.Value, toYear.Value));
                    int month = this.Next(1, 13);
                    int day = this.Next(1, 32);

                    _date = new DateTime(year, month, day);
                    dateOK = true;
                }
                catch
                {
                    dateOK = false;
                }
            }

            return DateTime.SpecifyKind(_date, DateTimeKind.Utc);  //Used for Postgres compatibility - only UTC is supported
        }

        //General random truefalse
        public bool Bool => (this.Next(0, 10) < 5) ? true : false;

        public string Email(string fname = null, string lname = null)
        {
            fname ??= FirstName;
            lname ??= LastName;

            return $"{fname}.{lname}@{_domains[this.Next(0, _domains.Length)]}";
        }

        public string Phone => $"{this.Next(700, 800)} {this.Next(100, 1000)} {this.Next(100, 1000)}";

        public string Country => _country[this.Next(0, _country.Length)];

        public string City(string Country = null)
        {

            var cIdx = this.Next(0, _city.Length);
            if (Country != null)
            {
                //Give a City in that specific country
                cIdx = Array.FindIndex(_country, c => c.ToLower() == Country.Trim().ToLower());

                if (cIdx == -1) throw new Exception("Country not found");
            }

            return _city[cIdx][this.Next(0, _city[cIdx].Length)];
        }

        public string StreetAdress(string Country = null)
        {

            var cIdx = this.Next(0, _city.Length);
            if (Country != null)
            {
                //Give a City in that specific country
                cIdx = Array.FindIndex(_country, c => c.ToLower() == Country.Trim().ToLower());

                if (cIdx == -1) throw new Exception("Country not found");
            }

            return $"{_adress[cIdx][this.Next(0, _adress[cIdx].Length)]} {this.Next(1, 51)}";
        }

        public int ZipCode => this.Next(10101, 100000);
        public GoodQuote Quote => _quotes[this.Next(0, _quotes.Length)];


        //Statics
        public static List<GoodQuote> AllQuotes
        {
            get
            {
                return new List<GoodQuote>(new csRandomData()._quotes);
            }
        }
    }
}

