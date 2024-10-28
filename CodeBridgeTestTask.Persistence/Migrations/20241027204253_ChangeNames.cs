using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeBridgeTestTask.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Weigth",
                table: "Dogs",
                newName: "Weight");

            migrationBuilder.RenameColumn(
                name: "TailLength",
                table: "Dogs",
                newName: "TailLenght");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "Dogs",
                newName: "Weigth");

            migrationBuilder.RenameColumn(
                name: "TailLenght",
                table: "Dogs",
                newName: "TailLength");
        }
    }
}
