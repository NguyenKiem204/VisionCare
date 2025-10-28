using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.DTOs.FeedbackDto;
using VisionCare.Application.Interfaces.Feedback;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeedbackController : ControllerBase
{
    private readonly IFeedbackService _feedbackService;

    public FeedbackController(IFeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }

    #region Doctor Feedback Management
    [HttpGet("doctors")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetAllDoctorFeedbacks()
    {
        var feedbacks = await _feedbackService.GetAllDoctorFeedbacksAsync();
        return Ok(ApiResponse<IEnumerable<FeedbackDoctorDto>>.Ok(feedbacks));
    }

    [HttpGet("doctors/{id}")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetDoctorFeedbackById(int id)
    {
        var feedback = await _feedbackService.GetDoctorFeedbackByIdAsync(id);
        if (feedback == null)
        {
            return NotFound(
                ApiResponse<FeedbackDoctorDto>.Fail($"Doctor feedback with ID {id} not found.")
            );
        }
        return Ok(ApiResponse<FeedbackDoctorDto>.Ok(feedback));
    }

    [HttpGet("doctors/appointment/{appointmentId}")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetDoctorFeedbackByAppointmentId(int appointmentId)
    {
        var feedback = await _feedbackService.GetDoctorFeedbackByAppointmentIdAsync(appointmentId);
        if (feedback == null)
        {
            return NotFound(
                ApiResponse<FeedbackDoctorDto>.Fail(
                    $"Doctor feedback for appointment {appointmentId} not found."
                )
            );
        }
        return Ok(ApiResponse<FeedbackDoctorDto>.Ok(feedback));
    }

    [HttpGet("doctors/doctor/{doctorId}")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetDoctorFeedbacksByDoctorId(int doctorId)
    {
        var feedbacks = await _feedbackService.GetDoctorFeedbacksByDoctorIdAsync(doctorId);
        return Ok(ApiResponse<IEnumerable<FeedbackDoctorDto>>.Ok(feedbacks));
    }

    [HttpGet("doctors/patient/{patientId}")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetDoctorFeedbacksByPatientId(int patientId)
    {
        var feedbacks = await _feedbackService.GetDoctorFeedbacksByPatientIdAsync(patientId);
        return Ok(ApiResponse<IEnumerable<FeedbackDoctorDto>>.Ok(feedbacks));
    }

    [HttpPost("doctors")]
    [Authorize(Policy = "PatientOrAdmin")]
    public async Task<IActionResult> CreateDoctorFeedback(
        [FromBody] CreateFeedbackDoctorRequest request
    )
    {
        var feedback = await _feedbackService.CreateDoctorFeedbackAsync(request);
        return CreatedAtAction(
            nameof(GetDoctorFeedbackById),
            new { id = feedback.Id },
            ApiResponse<FeedbackDoctorDto>.Ok(feedback)
        );
    }

    [HttpPut("doctors/{id}")]
    [Authorize(Policy = "PatientOrAdmin")]
    public async Task<IActionResult> UpdateDoctorFeedback(
        int id,
        [FromBody] UpdateFeedbackDoctorRequest request
    )
    {
        var feedback = await _feedbackService.UpdateDoctorFeedbackAsync(id, request);
        return Ok(ApiResponse<FeedbackDoctorDto>.Ok(feedback));
    }

    [HttpPost("doctors/{id}/respond")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> RespondToDoctorFeedback(
        int id,
        [FromBody] RespondToFeedbackRequest request
    )
    {
        var feedback = await _feedbackService.RespondToDoctorFeedbackAsync(id, request);
        return Ok(ApiResponse<FeedbackDoctorDto>.Ok(feedback));
    }

    [HttpDelete("doctors/{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteDoctorFeedback(int id)
    {
        var result = await _feedbackService.DeleteDoctorFeedbackAsync(id);
        if (!result)
        {
            return NotFound(
                ApiResponse<FeedbackDoctorDto>.Fail($"Doctor feedback with ID {id} not found.")
            );
        }
        return Ok(ApiResponse<FeedbackDoctorDto>.Ok(null, "Doctor feedback deleted successfully"));
    }

    #endregion

    #region Service Feedback Management
    [HttpGet("services")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetAllServiceFeedbacks()
    {
        var feedbacks = await _feedbackService.GetAllServiceFeedbacksAsync();
        return Ok(ApiResponse<IEnumerable<FeedbackServiceDto>>.Ok(feedbacks));
    }

    [HttpGet("services/{id}")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetServiceFeedbackById(int id)
    {
        var feedback = await _feedbackService.GetServiceFeedbackByIdAsync(id);
        if (feedback == null)
        {
            return NotFound(
                ApiResponse<FeedbackServiceDto>.Fail($"Service feedback with ID {id} not found.")
            );
        }
        return Ok(ApiResponse<FeedbackServiceDto>.Ok(feedback));
    }

    [HttpGet("services/appointment/{appointmentId}")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetServiceFeedbackByAppointmentId(int appointmentId)
    {
        var feedback = await _feedbackService.GetServiceFeedbackByAppointmentIdAsync(appointmentId);
        if (feedback == null)
        {
            return NotFound(
                ApiResponse<FeedbackServiceDto>.Fail(
                    $"Service feedback for appointment {appointmentId} not found."
                )
            );
        }
        return Ok(ApiResponse<FeedbackServiceDto>.Ok(feedback));
    }

    [HttpGet("services/service/{serviceId}")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetServiceFeedbacksByServiceId(int serviceId)
    {
        var feedbacks = await _feedbackService.GetServiceFeedbacksByServiceIdAsync(serviceId);
        return Ok(ApiResponse<IEnumerable<FeedbackServiceDto>>.Ok(feedbacks));
    }

    [HttpGet("services/patient/{patientId}")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetServiceFeedbacksByPatientId(int patientId)
    {
        var feedbacks = await _feedbackService.GetServiceFeedbacksByPatientIdAsync(patientId);
        return Ok(ApiResponse<IEnumerable<FeedbackServiceDto>>.Ok(feedbacks));
    }

    [HttpPost("services")]
    [Authorize(Policy = "PatientOrAdmin")]
    public async Task<IActionResult> CreateServiceFeedback(
        [FromBody] CreateFeedbackServiceRequest request
    )
    {
        var feedback = await _feedbackService.CreateServiceFeedbackAsync(request);
        return CreatedAtAction(
            nameof(GetServiceFeedbackById),
            new { id = feedback.Id },
            ApiResponse<FeedbackServiceDto>.Ok(feedback)
        );
    }

    [HttpPut("services/{id}")]
    [Authorize(Policy = "PatientOrAdmin")]
    public async Task<IActionResult> UpdateServiceFeedback(
        int id,
        [FromBody] UpdateFeedbackServiceRequest request
    )
    {
        var feedback = await _feedbackService.UpdateServiceFeedbackAsync(id, request);
        return Ok(ApiResponse<FeedbackServiceDto>.Ok(feedback));
    }

    [HttpPost("services/{id}/respond")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> RespondToServiceFeedback(
        int id,
        [FromBody] RespondToServiceFeedbackRequest request
    )
    {
        var feedback = await _feedbackService.RespondToServiceFeedbackAsync(id, request);
        return Ok(ApiResponse<FeedbackServiceDto>.Ok(feedback));
    }

    [HttpDelete("services/{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteServiceFeedback(int id)
    {
        var result = await _feedbackService.DeleteServiceFeedbackAsync(id);
        if (!result)
        {
            return NotFound(
                ApiResponse<FeedbackServiceDto>.Fail($"Service feedback with ID {id} not found.")
            );
        }
        return Ok(
            ApiResponse<FeedbackServiceDto>.Ok(null, "Service feedback deleted successfully")
        );
    }

    #endregion

    #region Analytics and Search
    [HttpPost("doctors/search")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> SearchDoctorFeedbacks([FromBody] FeedbackSearchRequest request)
    {
        var feedbacks = await _feedbackService.SearchDoctorFeedbacksAsync(request);
        return Ok(ApiResponse<IEnumerable<FeedbackDoctorDto>>.Ok(feedbacks));
    }

    [HttpPost("services/search")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> SearchServiceFeedbacks(
        [FromBody] ServiceFeedbackSearchRequest request
    )
    {
        var feedbacks = await _feedbackService.SearchServiceFeedbacksAsync(request);
        return Ok(ApiResponse<IEnumerable<FeedbackServiceDto>>.Ok(feedbacks));
    }

    [HttpGet("doctors/{doctorId}/average-rating")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetDoctorAverageRating(int doctorId)
    {
        var averageRating = await _feedbackService.GetDoctorAverageRatingAsync(doctorId);
        return Ok(ApiResponse<double>.Ok(averageRating));
    }

    [HttpGet("services/{serviceId}/average-rating")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetServiceAverageRating(int serviceId)
    {
        var averageRating = await _feedbackService.GetServiceAverageRatingAsync(serviceId);
        return Ok(ApiResponse<double>.Ok(averageRating));
    }

    [HttpGet("doctors/unresponded")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetUnrespondedDoctorFeedbacks()
    {
        var feedbacks = await _feedbackService.GetUnrespondedDoctorFeedbacksAsync();
        return Ok(ApiResponse<IEnumerable<FeedbackDoctorDto>>.Ok(feedbacks));
    }

    [HttpGet("services/unresponded")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetUnrespondedServiceFeedbacks()
    {
        var feedbacks = await _feedbackService.GetUnrespondedServiceFeedbacksAsync();
        return Ok(ApiResponse<IEnumerable<FeedbackServiceDto>>.Ok(feedbacks));
    }

    [HttpGet("doctors/count")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetTotalDoctorFeedbacksCount()
    {
        var count = await _feedbackService.GetTotalDoctorFeedbacksCountAsync();
        return Ok(ApiResponse<int>.Ok(count));
    }

    [HttpGet("services/count")]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetTotalServiceFeedbacksCount()
    {
        var count = await _feedbackService.GetTotalServiceFeedbacksCountAsync();
        return Ok(ApiResponse<int>.Ok(count));
    }

    #endregion
}
