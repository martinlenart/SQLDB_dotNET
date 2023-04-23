using System;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace DbContextLib
{
    public class DbItem
    {
        public string DbType { get; set; }
        public string DbConnection { get; set; }
        public string DbConnectionString => AppConfig.ConfigurationRoot.GetConnectionString(DbConnection);

        public DbContextOptionsBuilder<DbContextLib.MainDbContext> DbContext
        {
            get
            {
                var _optionsBuilder = new DbContextOptionsBuilder<DbContextLib.MainDbContext>();

                if (DbType == "SQLServer")
                {
                    _optionsBuilder.UseSqlServer(DbConnectionString);
                    return _optionsBuilder;
                }
                else if (DbType == "MariaDb")
                {
                    _optionsBuilder.UseMySql(DbConnectionString, ServerVersion.AutoDetect(DbConnectionString));
                    return _optionsBuilder;
                }
                else if (DbType == "Postgres")
                {
                    _optionsBuilder.UseNpgsql(DbConnectionString);
                    return _optionsBuilder;
                }
                else if (DbType == "SQLite")
                {
                    _optionsBuilder.UseSqlite(DbConnectionString);
                    return _optionsBuilder;
                }

                //unknown database type
                throw new InvalidDataException($"Database type {DbType} does not exist");
            }
        }

    }

    public sealed class AppConfig
    {
#if DEBUG
        public const string Appsettingfile = "appsettings.Development.json";
#else
        public const string Appsettingfile = "appsettings.json";
#endif

        private static readonly object instanceLock = new();

        private static AppConfig _instance = null;
        private static IConfigurationRoot _configuration = null ;

        private static List<DbItem> _dbMigrations = new List<DbItem>();

        private AppConfig()
        {
            //Lets get the credentials access Azure KV and set them as Environment variables
            //During Development this will come from User Secrets,
            //After Deployment it will come from appsettings.json

            var builder = new ConfigurationBuilder()
                                .SetBasePath(AppSettingsDirectory)
                                .AddJsonFile(Appsettingfile, optional: true, reloadOnChange: true);

            _configuration = builder.Build();
            _configuration.Bind("DbMigrations", _dbMigrations);  //Need the NuGet package Microsoft.Extensions.Configuration.Binder
        }

        public static string AppSettingsDirectory
        {
            get
            {
                //Normally LocalApplicationData is a good place to store configuration files.
                //Copy appsettings.json to the folder in documentPath
                var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                documentPath = Path.Combine(documentPath, "SQL Databases", "GoodFriends");
                if (!Directory.Exists(documentPath)) Directory.CreateDirectory(documentPath);

                return documentPath;
            }
        }

        public static bool AppSettingsExist => File.Exists(Path.Combine(AppSettingsDirectory, Appsettingfile));

        public static IConfigurationRoot ConfigurationRoot
        {
            get
            {
                lock (instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new AppConfig();
                    }
                    return _configuration;
                }
            }
        }

        public static List<DbItem> DbMigrations
        {
            get
            {
                lock (instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new AppConfig();
                    }
                    return _dbMigrations;
                }
            }
        }
    }
}
