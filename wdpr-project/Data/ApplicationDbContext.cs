using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using wdpr_project.Models;

namespace wdpr_project.Data
{
     public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<Expert> Experts { get; set; }
        public DbSet<Research> Researches { get; set; }
        public DbSet<PersonalData> PersonalData { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Disability> Disabilities { get; set; }
        public DbSet<DisabilityAid> DisabilityAids { get; set; }   
        public DbSet<ResearchCriterium> ResearchCriteria { get; set; }      
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserChat> UserChats { get; set; }
        public DbSet<ResearchExpert> ResearchExperts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Admin>().ToTable("Admins");
            modelBuilder.Entity<Business>().ToTable("Businesses");
            modelBuilder.Entity<Expert>().ToTable("Experts");

            modelBuilder.Entity<Admin>().HasBaseType<User>();
            modelBuilder.Entity<Business>().HasBaseType<User>();
            modelBuilder.Entity<Expert>().HasBaseType<User>();

            modelBuilder.Entity<UserChat>()
                .HasKey(uc => new { uc.UserId, uc.ChatId });

            modelBuilder.Entity<UserChat>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserChats)
                .HasForeignKey(uc => uc.UserId);

            modelBuilder.Entity<UserChat>()
                .HasOne(uc => uc.Chat)
                .WithMany(c => c.UserChats)
                .HasForeignKey(uc => uc.ChatId);
      
             base.OnModelCreating(modelBuilder);

            // Specify primary key for IdentityUserLogin<string>
            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            });
        }

    }
}
