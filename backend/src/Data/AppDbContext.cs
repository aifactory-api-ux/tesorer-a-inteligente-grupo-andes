using Microsoft.EntityFrameworkCore;
using Backend.Models.Entities;
using Backend.Models.Enums;

namespace Backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Subsidiary> Subsidiaries => Set<Subsidiary>();
    public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
    public DbSet<BankStatement> BankStatements => Set<BankStatement>();
    public DbSet<BankStatementLine> BankStatementLines => Set<BankStatementLine>();
    public DbSet<ExpectedCollection> ExpectedCollections => Set<ExpectedCollection>();
    public DbSet<PaymentRequest> PaymentRequests => Set<PaymentRequest>();
    public DbSet<ApprovalHistory> ApprovalHistories => Set<ApprovalHistory>();
    public DbSet<CashFlowProjection> CashFlowProjections => Set<CashFlowProjection>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.AzureAdObjectId).IsUnique();
            entity.HasIndex(e => e.Email);
            entity.Property(e => e.Role).HasConversion<string>();
        });

        modelBuilder.Entity<Subsidiary>(entity =>
        {
            entity.HasIndex(e => e.Code).IsUnique();
        });

        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.Property(e => e.AccountType).HasConversion<string>();
            entity.HasOne(e => e.Subsidiary)
                .WithMany(s => s.BankAccounts)
                .HasForeignKey(e => e.SubsidiaryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<BankStatement>(entity =>
        {
            entity.HasIndex(e => new { e.BankAccountId, e.StatementDate }).IsUnique();
            entity.Property(e => e.ImportStatus).HasConversion<string>();
            entity.HasOne(e => e.BankAccount)
                .WithMany(b => b.BankStatements)
                .HasForeignKey(e => e.BankAccountId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Creator)
                .WithMany()
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<BankStatementLine>(entity =>
        {
            entity.HasOne(e => e.BankStatement)
                .WithMany(s => s.Lines)
                .HasForeignKey(e => e.BankStatementId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.ReconciledWith)
                .WithMany()
                .HasForeignKey(e => e.ReconciledWithId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<ExpectedCollection>(entity =>
        {
            entity.Property(e => e.Status).HasConversion<string>();
            entity.HasOne(e => e.Subsidiary)
                .WithMany(s => s.ExpectedCollections)
                .HasForeignKey(e => e.SubsidiaryId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Creator)
                .WithMany()
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<PaymentRequest>(entity =>
        {
            entity.Property(e => e.Status).HasConversion<string>();
            entity.HasOne(e => e.Subsidiary)
                .WithMany(s => s.PaymentRequests)
                .HasForeignKey(e => e.SubsidiaryId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Creator)
                .WithMany()
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(e => e.Approver)
                .WithMany()
                .HasForeignKey(e => e.ApprovedBy)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<ApprovalHistory>(entity =>
        {
            entity.Property(e => e.Action).HasConversion<string>();
            entity.HasOne(e => e.PaymentRequest)
                .WithMany(p => p.ApprovalHistories)
                .HasForeignKey(e => e.PaymentRequestId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Approver)
                .WithMany()
                .HasForeignKey(e => e.ApproverId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CashFlowProjection>(entity =>
        {
            entity.Property(e => e.ProjectionDays).HasConversion<string>();
            entity.HasOne(e => e.Subsidiary)
                .WithMany(s => s.CashFlowProjections)
                .HasForeignKey(e => e.SubsidiaryId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasIndex(e => e.EntityType);
            entity.HasIndex(e => e.EntityId);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => e.UserId);
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
