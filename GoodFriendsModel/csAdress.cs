using System;
using System.ComponentModel.DataAnnotations;

namespace GoodFriendsModel
{
	public class csAdress
	{
        public string StreetAdress { get; set; }
        public int ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public override string ToString() => $"{StreetAdress}, {ZipCode} {City}, {Country}";


        public static class Factory
        {
            public static csAdress CreateRandom()
            {
                var rnd = new csRandomData();
                var country = rnd.Country;

                return new csAdress
                {
                    StreetAdress = rnd.StreetAdress(country),
                    ZipCode = rnd.ZipCode,
                    City = rnd.City(country),
                    Country = country,
                };
            }
        }
    }
}

