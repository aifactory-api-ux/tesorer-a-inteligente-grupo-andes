using Backend.DTOs;
using Backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class SubsidiaryService : ISubsidiaryService
{
    private readonly Data.AppDbContext _context;

    public SubsidiaryService(Data.AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<SubsidiaryDto>> GetAllSubsidiariesAsync()
    {
        var subsidiaries = await _context.Subsidiaries
            .OrderBy(s => s.Name)
            .ToListAsync();

        return subsidiaries.Select(MapToDto).ToList();
    }

    public async Task<List<SubsidiaryDto>> GetActiveSubsidiariesAsync()
    {
        var subsidiaries = await _context.Subsidiaries
            .Where(s => s.IsActive)
            .OrderBy(s => s.Name)
            .ToListAsync();

        return subsidiaries.Select(MapToDto).ToList();
    }

    public async Task<SubsidiaryDto?> GetSubsidiaryByIdAsync(Guid id)
    {
        var subsidiary = await _context.Subsidiaries.FindAsync(id);
        return subsidiary == null ? null : MapToDto(subsidiary);
    }

    public async Task<SubsidiaryDto> CreateSubsidiaryAsync(SubsidiaryCreateDto dto)
    {
        var subsidiary = new Subsidiary
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Code = dto.Code,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Subsidiaries.Add(subsidiary);
        await _context.SaveChangesAsync();
        return MapToDto(subsidiary);
    }

    public async Task<SubsidiaryDto?> UpdateSubsidiaryAsync(Guid id, SubsidiaryUpdateDto dto)
    {
        var subsidiary = await _context.Subsidiaries.FindAsync(id);
        if (subsidiary == null)
            return null;

        if (dto.Name != null)
            subsidiary.Name = dto.Name;
        if (dto.IsActive.HasValue)
            subsidiary.IsActive = dto.IsActive.Value;

        await _context.SaveChangesAsync();
        return MapToDto(subsidiary);
    }

    public async Task<bool> DeactivateSubsidiaryAsync(Guid id)
    {
        var subsidiary = await _context.Subsidiaries.FindAsync(id);
        if (subsidiary == null)
            return false;

        subsidiary.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ActivateSubsidiaryAsync(Guid id)
    {
        var subsidiary = await _context.Subsidiaries.FindAsync(id);
        if (subsidiary == null)
            return false;

        subsidiary.IsActive = true;
        await _context.SaveChangesAsync();
        return true;
    }

    private static SubsidiaryDto MapToDto(Subsidiary subsidiary)
    {
        return new SubsidiaryDto(
            subsidiary.Id,
            subsidiary.Name,
            subsidiary.Code,
            subsidiary.IsActive,
            subsidiary.CreatedAt
        );
    }
}
