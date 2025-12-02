using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fcg.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleAndUpdatePassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Clientes",
                type: "VARCHAR(300)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Clientes",
                type: "VARCHAR(20)",
                nullable: false,
                defaultValue: "User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Clientes");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Clientes",
                type: "VARCHAR(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(300)");
        }
    }
}
