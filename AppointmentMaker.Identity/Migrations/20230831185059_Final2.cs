using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AppointmentMaker.Identity.Migrations
{
    /// <inheritdoc />
    public partial class Final2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Schedule_ScheduleId",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3583abd8-e2d8-4bc7-bb1b-9dd5ab3adefc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3af00d0-048a-4985-bdec-f11770d44d95");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8da2b8f0-6074-418a-811f-3dd794ed2081", null, "ApplicationRole", "Doctor", "DOCTOR" },
                    { "f93f78cb-5970-4278-9e04-608f12908ccc", null, "ApplicationRole", "Patient", "PATIENT" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Schedule_ScheduleId",
                table: "AspNetUsers",
                column: "ScheduleId",
                principalTable: "Schedule",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Schedule_ScheduleId",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8da2b8f0-6074-418a-811f-3dd794ed2081");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f93f78cb-5970-4278-9e04-608f12908ccc");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3583abd8-e2d8-4bc7-bb1b-9dd5ab3adefc", null, "ApplicationRole", "Patient", "PATIENT" },
                    { "b3af00d0-048a-4985-bdec-f11770d44d95", null, "ApplicationRole", "Doctor", "DOCTOR" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Schedule_ScheduleId",
                table: "AspNetUsers",
                column: "ScheduleId",
                principalTable: "Schedule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
