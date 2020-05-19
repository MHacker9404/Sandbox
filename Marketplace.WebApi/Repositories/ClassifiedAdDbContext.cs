using Marketplace.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Marketplace.WebApi.Repositories
{
    public class ClassifiedAdDbContext : DbContext
    {
        private readonly ILoggerFactory _logger;
        private readonly DbContextOptions<ClassifiedAdDbContext> _options;

        public ClassifiedAdDbContext(DbContextOptions<ClassifiedAdDbContext> options, ILoggerFactory logger) : base(options)
        {
            _options = options;
            _logger = logger;
        }

        public DbSet<ClassifiedAd> ClassifiedAds { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_logger);
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClassifiedAdIdEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PictureEntityTypeConfiguration());
        }
    }

    public class PictureEntityTypeConfiguration : IEntityTypeConfiguration<Picture>
    {
        public void Configure(EntityTypeBuilder<Picture> builder)
        {
            builder.HasKey(p => p.PicId);
            builder.OwnsOne(p => p.Id);
            builder.OwnsOne(p => p.ParentId);
            builder.OwnsOne(p => p.Size);
        }
    }

    public class ClassifiedAdIdEntityTypeConfiguration : IEntityTypeConfiguration<ClassifiedAd>
    {
        public void Configure(EntityTypeBuilder<ClassifiedAd> builder)
        {
            builder.HasKey(ad => ad.AdId);
            builder.OwnsOne(x => x.Id);
            builder.OwnsOne(x => x.Price, p => p.OwnsOne(c => c.Currency));
            builder.OwnsOne(x => x.Text);
            builder.OwnsOne(x => x.Title);
            builder.OwnsOne( x => x.ApprovedBy );
            builder.OwnsOne( x => x.OwnerId );
        }
    }

    public static class AppBuilderDatabaseExtensions
    {
        public static void EnsureDatabase(this IApplicationBuilder app)
        {
            var context = app.ApplicationServices.GetService<ClassifiedAdDbContext>();
            if (!context.Database.EnsureCreated()) context.Database.Migrate();
        }
    }
}