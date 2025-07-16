using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiServiceAppointmentManager.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceTypesToProvider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ServiceType",
                table: "Providers",
                newName: "ServiceTypes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ServiceTypes",
                table: "Providers",
                newName: "ServiceType");
        }
    }
}
