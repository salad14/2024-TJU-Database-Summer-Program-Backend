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

             // 配置 Venue 和 Announcement 之间的多对多关系
            modelBuilder.Entity<VenueAnnouncement>()
                .HasKey(va => new { va.VenueId, va.AnnouncementId });

            modelBuilder.Entity<VenueAnnouncement>()
                .HasOne(va => va.Venue)
                .WithMany(v => v.VenueAnnouncements)
                .HasForeignKey(va => va.VenueId);

            modelBuilder.Entity<VenueAnnouncement>()
                .HasOne(va => va.Announcement)
                .WithMany(a => a.VenueAnnouncements)
                .HasForeignKey(va => va.AnnouncementId);

            // 配置 VenueEvent 的复合主键
            modelBuilder.Entity<VenueEvent>()
                .HasKey(ve => new { ve.VenueId, ve.EventId });

            modelBuilder.Entity<VenueEvent>()
                .HasOne(ve => ve.Venue)
                .WithMany(v => v.VenueEvents)
                .HasForeignKey(ve => ve.VenueId);

            modelBuilder.Entity<VenueEvent>()
                .HasOne(ve => ve.Event)
                .WithMany(e => e.VenueEvents)
                .HasForeignKey(ve => ve.EventId);

            // 配置 User 和 Reservation 之间的多对多关系
            modelBuilder.Entity<UserReservation>()
                .HasKey(ur => new { ur.UserId, ur.ReservationId });

            modelBuilder.Entity<UserReservation>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserReservations) // 确保与 UserReservation 关联
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserReservation>()
                .HasOne(ur => ur.Reservation)
                .WithMany(r => r.UserReservations)
                .HasForeignKey(ur => ur.ReservationId);

            // 配置 Group 和 Reservation 之间的一对多关系
            modelBuilder.Entity<Group>()
                .HasMany(g => g.GroupReservations)
                .WithOne(gr => gr.Group)
                .HasForeignKey(gr => gr.GroupId);

            // 配置 Venue 和 Reservation 之间的一对多关系
            modelBuilder.Entity<Venue>()
                .HasMany(v => v.Reservations)
                .WithOne(r => r.Venue)
                .HasForeignKey(r => r.VenueId);

            // 配置 Venue 和 VenueAvailability 之间的一对多关系
            modelBuilder.Entity<Venue>()
                .HasMany(v => v.VenueAvailabilities)
                .WithOne(va => va.Venue)
                .HasForeignKey(va => va.VenueId);

            // 配置 GroupReservationMember 的复合主键
            modelBuilder.Entity<GroupReservationMember>()
                .HasKey(grm => new { grm.ReservationId, grm.GroupId });

            modelBuilder.Entity<GroupReservationMember>()
                .HasOne(grm => grm.Group)
                .WithMany(g => g.GroupReservations)
                .HasForeignKey(grm => grm.GroupId);

            modelBuilder.Entity<GroupReservationMember>()
                .HasOne(grm => grm.Reservation)
                .WithMany(r => r.GroupReservationMembers)
                .HasForeignKey(grm => grm.ReservationId);


            // 配置 VenueAvailability 的主键
            modelBuilder.Entity<VenueAvailability>()
                .HasKey(va => va.AvailabilityId); // 配置主键

            // 这里可以添加其他实体的配置，例如关系、约束等
        }
    }
}
