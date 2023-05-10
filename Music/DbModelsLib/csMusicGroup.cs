using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DbModelsLib
{
	public class csMusicGroup
	{
        [Key]       // for EFC Code first
        public Guid MusicGroupId { get; set; }

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
                for (int i = 0; i < rnd.Next(4, 9); i++)
                {
                    _members.Add(csArtist.Factory.CreateRandom());
                }

                //Create between 5 and 20 Albums
                var _albums = new List<csAlbum>();
                for (int i = 0; i < rnd.Next(5, 21); i++)
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

