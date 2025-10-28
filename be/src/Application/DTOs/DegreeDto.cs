namespace VisionCare.Application.DTOs.DegreeDto;

public class DegreeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
}

public class CreateDegreeRequest
{
    public string Name { get; set; } = string.Empty;
}

public class UpdateDegreeRequest
{
    public string Name { get; set; } = string.Empty;
}
