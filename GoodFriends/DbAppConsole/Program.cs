using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

using DbContextLib;
using DbModelsLib;
using System.Data.Common;

namespace DbAppConsole
{
    static class MyLinqExtensions
    {
        public static void Print<T>(this IEnumerable<T> collection)
        {
            collection.ToList().ForEach(item => Console.WriteLine(item));
        }
    }


    class Program
    {
        private static DbContextOptionsBuilder<DbContextLib.MainDbContext> _optionsBuilder;

        static void Main(string[] args)
        {
            //TestModelsOnly();

            //ensure connections to the databases
            Console.WriteLine($"\nGetting Database connection strings...");
            if (!AppConfig.AppSettingsExist)
            {
                Console.WriteLine($"Cannot find {AppConfig.Appsettingfile}");
                Console.WriteLine($"Please ensure {AppConfig.Appsettingfile} is copied to {AppConfig.AppSettingsDirectory}");
                return;
            }


            foreach (var db in AppConfig.DbMigrations)
            {
                Console.WriteLine($"\nConnecting to database...");
                _optionsBuilder = CreateDbContextOptions(db);
                if (_optionsBuilder == null)
                {
                    Console.WriteLine($"Db Context could not be created");
                    Console.WriteLine($"Please ensure {AppConfig.Appsettingfile} at {AppConfig.AppSettingsDirectory} is updated.");

                    return;
                }

                Console.WriteLine($"\nSeeding database...");
                try
                {
                    SeedDataBase();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError: Database could not be seeded. Ensure the {db.DbType} database is correctly created");
                    return;
                }

                Console.WriteLine("\nQuery database...");
                QueryDatabaseAsync().Wait();
            }

            Console.WriteLine("\nPress any key to terminate");
            Console.ReadKey();

        }

        private static void TestModelsOnly()
        {
            Console.WriteLine($"\nTesting model only!");

            //Create a list of friends
            var _goodfriends = new List<csFriend>();
            for (int c = 0; c < 20; c++)
            {
                _goodfriends.Add(csFriend.Factory.CreateRandom());
            }

            foreach (var friend in _goodfriends)
            {
                Console.WriteLine(friend);
            }

            Console.WriteLine($"NrOfFriends: {_goodfriends.Count()}");
            Console.WriteLine($"NrOfFriends without any pets: {_goodfriends.Count(f => f.Pets == null)}");
            Console.WriteLine($"NrOfFriends without an adress: {_goodfriends.Count(f => f.Adress == null)}");
        }

        private static DbContextOptionsBuilder<DbContextLib.MainDbContext> CreateDbContextOptions(DbItem db)
        {
            //Ensures appsettings.json is in the right location and DbContext created
            Console.WriteLine($"AppSettings Directory: {AppConfig.AppSettingsDirectory}");

            var connectionString = db.DbConnection;
            if (!string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine($"Database type: {db.DbType}");
                Console.WriteLine($"Connection used: {connectionString}");

                return db.DbContext;
            }

            return null;
        }


        private static void SeedDataBase()
        {
            using (var db = new MainDbContext(_optionsBuilder.Options))
            {
                //Create a list of friends
                var _goodfriends = new List<csFriend>();
                for (int c = 0; c < 20; c++)
                {
                    _goodfriends.Add(csFriend.Factory.CreateRandom());
                }

                foreach (var friend in _goodfriends)
                {
                    db.Friends.Add(friend);
                }
                db.SaveChanges();
            }
        }

        private static async Task QueryDatabaseAsync()
        {
            Console.WriteLine("--------------");
            using (var db = new MainDbContext(_optionsBuilder.Options))
            {
                var friends = await db.Friends.CountAsync();
                var friendsNoPet = await db.Friends.CountAsync(f => f.Pets == null);
                var friendsNoAdress = await db.Friends.CountAsync(f => f.Adress == null);

                Console.WriteLine($"NrOfFriends: {friends}");
                Console.WriteLine($"NrOfFriends without any pets: {friendsNoPet}");
                Console.WriteLine($"NrOfFriends without an adress: {friendsNoAdress}");
            }
        }
    }
}
