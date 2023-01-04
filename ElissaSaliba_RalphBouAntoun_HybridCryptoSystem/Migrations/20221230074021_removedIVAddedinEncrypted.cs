using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElissaSaliba_RalphBouAntoun_HybridCryptoSystem.Migrations
{
    public partial class removedIVAddedinEncrypted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IV",
                table: "Messages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IV",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
