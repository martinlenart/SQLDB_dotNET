﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DbModelsLib
{
	public class csAlbum
	{
        [Key]       // for EFC Code first
        public Guid AlbumId { get; set; }

        public string Name { get; set; }
		public int ReleaseYear { get; set; }

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

                return new csAlbum
                {
                    Name = _name,
                    ReleaseYear = _releaseYear
                };
            }
        }
    }
}

