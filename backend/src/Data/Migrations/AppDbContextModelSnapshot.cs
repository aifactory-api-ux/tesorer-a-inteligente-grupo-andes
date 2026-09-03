namespace Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateModelSnapshot : ModelSnapshot
    {
        /// <inheritdoc />
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Backend.Models.Entities.AuditLog", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid")
                    .HasColumnName("id");

                b.Property<string>("Action")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("character varying(50)")
                    .HasColumnName("action");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("created_at");

                b.Property<Guid>("EntityId")
                    .HasColumnType("uuid")
                    .HasColumnName("entity_id");

                b.Property<string>("EntityType")
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnType("character varying(255)")
                    .HasColumnName("entity_type");

                b.Property<string>("IpAddress")
                    .HasMaxLength(50)
                    .HasColumnType("character varying(50)")
                    .HasColumnName("ip_address");

                b.Property<string>("NewValues")
                    .HasColumnType("text")
                    .HasColumnName("new_values");

                b.Property<string>("OldValues")
                    .HasColumnType("text")
                    .HasColumnName("old_values");

                b.Property<Guid>("UserId")
                    .HasColumnType("uuid")
                    .HasColumnName("user_id");

                b.Property<string>("UserEmail")
                    .HasMaxLength(255)
                    .HasColumnType("character varying(255)")
                    .HasColumnName("user_email");

                b.HasKey("Id");

                b.HasIndex("CreatedAt");

                b.HasIndex("EntityId");

                b.HasIndex("EntityType");

                b.HasIndex("UserId");

                b.ToTable("audit_logs", (string)null);
            });

            modelBuilder.Entity("Backend.Models.Entities.BankAccount", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid")
                    .HasColumnName("id");

                b.Property<string>("AccountNumber")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("character varying(50)")
                    .HasColumnName("account_number");

                b.Property<string>("AccountType")
                    .IsRequired()
                    .HasColumnType("character varying(20)")
                    .HasColumnName("account_type");

                b.Property<string>("BankName")
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnType("character varying(255)")
                    .HasColumnName("bank_name");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("created_at");

                b.Property<string>("Currency")
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnType("character varying(3)")
                    .HasColumnName("currency");

                b.Property<bool>("IsActive")
                    .HasColumnType("boolean")
                    .HasColumnName("is_active");

                b.Property<Guid>("SubsidiaryId")
                    .HasColumnType("uuid")
                    .HasColumnName("subsidiary_id");

                b.HasKey("Id");

                b.HasIndex("SubsidiaryId");

                b.ToTable("bank_accounts", (string)null);
            });

            modelBuilder.Entity("Backend.Models.Entities.BankStatement", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid")
                    .HasColumnName("id");

                b.Property<Guid>("BankAccountId")
                    .HasColumnType("uuid")
                    .HasColumnName("bank_account_id");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("created_at");

                b.Property<Guid?>("CreatedBy")
                    .HasColumnType("uuid")
                    .HasColumnName("created_by");

                b.Property<decimal>("FinalBalance")
                    .HasColumnType("numeric(18,2)")
                    .HasColumnName("final_balance");

                b.Property<string>("FileName")
                    .HasMaxLength(255)
                    .HasColumnType("character varying(255)")
                    .HasColumnName("file_name");

                b.Property<string>("FilePath")
                    .HasMaxLength(500)
                    .HasColumnType("character varying(500)")
                    .HasColumnName("file_path");

                b.Property<string>("ImportStatus")
                    .IsRequired()
                    .HasColumnType("character varying(20)")
                    .HasColumnName("import_status");

                b.Property<DateOnly>("StatementDate")
                    .HasColumnType("date")
                    .HasColumnName("statement_date");

                b.Property<decimal>("TotalCredits")
                    .HasColumnType("numeric(18,2)")
                    .HasColumnName("total_credits");

                b.Property<decimal>("TotalDebits")
                    .HasColumnType("numeric(18,2)")
                    .HasColumnName("total_debits");

                b.HasKey("Id");

                b.HasIndex("BankAccountId", "StatementDate")
                    .IsUnique();

                b.HasIndex("CreatedBy");

                b.ToTable("bank_statements", (string)null);
            });

            modelBuilder.Entity("Backend.Models.Entities.BankStatementLine", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid")
                    .HasColumnName("id");

                b.Property<decimal?>("Balance")
                    .HasColumnType("numeric(18,2)")
                    .HasColumnName("balance");

                b.Property<Guid>("BankStatementId")
                    .HasColumnType("uuid")
                    .HasColumnName("bank_statement_id");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("created_at");

                b.Property<string>("Credit")
                    .HasColumnType("numeric(18,2)")
                    .HasColumnName("credit");

                b.Property<string>("Debit")
                    .HasColumnType("numeric(18,2)")
                    .HasColumnName("debit");

                b.Property<string>("Description")
                    .HasColumnType("text")
                    .HasColumnName("description");

                b.Property<bool>("IsReconciled")
                    .HasColumnType("boolean")
                    .HasColumnName("is_reconciled");

                b.Property<int>("LineNumber")
                    .HasColumnType("integer")
                    .HasColumnName("line_number");

                b.Property<string>("Reference")
                    .HasMaxLength(255)
                    .HasColumnType("character varying(255)")
                    .HasColumnName("reference");

                b.Property<Guid?>("ReconciledWithId")
                    .HasColumnType("uuid")
                    .HasColumnName("reconciled_with_id");

                b.Property<DateOnly>("TransactionDate")
                    .HasColumnType("date")
                    .HasColumnName("transaction_date");

                b.HasKey("Id");

                b.HasIndex("BankStatementId");

                b.HasIndex("ReconciledWithId");

                b.ToTable("bank_statement_lines", (string)null);
            });

            modelBuilder.Entity("Backend.Models.Entities.CashFlowProjection", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid")
                    .HasColumnName("id");

                b.Property<DateTime>("CalculatedAt")
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("calculated_at");

                b.Property<decimal>("ProjectedBalance")
                    .HasColumnType("numeric(18,2)")
                    .HasColumnName("projected_balance");

                b.Property<decimal>("ProjectedInflow")
                    .HasColumnType("numeric(18,2)")
                    .HasColumnName("projected_inflow");

                b.Property<decimal>("ProjectedOutflow")
                    .HasColumnType("numeric(18,2)")
                    .HasColumnName("projected_outflow");

                b.Property<DateOnly>("ProjectionDate")
                    .HasColumnType("date")
                    .HasColumnName("projection_date");

                b.Property<string>("ProjectionDays")
                    .IsRequired()
                    .HasColumnType("character varying(50)")
                    .HasColumnName("projection_days");

                b.Property<Guid>("SubsidiaryId")
                    .HasColumnType("uuid")
                    .HasColumnName("subsidiary_id");

                b.HasKey("Id");

                b.HasIndex("SubsidiaryId");

                b.ToTable("cash_flow_projections", (string)null);
            });

            modelBuilder.Entity("Backend.Models.Entities.ExpectedCollection", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid")
                    .HasColumnName("id");

                b.Property<decimal>("Amount")
                    .HasColumnType("numeric(18,2)")
                    .HasColumnName("amount");

                b.Property<DateTime?>("ActualDate")
                    .HasColumnType("date")
                    .HasColumnName("actual_date");

                b.Property<string>("CustomerName")
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnType("character varying(255)")
                    .HasColumnName("customer_name");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("created_at");

                b.Property<Guid?>("CreatedBy")
                    .HasColumnType("uuid")
                    .HasColumnName("created_by");

                b.Property<DateOnly>("ExpectedDate")
                    .HasColumnType("date")
                    .HasColumnName("expected_date");

                b.Property<string>("Notes")
                    .HasColumnType("text")
                    .HasColumnName("notes");

                b.Property<string>("Status")
                    .IsRequired()
                    .HasColumnType("character varying(20)")
                    .HasColumnName("status");

                b.Property<Guid>("SubsidiaryId")
                    .HasColumnType("uuid")
                    .HasColumnName("subsidiary_id");

                b.Property<DateTime>("UpdatedAt")
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("updated_at");

                b.HasKey("Id");

                b.HasIndex("CreatedBy");

                b.HasIndex("SubsidiaryId");

                b.ToTable("expected_collections", (string)null);
            });

            modelBuilder.Entity("Backend.Models.Entities.PaymentRequest", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid")
                    .HasColumnName("id");

                b.Property<DateTime?>("ApprovedAt")
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("approved_at");

                b.Property<Guid?>("ApprovedBy")
                    .HasColumnType("uuid")
                    .HasColumnName("approved_by");

                b.Property<decimal>("Amount")
                    .HasColumnType("numeric(18,2)")
                    .HasColumnName("amount");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("created_at");

                b.Property<Guid?>("CreatedBy")
                    .HasColumnType("uuid")
                    .HasColumnName("created_by");

                b.Property<string>("Currency")
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnType("character varying(3)")
                    .HasColumnName("currency");

                b.Property<string>("Description")
                    .HasColumnType("text")
                    .HasColumnName("description");

                b.Property<DateOnly?>("DueDate")
                    .HasColumnType("date")
                    .HasColumnName("due_date");

                b.Property<string>("PaymentProofPath")
                    .HasMaxLength(500)
                    .HasColumnType("character varying(500)")
                    .HasColumnName("payment_proof_path");

                b.Property<DateOnly>("RequestDate")
                    .HasColumnType("date")
                    .HasColumnName("request_date");

                b.Property<string>("RejectionReason")
                    .HasColumnType("text")
                    .HasColumnName("rejection_reason");

                b.Property<string>("Status")
                    .IsRequired()
                    .HasColumnType("character varying(30)")
                    .HasColumnName("status");

                b.Property<Guid>("SubsidiaryId")
                    .HasColumnType("uuid")
                    .HasColumnName("subsidiary_id");

                b.Property<DateTime>("UpdatedAt")
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("updated_at");

                b.Property<string>("VendorName")
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnType("character varying(255)")
                    .HasColumnName("vendor_name");

                b.HasKey("Id");

                b.HasIndex("ApprovedBy");

                b.HasIndex("CreatedBy");

                b.HasIndex("SubsidiaryId");

                b.ToTable("payment_requests", (string)null);
            });

            modelBuilder.Entity("Backend.Models.Entities.ApprovalHistory", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid")
                    .HasColumnName("id");

                b.Property<string>("Action")
                    .IsRequired()
                    .HasColumnType("character varying(50)")
                    .HasColumnName("action");

                b.Property<Guid>("ApproverId")
                    .HasColumnType("uuid")
                    .HasColumnName("approver_id");

                b.Property<string>("Comments")
                    .HasColumnType("text")
                    .HasColumnName("comments");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("created_at");

                b.Property<Guid>("PaymentRequestId")
                    .HasColumnType("uuid")
                    .HasColumnName("payment_request_id");

                b.HasKey("Id");

                b.HasIndex("ApproverId");

                b.HasIndex("PaymentRequestId");

                b.ToTable("approval_history", (string)null);
            });

            modelBuilder.Entity("Backend.Models.Entities.Subsidiary", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid")
                    .HasColumnName("id");

                b.Property<string>("Code")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("character varying(50)")
                    .HasColumnName("code");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("created_at");

                b.Property<bool>("IsActive")
                    .HasColumnType("boolean")
                    .HasColumnName("is_active");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnType("character varying(255)")
                    .HasColumnName("name");

                b.HasKey("Id");

                b.HasIndex("Code")
                    .IsUnique();

                b.ToTable("subsidiaries", (string)null);
            });

            modelBuilder.Entity("Backend.Models.Entities.User", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid")
                    .HasColumnName("id");

                b.Property<string>("AzureAdObjectId")
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnType("character varying(255)")
                    .HasColumnName("azure_ad_object_id");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("created_at");

                b.Property<string>("DisplayName")
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnType("character varying(255)")
                    .HasColumnName("display_name");

                b.Property<string>("Email")
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnType("character varying(255)")
                    .HasColumnName("email");

                b.Property<bool>("IsActive")
                    .HasColumnType("boolean")
                    .HasColumnName("is_active");

                b.Property<string>("Role")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("character varying(50)")
                    .HasColumnName("role");

                b.Property<Guid?>("SubsidiaryId")
                    .HasColumnType("uuid")
                    .HasColumnName("subsidiary_id");

                b.Property<DateTime>("UpdatedAt")
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("updated_at");

                b.HasKey("Id");

                b.HasIndex("AzureAdObjectId")
                    .IsUnique();

                b.HasIndex("Email");

                b.HasIndex("SubsidiaryId");

                b.ToTable("users", (string)null);
            });

            modelBuilder.Entity("Backend.Models.Entities.ApprovalHistory", b =>
            {
                b.HasOne("Backend.Models.Entities.PaymentRequest", "PaymentRequest")
                    .WithMany("ApprovalHistories")
                    .HasForeignKey("PaymentRequestId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("Backend.Models.Entities.User", "Approver")
                    .WithMany()
                    .HasForeignKey("ApproverId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                b.Navigation("Approver");

                b.Navigation("PaymentRequest");
            });

            modelBuilder.Entity("Backend.Models.Entities.AuditLog", b =>
            {
                b.HasOne("Backend.Models.Entities.User", "User")
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                b.Navigation("User");
            });

            modelBuilder.Entity("Backend.Models.Entities.BankAccount", b =>
            {
                b.HasOne("Backend.Models.Entities.Subsidiary", "Subsidiary")
                    .WithMany("BankAccounts")
                    .HasForeignKey("SubsidiaryId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                b.Navigation("Subsidiary");
            });

            modelBuilder.Entity("Backend.Models.Entities.BankStatement", b =>
            {
                b.HasOne("Backend.Models.Entities.BankAccount", "BankAccount")
                    .WithMany("BankStatements")
                    .HasForeignKey("BankAccountId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                b.HasOne("Backend.Models.Entities.User", "Creator")
                    .WithMany()
                    .HasForeignKey("CreatedBy")
                    .OnDelete(DeleteBehavior.SetNull);

                b.Navigation("BankAccount");

                b.Navigation("Creator");
            });

            modelBuilder.Entity("Backend.Models.Entities.BankStatementLine", b =>
            {
                b.HasOne("Backend.Models.Entities.BankStatement", "BankStatement")
                    .WithMany("Lines")
                    .HasForeignKey("BankStatementId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("Backend.Models.Entities.BankStatementLine", "ReconciledWith")
                    .WithMany()
                    .HasForeignKey("ReconciledWithId")
                    .OnDelete(DeleteBehavior.SetNull);

                b.Navigation("BankStatement");

                b.Navigation("ReconciledWith");
            });

            modelBuilder.Entity("Backend.Models.Entities.CashFlowProjection", b =>
            {
                b.HasOne("Backend.Models.Entities.Subsidiary", "Subsidiary")
                    .WithMany("CashFlowProjections")
                    .HasForeignKey("SubsidiaryId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Subsidiary");
            });

            modelBuilder.Entity("Backend.Models.Entities.ExpectedCollection", b =>
            {
                b.HasOne("Backend.Models.Entities.Subsidiary", "Subsidiary")
                    .WithMany("ExpectedCollections")
                    .HasForeignKey("SubsidiaryId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                b.HasOne("Backend.Models.Entities.User", "Creator")
                    .WithMany()
                    .HasForeignKey("CreatedBy")
                    .OnDelete(DeleteBehavior.SetNull);

                b.Navigation("Creator");

                b.Navigation("Subsidiary");
            });

            modelBuilder.Entity("Backend.Models.Entities.PaymentRequest", b =>
            {
                b.HasOne("Backend.Models.Entities.Subsidiary", "Subsidiary")
                    .WithMany("PaymentRequests")
                    .HasForeignKey("SubsidiaryId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                b.HasOne("Backend.Models.Entities.User", "Approver")
                    .WithMany()
                    .HasForeignKey("ApprovedBy")
                    .OnDelete(DeleteBehavior.SetNull);

                b.HasOne("Backend.Models.Entities.User", "Creator")
                    .WithMany()
                    .HasForeignKey("CreatedBy")
                    .OnDelete(DeleteBehavior.SetNull);

                b.Navigation("Approver");

                b.Navigation("Creator");

                b.Navigation("Subsidiary");
            });

            modelBuilder.Entity("Backend.Models.Entities.User", b =>
            {
                b.HasOne("Backend.Models.Entities.Subsidiary", "Subsidiary")
                    .WithMany("Users")
                    .HasForeignKey("SubsidiaryId")
                    .OnDelete(DeleteBehavior.SetNull);

                b.Navigation("Subsidiary");
            });

            modelBuilder.Entity("Backend.Models.Entities.BankAccount", b =>
            {
                b.Navigation("BankStatements");
            });

            modelBuilder.Entity("Backend.Models.Entities.BankStatement", b =>
            {
                b.Navigation("Lines");
            });

            modelBuilder.Entity("Backend.Models.Entities.PaymentRequest", b =>
            {
                b.Navigation("ApprovalHistories");
            });

            modelBuilder.Entity("Backend.Models.Entities.Subsidiary", b =>
            {
                b.Navigation("BankAccounts");

                b.Navigation("CashFlowProjections");

                b.Navigation("ExpectedCollections");

                b.Navigation("PaymentRequests");

                b.Navigation("Users");
            });
#pragma warning restore 612, 618
        }
    }
}
