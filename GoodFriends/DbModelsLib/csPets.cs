using System;
using System.ComponentModel.DataAnnotations;

namespace DbModelsLib
{
    public enum enAnimal { Dog, Cat, Rabbit, Fish, Bird, LastEnum };
    public class csPet
	{
        [Key]       // for EFC Code first
        public Guid PetId { get; set; }

        public enAnimal AnimalKind { get; set; }
		public string Name { get; set; }

        public override string ToString() => $"{Name} the {AnimalKind}";

        public static class Factory
        {
            public static csPet CreateRandom()
            {
                var rnd = new csRandomData();

                return new csPet
                {
                    PetId = Guid.NewGuid(),

                    Name = rnd.PetName,
                    AnimalKind = (enAnimal) rnd.Next((int)enAnimal.Dog, (int)enAnimal.LastEnum)
                };
            }
        }
    }
}

