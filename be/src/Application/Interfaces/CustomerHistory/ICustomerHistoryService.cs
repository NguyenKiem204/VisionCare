using VisionCare.Application.DTOs.Common;
using VisionCare.Application.DTOs.CustomerHistory;

namespace VisionCare.Application.Interfaces.CustomerHistory;

public interface ICustomerHistoryService
{
    Task<PagedResult<CustomerBookingHistoryDto>> GetBookingsAsync(
        int accountId,
        CustomerBookingHistoryQuery query
    );

    Task<IEnumerable<CustomerPrescriptionHistoryDto>> GetPrescriptionsAsync(
        int accountId,
        int? encounterId = null
    );

    Task<IEnumerable<CustomerMedicalHistoryDto>> GetMedicalHistoryAsync(int accountId);
}

