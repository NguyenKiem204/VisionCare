using VisionCare.Application.DTOs.CustomerHistory;

namespace VisionCare.Application.Interfaces.CustomerHistory;

public interface ICustomerHistoryReadRepository
{
    Task<(IEnumerable<CustomerBookingHistoryDto> items, int totalCount)> GetBookingsAsync(
        int accountId,
        CustomerBookingHistoryQuery query
    );

    Task<IEnumerable<CustomerPrescriptionHistoryDto>> GetPrescriptionsAsync(
        int accountId,
        int? encounterId = null
    );

    Task<IEnumerable<CustomerMedicalHistoryDto>> GetMedicalHistoryAsync(int accountId);
}

