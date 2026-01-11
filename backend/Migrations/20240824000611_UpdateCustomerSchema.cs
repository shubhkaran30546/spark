using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace spark.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustomerSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SecurityAnswerHash",
                table: "Customers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SecurityQuestion",
                table: "Customers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecurityAnswerHash",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "SecurityQuestion",
                table: "Customers");
        }
    }
}
