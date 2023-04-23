using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DbModelsLib
{
	public class csArtist
	{
        [Key]       // for EFC Code first
        public Guid ArtistId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public csArtist()
		{
		}

        public static class Factory
        {
            public static csArtist CreateRandom()
            {
                var rnd = new csRandomData();

                var fn = rnd.FirstName;
                var ln = rnd.LastName;

                return new csArtist
                {
                    FirstName = fn,
                    LastName = ln
                };
            }
        }
    }
}

