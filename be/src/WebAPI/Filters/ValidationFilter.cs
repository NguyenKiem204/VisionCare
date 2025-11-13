using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Filters;

public class ValidationFilter : IActionFilter
{
    private readonly ILogger<ValidationFilter> _logger;

    public ValidationFilter(ILogger<ValidationFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context
                .ModelState.Where(x => x.Value != null && x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage ?? e.Exception?.Message ?? "Invalid value").ToArray()
                );

            // Log detailed errors for debugging
            foreach (var error in errors)
            {
                _logger.LogWarning("Validation error for {Key}: {Errors}", error.Key, string.Join(", ", error.Value));
            }

            var response = ApiResponse<object>.Fail("Validation failed", errors);
            context.Result = new BadRequestObjectResult(response);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
