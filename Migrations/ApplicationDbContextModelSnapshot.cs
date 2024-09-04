﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Oracle.EntityFrameworkCore.Metadata;
using VenueBookingSystem.Data;

#nullable disable

namespace sports_management.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            OracleModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("VenueBookingSystem.Models.Admin", b =>
                {
                    b.Property<string>("AdminId")
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<string>("AdminType")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("ContactNumber")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("RealName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("AdminId");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.AdminNotification", b =>
                {
                    b.Property<string>("NotificationId")
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<string>("AdminId")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("NewAdminId")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<DateTime>("NotificationTime")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<string>("NotificationType")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("NotificationId");

                    b.HasIndex("AdminId");

                    b.ToTable("AdminNotifications");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.Announcement", b =>
                {
                    b.Property<string>("AnnouncementId")
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<string>("AdminId")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<DateTime>("PublishedDate")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("AnnouncementId");

                    b.HasIndex("AdminId");

                    b.ToTable("Announcements");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.Equipment", b =>
                {
                    b.Property<string>("EquipmentId")
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<string>("AdminId")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<string>("EquipmentName")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("EquipmentId");

                    b.HasIndex("AdminId");

                    b.ToTable("Equipments");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.Group", b =>
                {
                    b.Property<string>("GroupId")
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<string>("Description")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<int>("MemberCount")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("GroupId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.GroupReservationMember", b =>
                {
                    b.Property<string>("ReservationId")
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<string>("GroupId")
                        .HasColumnType("NVARCHAR2(450)");

                    b.HasKey("ReservationId", "GroupId");

                    b.HasIndex("GroupId");

                    b.ToTable("GroupReservationMember");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.GroupUser", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<string>("GroupId")
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<DateTime>("JoinDate")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<string>("RoleInGroup")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("UserId", "GroupId");

                    b.HasIndex("GroupId");

                    b.ToTable("GroupUsers");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.MaintenanceRecord", b =>
                {
                    b.Property<string>("MaintenanceRecordId")
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<string>("EquipmentId")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<string>("MaintenanceDetails")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<DateTime>("MaintenanceEndTime")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<DateTime>("MaintenanceStartTime")
                        .HasColumnType("TIMESTAMP(7)");

                    b.HasKey("MaintenanceRecordId");

                    b.HasIndex("EquipmentId");

                    b.ToTable("MaintenanceRecords");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.Reservation", b =>
                {
                    b.Property<string>("ReservationId")
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<string>("AvailabilityId")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<decimal>("PaymentAmount")
                        .HasColumnType("DECIMAL(18, 2)");

                    b.Property<string>("ReservationItem")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<DateTime>("ReservationTime")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<string>("ReservationType")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("UserId")
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<string>("VenueId")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(450)");

                    b.HasKey("ReservationId");

                    b.HasIndex("AvailabilityId");

                    b.HasIndex("UserId");

                    b.HasIndex("VenueId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<string>("ContactNumber")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<double>("DiscountRate")
                        .HasColumnType("BINARY_DOUBLE");

                    b.Property<string>("IsVip")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("RealName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<string>("ReservationPermission")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<int>("ViolationCount")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.UserNotification", b =>
                {
                    b.Property<string>("NotificationId")
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<DateTime>("NotificationTime")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<string>("NotificationType")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("TargetTeam")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("TargetUser")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(450)");

                    b.HasKey("NotificationId");

                    b.HasIndex("UserId");

                    b.ToTable("UserNotifications");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.UserReservation", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<string>("ReservationId")
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<DateTime?>("CheckInTime")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<int>("NumOfPeople")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("UserId", "ReservationId");

                    b.HasIndex("ReservationId");

                    b.ToTable("UserReservations");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.Venue", b =>
                {
                    b.Property<string>("VenueId")
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<int>("Capacity")
                        .HasColumnType("NUMBER(10)");

                    b.Property<DateTime>("LastInspectionTime")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<int>("MaintenanceCount")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("VenueDescription")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("VenueImageUrl")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("VenueId");

                    b.ToTable("Venues");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.VenueAnnouncement", b =>
                {
                    b.Property<string>("VenueId")
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<string>("AnnouncementId")
                        .HasColumnType("NVARCHAR2(450)");

                    b.HasKey("VenueId", "AnnouncementId");

                    b.HasIndex("AnnouncementId");

                    b.ToTable("VenueAnnouncements");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.VenueAvailability", b =>
                {
                    b.Property<string>("AvailabilityId")
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<decimal>("Price")
                        .HasColumnType("DECIMAL(18, 2)");

                    b.Property<int>("RemainingCapacity")
                        .HasColumnType("NUMBER(10)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<string>("VenueId")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(450)");

                    b.HasKey("AvailabilityId");

                    b.HasIndex("VenueId");

                    b.ToTable("VenueAvailabilities");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.VenueEquipment", b =>
                {
                    b.Property<string>("EquipmentId")
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<string>("VenueId")
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<DateTime>("InstallationTime")
                        .HasColumnType("TIMESTAMP(7)");

                    b.HasKey("EquipmentId", "VenueId");

                    b.HasIndex("VenueId");

                    b.ToTable("VenueEquipments");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.VenueMaintenance", b =>
                {
                    b.Property<string>("VenueMaintenanceId")
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<DateTime>("MaintenanceEndDate")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<DateTime>("MaintenanceStartDate")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<string>("VenueId")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(450)");

                    b.HasKey("VenueMaintenanceId");

                    b.HasIndex("VenueId");

                    b.ToTable("VenueMaintenance");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.VenueManagement", b =>
                {
                    b.Property<string>("VenueId")
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<string>("AdminId")
                        .HasColumnType("NVARCHAR2(450)");

                    b.HasKey("VenueId", "AdminId");

                    b.HasIndex("AdminId");

                    b.ToTable("VenueManagements");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.AdminNotification", b =>
                {
                    b.HasOne("VenueBookingSystem.Models.Admin", "Admin")
                        .WithMany("AdminNotifications")
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admin");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.Announcement", b =>
                {
                    b.HasOne("VenueBookingSystem.Models.Admin", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admin");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.Equipment", b =>
                {
                    b.HasOne("VenueBookingSystem.Models.Admin", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admin");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.GroupReservationMember", b =>
                {
                    b.HasOne("VenueBookingSystem.Models.Group", "Group")
                        .WithMany("GroupReservations")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VenueBookingSystem.Models.Reservation", "Reservation")
                        .WithMany("GroupReservationMembers")
                        .HasForeignKey("ReservationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Reservation");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.GroupUser", b =>
                {
                    b.HasOne("VenueBookingSystem.Models.Group", "Group")
                        .WithMany("GroupUsers")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VenueBookingSystem.Models.User", "User")
                        .WithMany("GroupUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.MaintenanceRecord", b =>
                {
                    b.HasOne("VenueBookingSystem.Models.Equipment", "Equipment")
                        .WithMany()
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipment");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.Reservation", b =>
                {
                    b.HasOne("VenueBookingSystem.Models.VenueAvailability", "Availability")
                        .WithMany("Reservations")
                        .HasForeignKey("AvailabilityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VenueBookingSystem.Models.User", null)
                        .WithMany("Reservations")
                        .HasForeignKey("UserId");

                    b.HasOne("VenueBookingSystem.Models.Venue", "Venue")
                        .WithMany("Reservations")
                        .HasForeignKey("VenueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Availability");

                    b.Navigation("Venue");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.UserNotification", b =>
                {
                    b.HasOne("VenueBookingSystem.Models.User", "User")
                        .WithMany("UserNotifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.UserReservation", b =>
                {
                    b.HasOne("VenueBookingSystem.Models.Reservation", "Reservation")
                        .WithMany("UserReservations")
                        .HasForeignKey("ReservationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VenueBookingSystem.Models.User", "User")
                        .WithMany("UserReservations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reservation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.VenueAnnouncement", b =>
                {
                    b.HasOne("VenueBookingSystem.Models.Announcement", "Announcement")
                        .WithMany("VenueAnnouncements")
                        .HasForeignKey("AnnouncementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VenueBookingSystem.Models.Venue", "Venue")
                        .WithMany("VenueAnnouncements")
                        .HasForeignKey("VenueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Announcement");

                    b.Navigation("Venue");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.VenueAvailability", b =>
                {
                    b.HasOne("VenueBookingSystem.Models.Venue", "Venue")
                        .WithMany("VenueAvailabilities")
                        .HasForeignKey("VenueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Venue");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.VenueEquipment", b =>
                {
                    b.HasOne("VenueBookingSystem.Models.Equipment", "Equipment")
                        .WithMany()
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VenueBookingSystem.Models.Venue", "Venue")
                        .WithMany()
                        .HasForeignKey("VenueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipment");

                    b.Navigation("Venue");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.VenueMaintenance", b =>
                {
                    b.HasOne("VenueBookingSystem.Models.Venue", "Venue")
                        .WithMany("VenueMaintenances")
                        .HasForeignKey("VenueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Venue");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.VenueManagement", b =>
                {
                    b.HasOne("VenueBookingSystem.Models.Admin", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VenueBookingSystem.Models.Venue", "Venue")
                        .WithMany()
                        .HasForeignKey("VenueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admin");

                    b.Navigation("Venue");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.Admin", b =>
                {
                    b.Navigation("AdminNotifications");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.Announcement", b =>
                {
                    b.Navigation("VenueAnnouncements");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.Group", b =>
                {
                    b.Navigation("GroupReservations");

                    b.Navigation("GroupUsers");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.Reservation", b =>
                {
                    b.Navigation("GroupReservationMembers");

                    b.Navigation("UserReservations");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.User", b =>
                {
                    b.Navigation("GroupUsers");

                    b.Navigation("Reservations");

                    b.Navigation("UserNotifications");

                    b.Navigation("UserReservations");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.Venue", b =>
                {
                    b.Navigation("Reservations");

                    b.Navigation("VenueAnnouncements");

                    b.Navigation("VenueAvailabilities");

                    b.Navigation("VenueMaintenances");
                });

            modelBuilder.Entity("VenueBookingSystem.Models.VenueAvailability", b =>
                {
                    b.Navigation("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}