using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BeMarketer.Data.Migrations
{
    /// <inheritdoc />
    public partial class Leadsv4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "admin-role-id-1234", "8b3698e8-270e-483b-aad3-02b9c10631a5", "Admin", "ADMIN" },
                    { "user-role-id-5678", "e2169fa3-ea50-4382-bf58-81e187461c56", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "admin-user-id-9999", 0, "783b114a-c6f5-44cf-8d33-a0a756c534e4", "admin@bemarketer.pl", true, false, null, "ADMIN@BEMARKETER.PL", "ADMIN@BEMARKETER.PL", "AQAAAAIAAYagAAAAEGWsfDC5F25M+ckTv2Nf7kYCd3hre2mNXWDFnrFInNygbLQvGslQaM/3ZAmMTVd3Lw==", null, false, 0, "6126b565-3172-4922-9dea-ef36b20b20a3", false, "admin@bemarketer.pl" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "admin-role-id-1234", "admin-user-id-9999" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
