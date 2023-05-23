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
using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data;

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

            //Console.WriteLine($"\nTesting Model...");
            //TestModel();
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
                    //SeedDataBase();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError: Database could not be seeded. Ensure the {db.DbType} database is correctly created");
                    return;
                }

                Console.WriteLine("\nAccess Tables...");
                AccessTablesAsync().Wait();

                Console.WriteLine("\nAccess StoredProcedure...");
                AccessStoredProcedureAsync().Wait();

                Console.WriteLine("\nAccess Views...");
                AccessViewsAsync().Wait();

                Console.WriteLine("\nSQL Injection...");
                SQLInjectionAsync().Wait();
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

        private static void WriteModel(List<csMusicGroup> _greatMusicBands)
        {
            foreach (var band in _greatMusicBands)
            {
                Console.WriteLine(band);
            }

            Console.WriteLine($"Nr of great music bands: {_greatMusicBands.Count()}");
            Console.WriteLine($"Total nr of albums: {_greatMusicBands.Sum(b => b.Albums.Count)}");
            Console.WriteLine($"Total nr of music band members: {_greatMusicBands.Sum(b => b.Members.Count)}");
        }

        private static List<csMusicGroup> SeedModel()
        {
            //Create a list of 200 great bands
            var _greatMusicBands = new List<csMusicGroup>();
            for (int c = 0; c < 200; c++)
            {
                _greatMusicBands.Add(csMusicGroup.Factory.CreateRandom());
            }

            return _greatMusicBands;
        }
        #endregion

        #region Updated for new model
        private static void SeedDataBase()
        {
            using (var db = new MainDbContext(_optionsBuilder.Options))
            {
                var _modelList = SeedModel();

                #region Seeding the database using EFC
                foreach (var _musicgroup in _modelList)
                {
                    db.MusicGroups.Add(_musicgroup);
                }
                #endregion

                db.SaveChanges();
            }
        }

        private static async Task AccessTablesAsync()
        {
            try
            {
                Console.WriteLine("--------------");
                using (var db = new MainDbContext(_optionsBuilder.Options))
                {
                    #region Reading the database using EFC
                    var _modelList = await db.MusicGroups.ToListAsync();
                    var _artists = await db.Artists.ToListAsync();           //Needed if I want EFC to load the embedded List
                    var _albums = await db.Albums.ToListAsync();             //Needed if I want EFC to load the embedded List
                    #endregion

                    WriteModel(_modelList);
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion


        private static async Task AccessViewsAsync()
        {
            try
            {
                Console.WriteLine("--------------");
                using (var db = new MainDbContext(_optionsBuilder.Options))
                {

                    var _vwMusicGroups = await db.vwMusicGroups.ToListAsync();
                    var _vwArtists = await db.vwArtists.ToListAsync();
                    var _vwAlbums = await db.vwAlbums.ToListAsync();

                    Console.WriteLine($"Nr of great music bands: {_vwMusicGroups.Count}");
                    Console.WriteLine($"Total nr of albums: {_vwAlbums.Count}");
                    Console.WriteLine($"Total nr of music band members: {_vwArtists.Count}");
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private static async Task AccessStoredProcedureAsync()
        {
            Console.WriteLine("--------------");
            try
            {
                using (var db = new MainDbContext(_optionsBuilder.Options))
                using (var cmd1 = db.Database.GetDbConnection().CreateCommand())
                using (var cmd2 = db.Database.GetDbConnection().CreateCommand())
                using (var cmd3 = db.Database.GetDbConnection().CreateCommand())
                using (var cmd4 = db.Database.GetDbConnection().CreateCommand())
                {
                    cmd1.CommandType = cmd2.CommandType = cmd3.CommandType = cmd4.CommandType = CommandType.StoredProcedure;

                    //MusicGroup
                    cmd1.CommandText = "usr.usp_InsertMusicGroup";
                    cmd1.Parameters.Add(new SqlParameter("Name", "The Doe Family"));
                    cmd1.Parameters.Add(new SqlParameter("EstablishedYear", 2023));
                    int i = cmd1.Parameters.Add(new SqlParameter("InsertedMusicGroupId", SqlDbType.UniqueIdentifier) { Direction = ParameterDirection.Output });

                    db.Database.OpenConnection();
                    await cmd1.ExecuteScalarAsync();
                    Guid musicGroupId = (Guid) cmd1.Parameters[i].Value;
                    Console.WriteLine($"Inserted MusicGroup {musicGroupId}");


                    //Artis1
                    cmd2.CommandText = cmd3.CommandText = cmd4.CommandText = "usr.usp_InsertArtist";
                    cmd2.Parameters.Add(new SqlParameter("FirstName", "John"));
                    cmd2.Parameters.Add(new SqlParameter("LastName", "Doe"));
                    cmd2.Parameters.Add(new SqlParameter("MusicGroupId", musicGroupId));
                    i = cmd2.Parameters.Add(new SqlParameter("InsertedArtistId", SqlDbType.UniqueIdentifier) { Direction = ParameterDirection.Output });

                    await cmd2.ExecuteScalarAsync();
                    Guid artist1 = (Guid)cmd2.Parameters[i].Value;
                    Console.WriteLine($"Inserted Artist {artist1}");

                    //Artist2
                    cmd3.Parameters.Add(new SqlParameter("FirstName", "Mary"));
                    cmd3.Parameters.Add(new SqlParameter("LastName", "Doe"));
                    cmd3.Parameters.Add(new SqlParameter("MusicGroupId", musicGroupId));
                    i = cmd3.Parameters.Add(new SqlParameter("InsertedArtistId", SqlDbType.UniqueIdentifier) { Direction = ParameterDirection.Output });

                    await cmd3.ExecuteScalarAsync();
                    Guid artist2 = (Guid)cmd3.Parameters[i].Value;
                    Console.WriteLine($"Inserted Artist {artist2}");

                    //Artist3
                    cmd4.Parameters.Add(new SqlParameter("FirstName", "Kim"));
                    cmd4.Parameters.Add(new SqlParameter("LastName", "Doe"));
                    cmd4.Parameters.Add(new SqlParameter("MusicGroupId", musicGroupId));
                    i = cmd4.Parameters.Add(new SqlParameter("InsertedArtistId", SqlDbType.UniqueIdentifier) { Direction = ParameterDirection.Output });

                    await cmd4.ExecuteScalarAsync();
                    Guid artist3 = (Guid)cmd4.Parameters[i].Value;
                    Console.WriteLine($"Inserted Artist {artist3}");

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static async Task SQLInjectionAsync()
        {
            Console.WriteLine("--------------");
            try
            {
                using (var db = new MainDbContext(_optionsBuilder.Options))
                {

                    db.Database.OpenConnection();
                    await db.Database.ExecuteSqlAsync($"UPDATE dbo.Artists SET LastName = 'Valdemart' WHERE LastName = 'Voldemort';");
                    //await db.Database.ExecuteSqlAsync($"UPDATE dbo.Artists SET LastName = 'Voldemort' WHERE LastName = 'Valdemart';");
                    Console.WriteLine("Injection attack complete...");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
