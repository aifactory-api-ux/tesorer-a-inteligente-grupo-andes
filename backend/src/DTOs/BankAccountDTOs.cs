using Backend.Models.Enums;

namespace Backend.DTOs;

public record BankAccountDto(
    Guid Id,
    Guid SubsidiaryId,
    string? SubsidiaryName,
    string BankName,
    string AccountNumber,
    AccountType AccountType,
    string Currency,
    bool IsActive,
    DateTime CreatedAt
);

public record BankAccountCreateDto(
    Guid SubsidiaryId,
    string BankName,
    string AccountNumber,
    AccountType AccountType,
    string Currency = "CLP"
);

public record BankAccountUpdateDto(
    string? BankName,
    string? AccountNumber,
    AccountType? AccountType,
    string? Currency,
    bool? IsActive
);
