namespace Backend.DTOs;

public record PaginatedResultDto<T>(
    List<T> Items,
    int Total,
    int Page,
    int PageSize
);

public record ApiResponseDto<T>(
    bool Success,
    T? Data,
    string? Error,
    string? Message
);

public record HealthCheckDto(
    string Status,
    string Service,
    string Version
);
