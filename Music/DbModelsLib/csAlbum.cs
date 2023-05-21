using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DbModelsLib
{
    //used for view results
    public class dtoAlbum
    {
        [Key]       // for EFC Code first
        public Guid AlbumId { get; set; }

        public string Name { get; set; }
        public int ReleaseYear { get; set; }
        public long CopiesSold { get; set; }
    }

    //used to build table
    public class csAlbum
	{
        [Key]       // for EFC Code first
        public Guid AlbumId { get; set; }

        public string Name { get; set; }
		public int ReleaseYear { get; set; }
        public long CopiesSold { get; set; }

        public csAlbum()
		{
		}

        public static class Factory
        {
            public static csAlbum CreateRandom()
            {
                var rnd = new csRandomData();

                var _name = rnd.MusicBand;
                var _releaseYear = rnd.Next(1970, 1990);
                var _copiesSold = rnd.Next(50_000, 50_000_000);

                return new csAlbum
                {
                    Name = _name,
                    ReleaseYear = _releaseYear,
                    CopiesSold = _copiesSold
                };
            }
        }
    }
}

