using Microsoft.EntityFrameworkCore;
using VisionCare.Application.Interfaces;
using VisionCare.Domain.Entities;
using VisionCare.Infrastructure.Data;
using VisionCare.Infrastructure.Mappings;

namespace VisionCare.Infrastructure.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly VisionCareDbContext _context;

    public DoctorRepository(VisionCareDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Doctor>> GetAllAsync()
    {
        var doctors = await _context
            .Doctors.Include(d => d.Account)
            .Include(d => d.Specialization)
            .ToListAsync();

        return doctors.Select(DoctorMapper.ToDomain);
    }

    public async Task<Doctor?> GetByIdAsync(int id)
    {
        var doctor = await _context
            .Doctors.Include(d => d.Account)
            .Include(d => d.Specialization)
            .FirstOrDefaultAsync(d => d.AccountId == id);

        return doctor != null ? DoctorMapper.ToDomain(doctor) : null;
    }

    public async Task<Doctor> AddAsync(Doctor doctor)
    {
        var doctorModel = DoctorMapper.ToInfrastructure(doctor);
        _context.Doctors.Add(doctorModel);
        await _context.SaveChangesAsync();
        return DoctorMapper.ToDomain(doctorModel);
    }

    public async Task UpdateAsync(Doctor doctor)
    {
        var existingDoctor = await _context.Doctors.FirstOrDefaultAsync(d =>
            d.AccountId == doctor.Id
        );

        if (existingDoctor != null)
        {
            existingDoctor.FullName = doctor.DoctorName;
            existingDoctor.Phone = doctor.Phone;
            existingDoctor.ExperienceYears = doctor.ExperienceYears.HasValue
                ? (short?)doctor.ExperienceYears.Value
                : null;
            existingDoctor.SpecializationId =
                doctor.SpecializationId ?? existingDoctor.SpecializationId;
            // Chỉ update avatar nếu có giá trị mới
            if (!string.IsNullOrWhiteSpace(doctor.ProfileImage))
            {
                existingDoctor.Avatar = doctor.ProfileImage;
            }
            existingDoctor.Gender = doctor.Gender;
            existingDoctor.Dob = doctor.Dob;
            existingDoctor.Address = doctor.Address;
            existingDoctor.Status = doctor.DoctorStatus;
            existingDoctor.Rating = doctor.Rating.HasValue ? (decimal?)doctor.Rating.Value : null;
            existingDoctor.Biography = doctor.Biography;

            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        var doctor = await _context.Doctors.FindAsync(id);
        if (doctor != null)
        {
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Doctor>> GetBySpecializationAsync(int specializationId)
    {
        var doctors = await _context
            .Doctors.Include(d => d.Account)
            .Include(d => d.Specialization)
            .Where(d => d.SpecializationId == specializationId)
            .ToListAsync();

        return doctors.Select(DoctorMapper.ToDomain);
    }

    public async Task<IEnumerable<Doctor>> GetAvailableDoctorsAsync(DateTime date)
    {
        var doctors = await _context
            .Doctors.Include(d => d.Account)
            .Include(d => d.Specialization)
            .Where(d => d.Status == "Active")
            .ToListAsync();

        return doctors.Select(DoctorMapper.ToDomain);
    }

    public async Task<(IEnumerable<Doctor> items, int totalCount)> SearchDoctorsAsync(
        string keyword,
        int? specializationId,
        double? minRating,
        int page = 1,
        int pageSize = 10,
        string sortBy = "id",
        bool desc = false
    )
    {
        var query = _context
            .Doctors.Include(d => d.Account)
            .Include(d => d.Specialization)
            .AsQueryable();

        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(d =>
                (d.FullName != null && d.FullName.Contains(keyword))
                || (d.Account.Email != null && d.Account.Email.Contains(keyword))
                || (d.Specialization != null && d.Specialization.Name != null && d.Specialization.Name.Contains(keyword))
                || (d.Phone != null && d.Phone.Contains(keyword))
            );
        }

        if (specializationId.HasValue)
        {
            query = query.Where(d => d.SpecializationId == specializationId.Value);
        }

        if (minRating.HasValue)
        {
            var min = (decimal)minRating.Value;
            query = query.Where(d => d.Rating >= min);
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync();

        // Apply sorting
        query = sortBy.ToLower() switch
        {
            "fullname" => desc
                ? query.OrderByDescending(d => d.FullName)
                : query.OrderBy(d => d.FullName),
            "email" => desc
                ? query.OrderByDescending(d => d.Account.Email)
                : query.OrderBy(d => d.Account.Email),
            "specializationid" => desc
                ? query.OrderByDescending(d => d.SpecializationId)
                : query.OrderBy(d => d.SpecializationId),
            "rating" => desc
                ? query.OrderByDescending(d => d.Rating)
                : query.OrderBy(d => d.Rating),
            "experience" => desc
                ? query.OrderByDescending(d => d.ExperienceYears)
                : query.OrderBy(d => d.ExperienceYears),
            _ => desc ? query.OrderByDescending(d => d.AccountId) : query.OrderBy(d => d.AccountId),
        };

        // Apply pagination
        var doctors = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return (doctors.Select(DoctorMapper.ToDomain), totalCount);
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Doctors.CountAsync();
    }

    public async Task<double> GetAverageRatingAsync()
    {
        var avg = await _context.Doctors.AverageAsync(d => (double?)d.Rating);
        return avg ?? 0.0;
    }

    public async Task<IEnumerable<Doctor>> GetTopRatedDoctorsAsync(int count)
    {
        var doctors = await _context
            .Doctors.Include(d => d.Account)
            .Include(d => d.Specialization)
            .OrderByDescending(d => d.Rating)
            .Take(count)
            .ToListAsync();

        return doctors.Select(DoctorMapper.ToDomain);
    }

    // Mapping moved to DoctorMapper
}
