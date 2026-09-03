using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "subsidiaries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subsidiaries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    azure_ad_object_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    display_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    role = table.Column<string>(type: "character varying(50)", nullable: false),
                    subsidiary_id = table.Column<Guid>(type: "uuid", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_subsidiaries_subsidiary_id",
                        column: x => x.subsidiary_id,
                        principalTable: "subsidiaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "bank_accounts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    subsidiary_id = table.Column<Guid>(type: "uuid", nullable: false),
                    bank_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    account_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    account_type = table.Column<string>(type: "character varying(20)", nullable: false),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bank_accounts", x => x.id);
                    table.ForeignKey(
                        name: "FK_bank_accounts_subsidiaries_subsidiary_id",
                        column: x => x.subsidiary_id,
                        principalTable: "subsidiaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cash_flow_projections",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    subsidiary_id = table.Column<Guid>(type: "uuid", nullable: false),
                    projection_date = table.Column<DateOnly>(type: "date", nullable: false),
                    projection_days = table.Column<string>(type: "character varying(50)", nullable: false),
                    projected_inflow = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    projected_outflow = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    projected_balance = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    calculated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cash_flow_projections", x => x.id);
                    table.ForeignKey(
                        name: "FK_cash_flow_projections_subsidiaries_subsidiary_id",
                        column: x => x.subsidiary_id,
                        principalTable: "subsidiaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expected_collections",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    subsidiary_id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    expected_date = table.Column<DateOnly>(type: "date", nullable: false),
                    actual_date = table.Column<DateOnly>(type: "date", nullable: true),
                    status = table.Column<string>(type: "character varying(20)", nullable: false),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expected_collections", x => x.id);
                    table.ForeignKey(
                        name: "FK_expected_collections_subsidiaries_subsidiary_id",
                        column: x => x.subsidiary_id,
                        principalTable: "subsidiaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_expected_collections_users_created_by",
                        column: x => x.created_by,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "payment_requests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    subsidiary_id = table.Column<Guid>(type: "uuid", nullable: false),
                    vendor_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    request_date = table.Column<DateOnly>(type: "date", nullable: false),
                    due_date = table.Column<DateOnly>(type: "date", nullable: true),
                    status = table.Column<string>(type: "character varying(30)", nullable: false),
                    rejection_reason = table.Column<string>(type: "text", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    approved_by = table.Column<Guid>(type: "uuid", nullable: true),
                    approved_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    payment_proof_path = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_requests", x => x.id);
                    table.ForeignKey(
                        name: "FK_payment_requests_subsidiaries_subsidiary_id",
                        column: x => x.subsidiary_id,
                        principalTable: "subsidiaries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_payment_requests_users_approved_by",
                        column: x => x.approved_by,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_payment_requests_users_created_by",
                        column: x => x.created_by,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "bank_statements",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    bank_account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    statement_date = table.Column<DateOnly>(type: "date", nullable: false),
                    file_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    file_path = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    total_credits = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    total_debits = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    final_balance = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    import_status = table.Column<string>(type: "character varying(20)", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bank_statements", x => x.id);
                    table.ForeignKey(
                        name: "FK_bank_statements_bank_accounts_bank_account_id",
                        column: x => x.bank_account_id,
                        principalTable: "bank_accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_bank_statements_users_created_by",
                        column: x => x.created_by,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "approval_history",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    payment_request_id = table.Column<Guid>(type: "uuid", nullable: false),
                    approver_id = table.Column<Guid>(type: "uuid", nullable: false),
                    action = table.Column<string>(type: "character varying(50)", nullable: false),
                    comments = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_approval_history", x => x.id);
                    table.ForeignKey(
                        name: "FK_approval_history_payment_requests_payment_request_id",
                        column: x => x.payment_request_id,
                        principalTable: "payment_requests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_approval_history_users_approver_id",
                        column: x => x.approver_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "audit_logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    entity_type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    entity_id = table.Column<Guid>(type: "uuid", nullable: false),
                    action = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    old_values = table.Column<string>(type: "text", nullable: true),
                    new_values = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ip_address = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_logs", x => x.id);
                    table.ForeignKey(
                        name: "FK_audit_logs_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "bank_statement_lines",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    bank_statement_id = table.Column<Guid>(type: "uuid", nullable: false),
                    line_number = table.Column<int>(type: "integer", nullable: false),
                    transaction_date = table.Column<DateOnly>(type: "date", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    reference = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    credit = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    debit = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    balance = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    is_reconciled = table.Column<bool>(type: "boolean", nullable: false),
                    reconciled_with_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bank_statement_lines", x => x.id);
                    table.ForeignKey(
                        name: "FK_bank_statement_lines_bank_statement_lines_reconciled_with_id",
                        column: x => x.reconciled_with_id,
                        principalTable: "bank_statement_lines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_bank_statement_lines_bank_statements_bank_statement_id",
                        column: x => x.bank_statement_id,
                        principalTable: "bank_statements",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_created_at",
                table: "audit_logs",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_entity_id",
                table: "audit_logs",
                column: "entity_id");

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_entity_type",
                table: "audit_logs",
                column: "entity_type");

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_user_id",
                table: "audit_logs",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_approval_history_approver_id",
                table: "approval_history",
                column: "approver_id");

            migrationBuilder.CreateIndex(
                name: "IX_approval_history_payment_request_id",
                table: "approval_history",
                column: "payment_request_id");

            migrationBuilder.CreateIndex(
                name: "IX_bank_accounts_subsidiary_id",
                table: "bank_accounts",
                column: "subsidiary_id");

            migrationBuilder.CreateIndex(
                name: "IX_bank_statement_lines_bank_statement_id",
                table: "bank_statement_lines",
                column: "bank_statement_id");

            migrationBuilder.CreateIndex(
                name: "IX_bank_statement_lines_reconciled_with_id",
                table: "bank_statement_lines",
                column: "reconciled_with_id");

            migrationBuilder.CreateIndex(
                name: "IX_bank_statements_bank_account_id_statement_date",
                table: "bank_statements",
                columns: new[] { "bank_account_id", "statement_date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bank_statements_created_by",
                table: "bank_statements",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_cash_flow_projections_subsidiary_id",
                table: "cash_flow_projections",
                column: "subsidiary_id");

            migrationBuilder.CreateIndex(
                name: "IX_expected_collections_created_by",
                table: "expected_collections",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_expected_collections_subsidiary_id",
                table: "expected_collections",
                column: "subsidiary_id");

            migrationBuilder.CreateIndex(
                name: "IX_payment_requests_approved_by",
                table: "payment_requests",
                column: "approved_by");

            migrationBuilder.CreateIndex(
                name: "IX_payment_requests_created_by",
                table: "payment_requests",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_payment_requests_subsidiary_id",
                table: "payment_requests",
                column: "subsidiary_id");

            migrationBuilder.CreateIndex(
                name: "IX_subsidiaries_code",
                table: "subsidiaries",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_azure_ad_object_id",
                table: "users",
                column: "azure_ad_object_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_users_subsidiary_id",
                table: "users",
                column: "subsidiary_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "approval_history");

            migrationBuilder.DropTable(
                name: "audit_logs");

            migrationBuilder.DropTable(
                name: "bank_statement_lines");

            migrationBuilder.DropTable(
                name: "cash_flow_projections");

            migrationBuilder.DropTable(
                name: "expected_collections");

            migrationBuilder.DropTable(
                name: "payment_requests");

            migrationBuilder.DropTable(
                name: "bank_statements");

            migrationBuilder.DropTable(
                name: "bank_accounts");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "subsidiaries");
        }
    }
}
