using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeMarketer.Data.Migrations
{
    /// <inheritdoc />
    public partial class SendAdminAccv1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin-role-id-1234",
                column: "ConcurrencyStamp",
                value: "e7db4a6c-eff4-4d10-af5f-b0e3699168eb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "user-role-id-5678",
                column: "ConcurrencyStamp",
                value: "d3b41aa5-d57b-4cc8-b580-53c496f34e2f");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id-9999",
                column: "ConcurrencyStamp",
                value: "ba8c7e65-222b-475e-b9be-31a7a13b1e6a");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                column: "ConcurrencyStamp",
                value: "de0d6850-9554-4b10-aa7c-cc0468ce90e8");
        }
    }
}
