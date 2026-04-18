using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BeMarketer.Data.Migrations
{
    /// <inheritdoc />
    public partial class SendAdminAccv3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "user-role-id-5678");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "admin-role-id-1234", "admin-user-id-9999" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin-role-id-1234");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id-9999");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "admin-role-id-1234", "a8833b70-a702-4348-8f01-e64e78c19ccd", "Admin", "ADMIN" },
                    { "user-role-id-5678", "d4e83b07-b6c8-4619-8f5e-5def86569724", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "admin-user-id-9999", 0, "837ac685-50d7-4564-a175-a0fbf1c034c4", "admin@bemarketer.pl", true, false, null, "ADMIN@BEMARKETER.PL", "ADMIN@BEMARKETER.PL", "AQAAAAIAAYagAAAAEBnXpXJzvoBCbpoluWVDN87JPoAW+vnRuf12LHl95qa7WhuCH+t9qzJOfk42lNTRFw==", null, false, 0, "7a3e9db8-b72f-4451-9532-887370d6d648", false, "admin@bemarketer.pl" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "admin-role-id-1234", "admin-user-id-9999" });
        }
    }
}
