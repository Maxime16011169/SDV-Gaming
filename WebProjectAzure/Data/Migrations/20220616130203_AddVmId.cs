using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebProjectAzure.Data.Migrations
{
    public partial class AddVmId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdVm",
                table: "ListeAbonnement",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdVm",
                table: "ListeAbonnement");
        }
    }
}
