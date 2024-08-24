using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MuratBaloglu.Domain.Entities;
using MuratBaloglu.Domain.Entities.Common;
using MuratBaloglu.Domain.Entities.Identity;

namespace MuratBaloglu.Persistence.Contexts
{
    public class MuratBalogluDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public MuratBalogluDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Domain.Entities.File> Files { get; set; }
        public DbSet<BlogImageFile> BlogImageFiles { get; set; }
        public DbSet<SpecialityImageFile> SpecialityImageFiles { get; set; }
        public DbSet<CarouselImageFile> CarouselImageFiles { get; set; }
        public DbSet<AboutMeImageFile> AboutMeImageFiles { get; set; }
        public DbSet<NewsImageFile> NewsImageFiles { get; set; }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Speciality> Specialties { get; set; }
        public DbSet<SpecialityCategory> SpecialityCategories { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<SocialMediaAccount> SocialMediaAccounts { get; set; }
        public DbSet<WorkingHour> WorkingHours { get; set; }
        public DbSet<PatientComment> PatientComments { get; set; }
        public DbSet<AboutMe> AboutMe { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Endpoint> Endpoints { get; set; }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var datas = ChangeTracker.Entries<BaseEntity>();

            foreach (var trackedEntity in datas)
            {
                _ = trackedEntity.State switch
                {
                    EntityState.Added => trackedEntity.Entity.CreatedDate = DateTime.Now,
                    EntityState.Modified => trackedEntity.Entity.UpdatedDate = DateTime.Now,
                    _ => DateTime.Now
                };
            }

            return await base.SaveChangesAsync(cancellationToken);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BlogImageFile>()
                .HasOne(bif => bif.Blog)
                .WithMany(b => b.BlogImageFiles)
                .HasForeignKey(bif => bif.BlogId);

            modelBuilder.Entity<SpecialityImageFile>()
                .HasOne(sif => sif.Speciality)
                .WithMany(s => s.SpecialityImageFiles)
                .HasForeignKey(sif => sif.SpecialityId);

            modelBuilder.Entity<Speciality>()
               .HasOne(s => s.Category)
               .WithMany(sc => sc.Specialties)
               .HasForeignKey(s => s.CategoryId);

            modelBuilder.Entity<AboutMe>()
                .HasOne(a => a.AboutMeImageFile)
                .WithOne(aif => aif.AboutMe)
                .HasForeignKey<AboutMeImageFile>(aif => aif.AboutMeId);

            modelBuilder.Entity<News>()
                .HasOne(n => n.NewsImageFile)
                .WithOne(nif => nif.News)
                .HasForeignKey<NewsImageFile>(nif => nif.NewsId);

            modelBuilder.Entity<Endpoint>()
                .HasOne(e => e.Menu)
                .WithMany(m => m.Endpoints)
                .HasForeignKey(e => e.MenuId);
        }

    }
}
