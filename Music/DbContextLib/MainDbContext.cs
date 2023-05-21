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

        
        #region model the Views
        public DbSet<dtoMusicGroup> vwMusicGroups { get; set; }
        public DbSet<dtoArtist> vwArtists { get; set; }
        public DbSet<dtoAlbum> vwAlbums { get; set; }
        #endregion
        
        public MainDbContext() { }
        public MainDbContext(DbContextOptions options) : base(options)
        { }

        
        #region model the Views
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<dtoMusicGroup>().ToView("vwMusicGroups","usr");
            modelBuilder.Entity<dtoArtist>().ToView("vwArtists", "usr");
            modelBuilder.Entity<dtoAlbum>().ToView("vwAlbums", "usr");
            base.OnModelCreating(modelBuilder);
        }
        #endregion
        
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
