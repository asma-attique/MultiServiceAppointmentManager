using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiServiceAppointmentManager.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Providers_ProviderId",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Services",
                newName: "Charges");

            migrationBuilder.AlterColumn<int>(
                name: "ProviderId",
                table: "Services",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Providers_ProviderId",
                table: "Services",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Providers_ProviderId",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "Charges",
                table: "Services",
                newName: "Price");

            migrationBuilder.AlterColumn<int>(
                name: "ProviderId",
                table: "Services",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Providers_ProviderId",
                table: "Services",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
