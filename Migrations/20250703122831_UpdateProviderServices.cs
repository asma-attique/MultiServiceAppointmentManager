using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiServiceAppointmentManager.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProviderServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Providers_ProviderId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_ProviderId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ServiceTypes",
                table: "Providers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Services",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateTable(
                name: "ProviderService",
                columns: table => new
                {
                    ProvidersId = table.Column<int>(type: "INTEGER", nullable: false),
                    ServicesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderService", x => new { x.ProvidersId, x.ServicesId });
                    table.ForeignKey(
                        name: "FK_ProviderService_Providers_ProvidersId",
                        column: x => x.ProvidersId,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProviderService_Services_ServicesId",
                        column: x => x.ServicesId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProviderService_ServicesId",
                table: "ProviderService",
                column: "ServicesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProviderService");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Services",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProviderId",
                table: "Services",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceTypes",
                table: "Providers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ProviderId",
                table: "Services",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Providers_ProviderId",
                table: "Services",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "Id");
        }
    }
}
