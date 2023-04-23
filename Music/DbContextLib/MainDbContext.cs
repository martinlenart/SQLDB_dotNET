using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using DbModelsLib;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DbContextLib
{
    public class MainDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<csMusicGroup> MusicGroups { get; set; }
        public DbSet<csArtist> Artists { get; set; }
        public DbSet<csAlbum> Albums { get; set; }

        public MainDbContext() { }
        public MainDbContext(DbContextOptions options) : base(options)
        { }
    }


    public class SqlServerDbContext : MainDbContext
    {
        public SqlServerDbContext() { }
        public SqlServerDbContext(DbContextOptions options) : base(options)
        { }


        //Used only for CodeFirst Database Migration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = AppConfig.DbMigrations.Find(i => i.DbType == "SQLServer").DbConnectionString;
                optionsBuilder.UseSqlServer(connectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HaveColumnType("money");
            configurationBuilder.Properties<string>().HaveColumnType("nvarchar(200)");

            base.ConfigureConventions(configurationBuilder);
        }
    }

    public class MySqlDbContext : MainDbContext
    {
        public MySqlDbContext() { }
        public MySqlDbContext(DbContextOptions options) : base(options)
        { }


        //Used only for CodeFirst Database Migration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = AppConfig.DbMigrations.Find(i => i.DbType == "MariaDb").DbConnectionString;
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>().HaveColumnType("varchar(200)");

            base.ConfigureConventions(configurationBuilder);

        }
    }

    public class PostgresDbContext : MainDbContext
    {
        public PostgresDbContext() { }
        public PostgresDbContext(DbContextOptions options) : base(options)
        { }


        //Used only for CodeFirst Database Migration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = AppConfig.DbMigrations.Find(i => i.DbType == "Postgres").DbConnectionString;
                optionsBuilder.UseNpgsql(connectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>().HaveColumnType("varchar(200)");
            base.ConfigureConventions(configurationBuilder);
        }
    }

    public class SqliteDbContext : MainDbContext
    {
        public SqliteDbContext() { }
        public SqliteDbContext(DbContextOptions options) : base(options)
        { }


        //Used only for CodeFirst Database Migration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = AppConfig.DbMigrations.Find(i => i.DbType == "SQLite").DbConnectionString;
                optionsBuilder.UseSqlite(connectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }
    }
}
