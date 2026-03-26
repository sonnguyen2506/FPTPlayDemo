using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FPTPlay.Migrations
{
    /// <inheritdoc />
    public partial class RenamePhoneToMobile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
            name: "PhoneNumber",
            table: "Users",
            newName: "Mobile");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
