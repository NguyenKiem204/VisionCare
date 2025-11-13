using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;

namespace VisionCare.Infrastructure.Repositories;

public class StaffRepository : IStaffRepository
{
    private readonly VisionCareDbContext _context;

    public StaffRepository(VisionCareDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Staff>> GetAllAsync()
    {
        var staff = await _context.Staff.Include(s => s.Account).ToListAsync();

        return staff.Select(MapToDomain);
    }

    public async Task<Staff?> GetByIdAsync(int id)
    {
        var staff = await _context
            .Staff.Include(s => s.Account)
            .FirstOrDefaultAsync(s => s.AccountId == id);

        return staff != null ? MapToDomain(staff) : null;
    }

    public async Task<Staff?> GetByAccountIdAsync(int accountId)
    {
        var staff = await _context
            .Staff.Include(s => s.Account)
            .FirstOrDefaultAsync(s => s.AccountId == accountId);

        return staff != null ? MapToDomain(staff) : null;
    }

    public async Task<Staff> AddAsync(Staff staff)
    {
        var staffModel = MapToModel(staff);
        _context.Staff.Add(staffModel);
        await _context.SaveChangesAsync();
        return MapToDomain(staffModel);
    }

    public async Task UpdateAsync(Staff staff)
    {
        var existingStaff = await _context.Staff.FirstOrDefaultAsync(s => s.AccountId == staff.Id);

        if (existingStaff != null)
        {
            existingStaff.FullName = staff.StaffName ?? existingStaff.FullName;
            existingStaff.Gender = staff.Gender ?? existingStaff.Gender;
            existingStaff.Dob = staff.Dob ?? existingStaff.Dob;
            existingStaff.Address = staff.Address ?? existingStaff.Address;
            existingStaff.Phone = staff.Phone ?? existingStaff.Phone;
            existingStaff.Avatar = staff.Avatar ?? existingStaff.Avatar;

            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var staff = await _context.Staff.FindAsync(id);
        if (staff != null)
        {
            _context.Staff.Remove(staff);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Staff>> SearchAsync(string keyword, string? gender)
    {
        var query = _context.Staff.Include(s => s.Account).AsQueryable();

        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(s => s.FullName.Contains(keyword));
        }

        if (!string.IsNullOrEmpty(gender))
        {
            query = query.Where(s => s.Gender == gender);
        }

        var staff = await query.ToListAsync();
        return staff.Select(MapToDomain);
    }

    public async Task<IEnumerable<Staff>> GetByGenderAsync(string gender)
    {
        var staff = await _context
            .Staff.Include(s => s.Account)
            .Where(s => s.Gender == gender)
            .ToListAsync();

        return staff.Select(MapToDomain);
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Staff.CountAsync();
    }

    public async Task<Dictionary<string, int>> GetGenderStatsAsync()
    {
        return await _context
            .Staff.GroupBy(s => s.Gender)
            .ToDictionaryAsync(g => g.Key ?? "Unknown", g => g.Count());
    }

    private static Staff MapToDomain(VisionCare.Infrastructure.Models.Staff model)
    {
        return new Staff
        {
            Id = model.AccountId,
            AccountId = model.AccountId,
            StaffName = model.FullName,
            Gender = model.Gender,
            Dob = model.Dob,
            Address = model.Address,
            Phone = model.Phone,
            Avatar = model.Avatar,
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow,
            Account =
                model.Account != null
                    ? new User
                    {
                        Id = model.Account.AccountId,
                        Email = model.Account.Email,
                        Username = model.Account.Username,
                    }
                    : null,
        };
    }

    private static VisionCare.Infrastructure.Models.Staff MapToModel(Staff domain)
    {
        return new VisionCare.Infrastructure.Models.Staff
        {
            AccountId = domain.AccountId ?? 0,
            FullName = domain.StaffName ?? string.Empty,
            Gender = domain.Gender,
            Dob = domain.Dob,
            Address = domain.Address,
            Phone = domain.Phone,
            Avatar = domain.Avatar,
        };
    }
}
