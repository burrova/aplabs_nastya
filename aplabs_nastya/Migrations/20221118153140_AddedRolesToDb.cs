using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aplabs_nastya.Migrations
{
    public partial class AddedRolesToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "60885684-ba54-4f51-824a-5848db8e22a6", "27fb5355-6159-4b3c-a582-f0dc28229a85", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c83bfc69-1942-4126-834a-64955d7caf96", "c8cfef4c-3a3c-47d2-b345-e64f1e911c16", "Manager", "MANAGER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "60885684-ba54-4f51-824a-5848db8e22a6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c83bfc69-1942-4126-834a-64955d7caf96");
        }
    }
}
