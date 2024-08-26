using Microsoft.EntityFrameworkCore;
using VenueBookingSystem.Models;

namespace VenueBookingSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Group> Groups { get; set; }  // 添加 Group 实体
        public DbSet<GroupUser> GroupUsers { get; set; }  // 添加 GroupUser 实体

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 配置 Group 和 User 之间的多对多关系
            modelBuilder.Entity<GroupUser>()
                .HasKey(gu => new { gu.UserId, gu.GroupId });

            modelBuilder.Entity<GroupUser>()
                .HasOne(gu => gu.User)
                .WithMany(u => u.GroupUsers)
                .HasForeignKey(gu => gu.UserId);

            modelBuilder.Entity<GroupUser>()
                .HasOne(gu => gu.Group)
                .WithMany(g => g.GroupUsers)
                .HasForeignKey(gu => gu.GroupId);

            // 这里可以添加其他实体的配置，例如关系、约束等
        }
    }
}
