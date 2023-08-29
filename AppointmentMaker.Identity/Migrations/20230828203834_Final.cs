using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AppointmentMaker.Identity.Migrations
{
    /// <inheritdoc />
    public partial class Final : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_FamilyDoctorId",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0161c73c-6e34-49e9-877e-5c34ba7f588e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b0044685-478f-4f21-8843-e7af17e89f03");

            migrationBuilder.DropColumn(
                name: "Doctor_Birthday",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Doctor_FullName",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "PhotoId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FileModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Extention = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    DoctorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoctorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScheduleSlots = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ScheduleTemplate = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3583abd8-e2d8-4bc7-bb1b-9dd5ab3adefc", null, "ApplicationRole", "Patient", "PATIENT" },
                    { "b3af00d0-048a-4985-bdec-f11770d44d95", null, "ApplicationRole", "Doctor", "DOCTOR" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PhotoId",
                table: "AspNetUsers",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ScheduleId",
                table: "AspNetUsers",
                column: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_FamilyDoctorId",
                table: "AspNetUsers",
                column: "FamilyDoctorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_FileModel_PhotoId",
                table: "AspNetUsers",
                column: "PhotoId",
                principalTable: "FileModel",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Schedule_ScheduleId",
                table: "AspNetUsers",
                column: "ScheduleId",
                principalTable: "Schedule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_FamilyDoctorId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_FileModel_PhotoId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Schedule_ScheduleId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "FileModel");

            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PhotoId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ScheduleId",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3583abd8-e2d8-4bc7-bb1b-9dd5ab3adefc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3af00d0-048a-4985-bdec-f11770d44d95");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "Doctor_Birthday",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Doctor_FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0161c73c-6e34-49e9-877e-5c34ba7f588e", null, "ApplicationRole", "Doctor", "DOCTOR" },
                    { "b0044685-478f-4f21-8843-e7af17e89f03", null, "ApplicationRole", "Patient", "PATIENT" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_FamilyDoctorId",
                table: "AspNetUsers",
                column: "FamilyDoctorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
