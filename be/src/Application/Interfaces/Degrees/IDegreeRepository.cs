using VisionCare.Domain.Entities;

namespace VisionCare.Application.Interfaces.Degrees;

public interface IDegreeRepository
{
    Task<IEnumerable<Degree>> GetAllAsync();
    Task<Degree?> GetByIdAsync(int id);
    Task<Degree> AddAsync(Degree degree);
    Task UpdateAsync(Degree degree);
    Task DeleteAsync(int id);
}
