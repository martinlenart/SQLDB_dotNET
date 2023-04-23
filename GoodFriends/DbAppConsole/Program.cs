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
            #region run below to test the model only

            Console.WriteLine($"\nTesting Model...");
            TestModel();
            #endregion

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


        private static void TestModel()
        {
            var _modelList = SeedModel();
            WriteModel(_modelList);
        }

        #region Replaced by new model methods
        private static void WriteModel(List<csFriend> _modelList)
        {
            foreach (var friend in _modelList)
            {
                Console.WriteLine(friend);
            }

            Console.WriteLine($"NrOfFriends: {_modelList.Count()}");
            Console.WriteLine($"NrOfFriends without any pets: {_modelList.Count(f => f.Pets == null)}");
            Console.WriteLine($"NrOfFriends without an adress: {_modelList.Count(f => f.Adress == null)}");
        }

        private static List<csFriend> SeedModel()
        {
            //Create a list of friends
            var _goodfriends = new List<csFriend>();
            for (int c = 0; c < 20; c++)
            {
                _goodfriends.Add(csFriend.Factory.CreateRandom());
            }
            return _goodfriends;
        }
        #endregion

        private static void SeedDataBase()
        {
            using (var db = new MainDbContext(_optionsBuilder.Options))
            {
                var _modelList = SeedModel();

                #region Seeding the database using EFC
                foreach (var _friend in _modelList)
                {
                    db.Friends.Add(_friend);
                }
                #endregion

                db.SaveChanges();
            }
        }

        private static async Task QueryDatabaseAsync()
        {
            Console.WriteLine("--------------");
            using (var db = new MainDbContext(_optionsBuilder.Options))
            {
                #region Reading the database using EFC
                var _modelList = await db.Friends.ToListAsync();
                var _pets = await db.Pets.ToListAsync();               //Needed if I want EFC to load the embedded List
                var _adresses = await db.Adress.ToListAsync();           //Needed if I want EFC to load the embedded List

                #endregion

                WriteModel(_modelList);
            }
        }
    }
}
