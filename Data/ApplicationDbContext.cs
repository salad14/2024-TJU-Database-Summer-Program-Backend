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
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Group> Groups { get; set; }  // 添加 Group 实体
        public DbSet<GroupUser> GroupUsers { get; set; }  // 添加 GroupUser 实体
        public DbSet<UserNotification> UserNotifications { get; set; }
        public DbSet<AdminNotification> AdminNotifications { get; set; }
        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<VenueEquipment> VenueEquipments { get; set; }
        public DbSet<VenueAvailability> VenueAvailabilities { get; set; }



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

            // 配置 Admin 的主键
            modelBuilder.Entity<Admin>()
                .HasKey(a => a.AdminId); // 定义主键

            // 配置 AdminNotification 的主键和外键
            modelBuilder.Entity<AdminNotification>()
                .HasKey(an => an.NotificationId); // 定义主键

            modelBuilder.Entity<AdminNotification>()
                .HasOne(an => an.Admin) // 定义外键关系
                .WithMany(a => a.AdminNotifications)
                .HasForeignKey(an => an.AdminId);

            // 配置 UserNotification 的主键和外键
            modelBuilder.Entity<UserNotification>()
                .HasKey(un => un.NotificationId); // 定义主键

            modelBuilder.Entity<UserNotification>()
                .HasOne(un => un.User) // 定义外键关系
                .WithMany(u => u.UserNotifications)
                .HasForeignKey(un => un.UserId);

             // 配置 VenueEquipment 的复合主键
             modelBuilder.Entity<VenueEquipment>()
             .HasKey(ve => new { ve.EquipmentId, ve.VenueId });

            // 这里可以添加其他实体的配置，例如关系、约束等
        }
    }
}
