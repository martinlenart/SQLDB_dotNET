using System;
using System.Collections.Generic;

namespace MusicModel
{
	public class csArtist
	{
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

