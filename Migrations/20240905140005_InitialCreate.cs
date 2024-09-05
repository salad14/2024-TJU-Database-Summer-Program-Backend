using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sports_management.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    AdminId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    RealName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Password = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ContactNumber = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    AdminType = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.AdminId);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    GroupId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    GroupName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    MemberCount = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.GroupId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Username = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Password = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ContactNumber = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    UserType = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ReservationPermission = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ViolationCount = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IsVip = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DiscountRate = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    RealName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Venues",
                columns: table => new
                {
                    VenueId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Type = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Capacity = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Status = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    MaintenanceCount = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    LastInspectionTime = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    VenueImageUrl = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    VenueDescription = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    VenueLocation = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venues", x => x.VenueId);
                });

            migrationBuilder.CreateTable(
                name: "AdminNotifications",
                columns: table => new
                {
                    NotificationId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    AdminId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    NewAdminId = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NotificationType = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Title = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Content = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NotificationTime = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminNotifications", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_AdminNotifications_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "AdminId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    AnnouncementId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Title = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Content = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    PublishedDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    AdminId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.AnnouncementId);
                    table.ForeignKey(
                        name: "FK_Announcements_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "AdminId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Equipments",
                columns: table => new
                {
                    EquipmentId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    EquipmentName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    AdminId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.EquipmentId);
                    table.ForeignKey(
                        name: "FK_Equipments_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "AdminId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupUsers",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    GroupId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    JoinDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    RoleInGroup = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupUsers", x => new { x.UserId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_GroupUsers_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserNotifications",
                columns: table => new
                {
                    NotificationId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    UserId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    NotificationType = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Title = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Content = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    TargetUser = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TargetTeam = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    NotificationTime = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotifications", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_UserNotifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VenueAvailabilities",
                columns: table => new
                {
                    AvailabilityId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    VenueId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    EndTime = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingCapacity = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VenueAvailabilities", x => x.AvailabilityId);
                    table.ForeignKey(
                        name: "FK_VenueAvailabilities_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "VenueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VenueMaintenance",
                columns: table => new
                {
                    VenueMaintenanceId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    MaintenanceStartDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    MaintenanceEndDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    VenueId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VenueMaintenance", x => x.VenueMaintenanceId);
                    table.ForeignKey(
                        name: "FK_VenueMaintenance_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "VenueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VenueManagements",
                columns: table => new
                {
                    VenueId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    AdminId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VenueManagements", x => new { x.VenueId, x.AdminId });
                    table.ForeignKey(
                        name: "FK_VenueManagements_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "AdminId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VenueManagements_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "VenueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VenueAnnouncements",
                columns: table => new
                {
                    VenueId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    AnnouncementId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VenueAnnouncements", x => new { x.VenueId, x.AnnouncementId });
                    table.ForeignKey(
                        name: "FK_VenueAnnouncements_Announcements_AnnouncementId",
                        column: x => x.AnnouncementId,
                        principalTable: "Announcements",
                        principalColumn: "AnnouncementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VenueAnnouncements_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "VenueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceRecords",
                columns: table => new
                {
                    MaintenanceRecordId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    EquipmentId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    MaintenanceStartTime = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    MaintenanceEndTime = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    MaintenanceDetails = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceRecords", x => x.MaintenanceRecordId);
                    table.ForeignKey(
                        name: "FK_MaintenanceRecords_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "EquipmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VenueEquipments",
                columns: table => new
                {
                    EquipmentId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    VenueId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    InstallationTime = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VenueEquipments", x => new { x.EquipmentId, x.VenueId });
                    table.ForeignKey(
                        name: "FK_VenueEquipments_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "EquipmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VenueEquipments_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "VenueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    ReservationId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    VenueId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    AvailabilityId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    ReservationItem = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ReservationTime = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    PaymentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReservationType = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    UserId = table.Column<string>(type: "NVARCHAR2(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.ReservationId);
                    table.ForeignKey(
                        name: "FK_Reservations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Reservations_VenueAvailabilities_AvailabilityId",
                        column: x => x.AvailabilityId,
                        principalTable: "VenueAvailabilities",
                        principalColumn: "AvailabilityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservations_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "VenueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupReservationMembers",
                columns: table => new
                {
                    ReservationId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    GroupId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupReservationMembers", x => new { x.ReservationId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_GroupReservationMembers_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupReservationMembers_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "ReservationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserReservations",
                columns: table => new
                {
                    ReservationId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    UserId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    CheckInTime = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    Status = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NumOfPeople = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReservations", x => new { x.UserId, x.ReservationId });
                    table.ForeignKey(
                        name: "FK_UserReservations_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "ReservationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserReservations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminNotifications_AdminId",
                table: "AdminNotifications",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_AdminId",
                table: "Announcements",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_AdminId",
                table: "Equipments",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupReservationMembers_GroupId",
                table: "GroupReservationMembers",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUsers_GroupId",
                table: "GroupUsers",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRecords_EquipmentId",
                table: "MaintenanceRecords",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_AvailabilityId",
                table: "Reservations",
                column: "AvailabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_UserId",
                table: "Reservations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_VenueId",
                table: "Reservations",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_UserId",
                table: "UserNotifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserReservations_ReservationId",
                table: "UserReservations",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_VenueAnnouncements_AnnouncementId",
                table: "VenueAnnouncements",
                column: "AnnouncementId");

            migrationBuilder.CreateIndex(
                name: "IX_VenueAvailabilities_VenueId",
                table: "VenueAvailabilities",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_VenueEquipments_VenueId",
                table: "VenueEquipments",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_VenueMaintenance_VenueId",
                table: "VenueMaintenance",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_VenueManagements_AdminId",
                table: "VenueManagements",
                column: "AdminId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminNotifications");

            migrationBuilder.DropTable(
                name: "GroupReservationMembers");

            migrationBuilder.DropTable(
                name: "GroupUsers");

            migrationBuilder.DropTable(
                name: "MaintenanceRecords");

            migrationBuilder.DropTable(
                name: "UserNotifications");

            migrationBuilder.DropTable(
                name: "UserReservations");

            migrationBuilder.DropTable(
                name: "VenueAnnouncements");

            migrationBuilder.DropTable(
                name: "VenueEquipments");

            migrationBuilder.DropTable(
                name: "VenueMaintenance");

            migrationBuilder.DropTable(
                name: "VenueManagements");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "Equipments");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "VenueAvailabilities");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Venues");
        }
    }
}
