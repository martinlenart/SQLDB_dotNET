using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DbModelsLib
{
    //used for view results
    public class dtoArtist
    {
        [Key]       // for EFC Code first
        public Guid ArtistId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime? BirthDay { get; set; }
    }

    //used to build table
    public class csArtist
	{
        [Key]       // for EFC Code first
        public Guid ArtistId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime? BirthDay { get; set; }

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
                DateTime? _birthday = (rnd.Bool)?rnd.getDateTime(1940, 1990) :null; 

                return new csArtist
                {
                    FirstName = fn,
                    LastName = ln,
                    BirthDay = _birthday
                };
            }
        }
    }
}

