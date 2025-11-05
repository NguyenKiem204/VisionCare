using System.Text.RegularExpressions;

namespace VisionCare.Domain.Services;

public static class AppointmentCodeGenerator
{
    public static string Generate(DateOnly date)
    {
        var datePart = date.ToString("yyyyMMdd");
        var randomPart = Random.Shared.Next(100000, 999999).ToString("D6");
        return $"VC-{datePart}-{randomPart}";
    }
    
    public static bool Validate(string code)
    {
        // Format: VC-YYYYMMDD-XXXXXX
        return Regex.IsMatch(code, @"^VC-\d{8}-\d{6}$");
    }
}
