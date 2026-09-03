namespace Api.Models.DTOs;

public class SubsidiaryCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}

public class SubsidiaryUpdateDto
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public bool? IsActive { get; set; }
}

public class SubsidiaryResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
