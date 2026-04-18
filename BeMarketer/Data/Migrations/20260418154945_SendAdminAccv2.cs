using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeMarketer.Data.Migrations
{
    /// <inheritdoc />
    public partial class SendAdminAccv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "admin-role-id-1234",
                column: "ConcurrencyStamp",
                value: "a8833b70-a702-4348-8f01-e64e78c19ccd");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "user-role-id-5678",
                column: "ConcurrencyStamp",
                value: "d4e83b07-b6c8-4619-8f5e-5def86569724");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "admin-user-id-9999",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "837ac685-50d7-4564-a175-a0fbf1c034c4", "AQAAAAIAAYagAAAAEBnXpXJzvoBCbpoluWVDN87JPoAW+vnRuf12LHl95qa7WhuCH+t9qzJOfk42lNTRFw==", "7a3e9db8-b72f-4451-9532-887370d6d648" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ba8c7e65-222b-475e-b9be-31a7a13b1e6a", "AQAAAAIAAYagAAAAEOXK9n1k1e/Y8w/V1Z0Z9VbQ2/z10+QG9WjQ1wG8rG5P9/XQ==", "b57530e0-c115-46b0-bb2e-0d04dbcf8ab1" });
        }
    }
}
