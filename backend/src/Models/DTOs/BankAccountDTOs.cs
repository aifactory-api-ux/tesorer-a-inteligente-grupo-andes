namespace Api.Models.DTOs;

public class BankAccountCreateDto
{
    public Guid SubsidiaryId { get; set; }
    public string BankName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string AccountType { get; set; } = string.Empty;
    public string Currency { get; set; } = "CLP";
}

public class BankAccountUpdateDto
{
    public string? BankName { get; set; }
    public string? AccountNumber { get; set; }
    public string? AccountType { get; set; }
    public string? Currency { get; set; }
    public bool? IsActive { get; set; }
}

public class BankAccountResponseDto
{
    public Guid Id { get; set; }
    public Guid SubsidiaryId { get; set; }
    public string BankName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string AccountType { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
