namespace VisionCare.WebAPI.Utils;

public static class UploadPrefixes
{
    public static string Banner(string place) =>
        $"banners/{(string.IsNullOrWhiteSpace(place) ? "home_hero" : place)}";

    public static string Section(string key) => $"sections/{key}";

    public static string UserAvatar(int accountId) => $"avatars/users/{accountId}";

    public static string DoctorAvatar(int accountId) => $"avatars/doctors/{accountId}";

    public static string StaffAvatar(int accountId) => $"avatars/staff/{accountId}";

    public static string CustomerAvatar(int accountId) => $"avatars/customers/{accountId}";

    public static string Equipment(int equipmentId) => $"equipment/{equipmentId}";

    public static string Service(int serviceId) => $"services/{serviceId}";

    public static string ServiceType(int serviceTypeId) => $"services/types/{serviceTypeId}";

    public static string Blog(int blogId) => $"blogs/{blogId}";

    public static string DoctorCertificate(int doctorId, int certificateId) =>
        $"doctors/{doctorId}/certificates/{certificateId}";

    public static string DoctorDegree(int doctorId, int degreeId) =>
        $"doctors/{doctorId}/degrees/{degreeId}";
}