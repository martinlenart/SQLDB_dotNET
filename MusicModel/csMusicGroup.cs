using System;
using System.Collections.Generic;

namespace MusicModel
{
	public class csMusicGroup
	{
		public string Name { get; set; }
		public int EstablishedYear { get; set; }

		public List<csArtist> Members { get; set; }
		public List<csAlbum>  Albums { get; set; }

        public override string ToString() =>
            $"{Name} with {Members.Count} members was esblished {EstablishedYear} and made {Albums.Count} great albums. ";

        public csMusicGroup()
		{
		}

        public static class Factory
        {
            public static csMusicGroup CreateRandom()
            {
                var rnd = new csRandomData();

                var _name = rnd.MusicBand;

                //Create between 3 and 8 Members
                var _members = new List<csArtist>();
                for (int i = 3; i < rnd.Next(4, 9); i++)
                {
                    _members.Add(csArtist.Factory.CreateRandom());
                }

                //Create between 5 and 16 Albums
                var _albums = new List<csAlbum>();
                for (int i = 5; i < rnd.Next(6, 17); i++)
                {
                    _albums.Add(csAlbum.Factory.CreateRandom());
                }

                var _establishedYear = _albums.Min(a => a.ReleaseYear);
                var _mostSoldAlbum = _albums[rnd.Next(0, _albums.Count)];
                var _mostSoldCopies = rnd.Next(1_000_000, 30_000_000);

                return new csMusicGroup
                {
                    Name = _name,
                    EstablishedYear = _establishedYear,
                    Members = _members,
                    Albums = _albums
                };
            }
        }
    }
}

