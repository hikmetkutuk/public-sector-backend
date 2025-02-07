using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganisationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "organisations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    latitude = table.Column<string>(type: "text", nullable: true),
                    longitude = table.Column<string>(type: "text", nullable: true),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    geographic_region_def_id = table.Column<Guid>(type: "uuid", nullable: true),
                    procedure_region_def_id = table.Column<Guid>(type: "uuid", nullable: true),
                    center_class = table.Column<byte>(type: "smallint", nullable: true),
                    local_status = table.Column<int>(type: "integer", nullable: true),
                    atm_count = table.Column<int>(type: "integer", nullable: true),
                    branch_count = table.Column<int>(type: "integer", nullable: true),
                    distribution_status = table.Column<int>(type: "integer", nullable: true),
                    wage_take_machine = table.Column<int>(type: "integer", nullable: true),
                    wage_pay_machine = table.Column<int>(type: "integer", nullable: true),
                    document_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    document_number = table.Column<int>(type: "integer", nullable: true),
                    note = table.Column<string>(type: "text", nullable: true),
                    organisation_type = table.Column<int>(type: "integer", nullable: true),
                    converted_from_id = table.Column<Guid>(type: "uuid", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    created_by_user_id = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    modified_by_user_id = table.Column<string>(type: "text", nullable: true),
                    modified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_organisations", x => x.id);
                    table.ForeignKey(
                        name: "fk_organisations_definitions_geographic_region_def_id",
                        column: x => x.geographic_region_def_id,
                        principalTable: "definitions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_organisations_definitions_procedure_region_def_id",
                        column: x => x.procedure_region_def_id,
                        principalTable: "definitions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_organisations_organisations_converted_from_id",
                        column: x => x.converted_from_id,
                        principalTable: "organisations",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_organisations_organisations_parent_id",
                        column: x => x.parent_id,
                        principalTable: "organisations",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "organisation_logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    organisation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    document_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    document_number = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    note = table.Column<string>(type: "text", nullable: true),
                    created_by_user_id = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    modified_by_user_id = table.Column<string>(type: "text", nullable: true),
                    modified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_organisation_logs", x => x.id);
                    table.ForeignKey(
                        name: "fk_organisation_logs_organisations_organisation_id",
                        column: x => x.organisation_id,
                        principalTable: "organisations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_organisation_logs_organisation_id",
                table: "organisation_logs",
                column: "organisation_id");

            migrationBuilder.CreateIndex(
                name: "ix_organisations_converted_from_id",
                table: "organisations",
                column: "converted_from_id");

            migrationBuilder.CreateIndex(
                name: "ix_organisations_geographic_region_def_id",
                table: "organisations",
                column: "geographic_region_def_id");

            migrationBuilder.CreateIndex(
                name: "ix_organisations_parent_id",
                table: "organisations",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_organisations_procedure_region_def_id",
                table: "organisations",
                column: "procedure_region_def_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "organisation_logs");

            migrationBuilder.DropTable(
                name: "organisations");
        }
    }
}
