using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiServiceAppointmentManager.Migrations
{
    /// <inheritdoc />
    public partial class AddProviderServiceTypeAndExperience : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ServiceType",
                table: "Providers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "YearsOfExperience",
                table: "Providers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceType",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "YearsOfExperience",
                table: "Providers");
        }
    }
}
