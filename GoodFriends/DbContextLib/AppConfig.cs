#define IsConsoleApp

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
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
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
        private static IConfigurationRoot _configuration;

        private static List<DbItem> _dbMigrations = new List<DbItem>();

        private AppConfig()
        {
#if DEBUG
            //Lets get the credentials access Azure KV and set them as Environment variables
            //During Development this will come from User Secrets,
            //After Deployment it will come from appsettings.json

            var _azureConf = new ConfigurationBuilder()
                                .SetBasePath(AppSettingsDirectory)
                                .AddJsonFile(Appsettingfile, optional: true, reloadOnChange: true)
                                .AddUserSecrets("3d2b8454-7957-4457-9167-d64aaaedb8d3")
                                .Build();

            // Access the Azure KeyVault
            Environment.SetEnvironmentVariable("AZURE_KeyVaultUri", _azureConf["AZURE_KeyVaultUri"]);
            Environment.SetEnvironmentVariable("AZURE_CLIENT_SECRET", _azureConf["AZURE_CLIENT_SECRET"]);
            Environment.SetEnvironmentVariable("AZURE_CLIENT_ID", _azureConf["AZURE_CLIENT_ID"]);
            Environment.SetEnvironmentVariable("AZURE_TENANT_ID", _azureConf["AZURE_TENANT_ID"]);
#endif
            var _kvuri = Environment.GetEnvironmentVariable("AZURE_KeyVaultUri");
            //Get the user-secrets from Azure Key Vault

            var client = new SecretClient(new Uri(_kvuri), new DefaultAzureCredential(
                new DefaultAzureCredentialOptions { AdditionallyAllowedTenants = { "*" } }));

            //Get user-secrets from AKV and flatten it into a Dictionary<string, string>
            var secret = client.GetSecret("user-secrets1");
            var message = secret.Value.Value;
            var userSecretsAKV = JsonFlatToDictionary(message);

            //Create final ConfigurationRoot which includes also AzureKeyVault
            var builder = new ConfigurationBuilder()

                                .SetBasePath(AppSettingsDirectory)
                                .AddJsonFile(Appsettingfile, optional: true, reloadOnChange: true)
#if DEBUG
                                //Shared on one developer machine
                                .AddUserSecrets("3d2b8454-7957-4457-9167-d64aaaedb8d3");
#endif
            //super secrets managed by Azure Key Vault
            //.AddInMemoryCollection(userSecretsAKV);

            _configuration = builder.Build();
            _configuration.Bind("DbMigrations", _dbMigrations);  //Need the NuGet package Microsoft.Extensions.Configuration.Binder
        }

        public static string AppSettingsDirectory
        {
            get
            {
#if IsConsoleApp

                //Normally LocalApplicationData is a good place to store configuration files.
                //Copy appsettings.json to the folder in documentPath
                var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                documentPath = Path.Combine(documentPath, "SQL Databases", "GoodFriends");
                if (!Directory.Exists(documentPath)) Directory.CreateDirectory(documentPath);
#else
                //In an ASP.NET Core app the appsettings.json is in the root directory of the App itself
                var documentPath = Directory.GetCurrentDirectory();
#endif
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

        private static Dictionary<string, string> JsonFlatToDictionary(string json)
        {
            IEnumerable<(string Path, JsonProperty P)> GetLeaves(string path, JsonProperty p)
                => p.Value.ValueKind != JsonValueKind.Object
                    ? new[] { (Path: path == null ? p.Name : path + ":" + p.Name, p) }
                    : p.Value.EnumerateObject().SelectMany(child => GetLeaves(path == null ? p.Name : path + ":" + p.Name, child));

            using (JsonDocument document = JsonDocument.Parse(json)) // Optional JsonDocumentOptions options
                return document.RootElement.EnumerateObject()
                    .SelectMany(p => GetLeaves(null, p))
                    .ToDictionary(k => k.Path, v => v.P.Value.Clone().ToString()); //Clone so that we can use the values outside of using
        }

        public static string SecretMessage => ConfigurationRoot["SecretMessage"];

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
