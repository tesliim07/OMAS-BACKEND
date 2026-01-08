using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMedicalAppointmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceDescriptionToServiceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ServiceDescription",
                table: "Services",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceDescription",
                table: "Services");
        }
    }
}
