using System;
namespace MusicModel
{
	public class csMusicGroup
	{
		public string Name { get; set; }
		public int EstablishedYear { get; set; }

		public csAlbum MostSoldAlbum { get; set; }

		public List<csArtist> Members { get; set; }
		public List<csAlbum>  Albums { get; set; }

		public csMusicGroup()
		{
		}
	}
}

