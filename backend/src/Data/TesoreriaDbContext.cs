using Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class TesoreriaDbContext : DbContext
{
    public TesoreriaDbContext(DbContextOptions<TesoreriaDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Subsidiary> Subsidiaries { get; set; } = null!;
    public DbSet<BankAccount> BankAccounts { get; set; } = null!;
    public DbSet<BankStatement> BankStatements { get; set; } = null!;
    public DbSet<BankStatementLine> BankStatementLines { get; set; } = null!;
    public DbSet<ExpectedCollection> ExpectedCollections { get; set; } = null!;
    public DbSet<PaymentRequest> PaymentRequests { get; set; } = null!;
    public DbSet<ApprovalHistory> ApprovalHistories { get; set; } = null!;
    public DbSet<CashFlowProjection> CashFlowProjections { get; set; } = null!;
    public DbSet<AuditLog> AuditLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.AzureAdObjectId).HasColumnName("azure_ad_object_id").IsRequired();
            entity.Property(e => e.Email).HasColumnName("email").IsRequired();
            entity.Property(e => e.DisplayName).HasColumnName("display_name").IsRequired();
            entity.Property(e => e.Role).HasColumnName("role").IsRequired();
            entity.Property(e => e.SubsidiaryId).HasColumnName("subsidiary_id");
            entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.HasIndex(e => e.AzureAdObjectId).IsUnique();
            entity.HasOne(e => e.Subsidiary).WithMany().HasForeignKey(e => e.SubsidiaryId);
        });

        modelBuilder.Entity<Subsidiary>(entity =>
        {
            entity.ToTable("subsidiaries");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired();
            entity.Property(e => e.Code).HasColumnName("code").IsRequired();
            entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.HasIndex(e => e.Code).IsUnique();
        });

        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.ToTable("bank_accounts");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.SubsidiaryId).HasColumnName("subsidiary_id").IsRequired();
            entity.Property(e => e.BankName).HasColumnName("bank_name").IsRequired();
            entity.Property(e => e.AccountNumber).HasColumnName("account_number").IsRequired();
            entity.Property(e => e.AccountType).HasColumnName("account_type").IsRequired();
            entity.Property(e => e.Currency).HasColumnName("currency").HasDefaultValue("CLP");
            entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.HasOne(e => e.Subsidiary).WithMany().HasForeignKey(e => e.SubsidiaryId);
        });

        modelBuilder.Entity<BankStatement>(entity =>
        {
            entity.ToTable("bank_statements");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.BankAccountId).HasColumnName("bank_account_id").IsRequired();
            entity.Property(e => e.StatementDate).HasColumnName("statement_date").IsRequired();
            entity.Property(e => e.FileName).HasColumnName("file_name");
            entity.Property(e => e.FilePath).HasColumnName("file_path");
            entity.Property(e => e.TotalCredits).HasColumnName("total_credits").HasDefaultValue(0);
            entity.Property(e => e.TotalDebits).HasColumnName("total_debits").HasDefaultValue(0);
            entity.Property(e => e.FinalBalance).HasColumnName("final_balance").IsRequired();
            entity.Property(e => e.ImportStatus).HasColumnName("import_status").HasDefaultValue("pending");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.HasOne(e => e.BankAccount).WithMany().HasForeignKey(e => e.BankAccountId);
            entity.HasOne(e => e.Creator).WithMany().HasForeignKey(e => e.CreatedBy);
            entity.HasIndex(e => new { e.BankAccountId, e.StatementDate }).IsUnique();
        });

        modelBuilder.Entity<BankStatementLine>(entity =>
        {
            entity.ToTable("bank_statement_lines");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.BankStatementId).HasColumnName("bank_statement_id").IsRequired();
            entity.Property(e => e.LineNumber).HasColumnName("line_number").IsRequired();
            entity.Property(e => e.TransactionDate).HasColumnName("transaction_date").IsRequired();
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Reference).HasColumnName("reference");
            entity.Property(e => e.Credit).HasColumnName("credit").HasDefaultValue(0);
            entity.Property(e => e.Debit).HasColumnName("debit").HasDefaultValue(0);
            entity.Property(e => e.Balance).HasColumnName("balance");
            entity.Property(e => e.IsReconciled).HasColumnName("is_reconciled").HasDefaultValue(false);
            entity.Property(e => e.ReconciledWithId).HasColumnName("reconciled_with_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.HasOne(e => e.BankStatement).WithMany(s => s.Lines).HasForeignKey(e => e.BankStatementId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.ReconciledWith).WithMany().HasForeignKey(e => e.ReconciledWithId);
        });

        modelBuilder.Entity<ExpectedCollection>(entity =>
        {
            entity.ToTable("expected_collections");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.SubsidiaryId).HasColumnName("subsidiary_id").IsRequired();
            entity.Property(e => e.CustomerName).HasColumnName("customer_name").IsRequired();
            entity.Property(e => e.Amount).HasColumnName("amount").IsRequired();
            entity.Property(e => e.ExpectedDate).HasColumnName("expected_date").IsRequired();
            entity.Property(e => e.ActualDate).HasColumnName("actual_date");
            entity.Property(e => e.Status).HasColumnName("status").HasDefaultValue("pending");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.HasOne(e => e.Subsidiary).WithMany().HasForeignKey(e => e.SubsidiaryId);
            entity.HasOne(e => e.Creator).WithMany().HasForeignKey(e => e.CreatedBy);
        });

        modelBuilder.Entity<PaymentRequest>(entity =>
        {
            entity.ToTable("payment_requests");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.SubsidiaryId).HasColumnName("subsidiary_id").IsRequired();
            entity.Property(e => e.VendorName).HasColumnName("vendor_name").IsRequired();
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Amount).HasColumnName("amount").IsRequired();
            entity.Property(e => e.Currency).HasColumnName("currency").HasDefaultValue("CLP");
            entity.Property(e => e.RequestDate).HasColumnName("request_date").IsRequired();
            entity.Property(e => e.DueDate).HasColumnName("due_date");
            entity.Property(e => e.Status).HasColumnName("status").HasDefaultValue("pending");
            entity.Property(e => e.RejectionReason).HasColumnName("rejection_reason");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.ApprovedBy).HasColumnName("approved_by");
            entity.Property(e => e.ApprovedAt).HasColumnName("approved_at");
            entity.Property(e => e.PaymentProofPath).HasColumnName("payment_proof_path");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.HasOne(e => e.Subsidiary).WithMany().HasForeignKey(e => e.SubsidiaryId);
            entity.HasOne(e => e.Creator).WithMany().HasForeignKey(e => e.CreatedBy);
            entity.HasOne(e => e.Approver).WithMany().HasForeignKey(e => e.ApprovedBy);
        });

        modelBuilder.Entity<ApprovalHistory>(entity =>
        {
            entity.ToTable("approval_history");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.PaymentRequestId).HasColumnName("payment_request_id").IsRequired();
            entity.Property(e => e.ApproverId).HasColumnName("approver_id").IsRequired();
            entity.Property(e => e.Action).HasColumnName("action").IsRequired();
            entity.Property(e => e.Comments).HasColumnName("comments");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.HasOne(e => e.PaymentRequest).WithMany(p => p.ApprovalHistories).HasForeignKey(e => e.PaymentRequestId);
            entity.HasOne(e => e.Approver).WithMany().HasForeignKey(e => e.ApproverId);
        });

        modelBuilder.Entity<CashFlowProjection>(entity =>
        {
            entity.ToTable("cash_flow_projections");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.SubsidiaryId).HasColumnName("subsidiary_id").IsRequired();
            entity.Property(e => e.ProjectionDate).HasColumnName("projection_date").IsRequired();
            entity.Property(e => e.ProjectionDays).HasColumnName("projection_days").IsRequired();
            entity.Property(e => e.ProjectedInflow).HasColumnName("projected_inflow").HasDefaultValue(0);
            entity.Property(e => e.ProjectedOutflow).HasColumnName("projected_outflow").HasDefaultValue(0);
            entity.Property(e => e.ProjectedBalance).HasColumnName("projected_balance").IsRequired();
            entity.Property(e => e.CalculatedAt).HasColumnName("calculated_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.HasOne(e => e.Subsidiary).WithMany().HasForeignKey(e => e.SubsidiaryId);
            entity.HasIndex(e => new { e.SubsidiaryId, e.ProjectionDate, e.ProjectionDays }).IsUnique();
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.ToTable("audit_logs");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Action).HasColumnName("action").IsRequired();
            entity.Property(e => e.EntityType).HasColumnName("entity_type").IsRequired();
            entity.Property(e => e.EntityId).HasColumnName("entity_id").IsRequired();
            entity.Property(e => e.OldValues).HasColumnName("old_values").HasColumnType("jsonb");
            entity.Property(e => e.NewValues).HasColumnName("new_values").HasColumnType("jsonb");
            entity.Property(e => e.IpAddress).HasColumnName("ip_address");
            entity.Property(e => e.UserAgent).HasColumnName("user_agent");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId);
        });
    }
}
