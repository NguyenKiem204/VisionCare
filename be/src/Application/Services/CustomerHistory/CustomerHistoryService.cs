using VisionCare.Application.DTOs.Common;
using VisionCare.Application.DTOs.CustomerHistory;
using VisionCare.Application.Interfaces.CustomerHistory;

namespace VisionCare.Application.Services.CustomerHistory;

public class CustomerHistoryService : ICustomerHistoryService
{
    private readonly ICustomerHistoryReadRepository _repository;

    public CustomerHistoryService(ICustomerHistoryReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<CustomerBookingHistoryDto>> GetBookingsAsync(
        int accountId,
        CustomerBookingHistoryQuery query
    )
    {
        query ??= new CustomerBookingHistoryQuery();

        query.Page = query.Page <= 0 ? 1 : query.Page;
        query.PageSize = query.PageSize is <= 0 or > 50 ? 10 : query.PageSize;

        var (items, totalCount) = await _repository.GetBookingsAsync(accountId, query);

        return new PagedResult<CustomerBookingHistoryDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize,
        };
    }

    public Task<IEnumerable<CustomerPrescriptionHistoryDto>> GetPrescriptionsAsync(
        int accountId,
        int? encounterId = null
    )
    {
        return _repository.GetPrescriptionsAsync(accountId, encounterId);
    }

    public Task<IEnumerable<CustomerMedicalHistoryDto>> GetMedicalHistoryAsync(int accountId)
    {
        return _repository.GetMedicalHistoryAsync(accountId);
    }
}

