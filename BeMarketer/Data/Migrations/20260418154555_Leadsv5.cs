using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeMarketer.Data.Migrations
{
    /// <inheritdoc />
    public partial class Leadsv5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin-role-id-1234",
                column: "ConcurrencyStamp",
                value: "3ccad01a-ffe9-4a55-9e2c-01c2b9aef564");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "user-role-id-5678",
                column: "ConcurrencyStamp",
                value: "86cb22e9-e423-4212-9157-4e743d69dc1d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id-9999",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "de0d6850-9554-4b10-aa7c-cc0468ce90e8", "AQAAAAIAAYagAAAAEOXK9n1k1e/Y8w/V1Z0Z9VbQ2/z10+QG9WjQ1wG8rG5P9/XQ==", "b57530e0-c115-46b0-bb2e-0d04dbcf8ab1" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin-role-id-1234",
                column: "ConcurrencyStamp",
                value: "8b3698e8-270e-483b-aad3-02b9c10631a5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "user-role-id-5678",
                column: "ConcurrencyStamp",
                value: "e2169fa3-ea50-4382-bf58-81e187461c56");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id-9999",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "783b114a-c6f5-44cf-8d33-a0a756c534e4", "AQAAAAIAAYagAAAAEGWsfDC5F25M+ckTv2Nf7kYCd3hre2mNXWDFnrFInNygbLQvGslQaM/3ZAmMTVd3Lw==", "6126b565-3172-4922-9dea-ef36b20b20a3" });
        }
    }
}
