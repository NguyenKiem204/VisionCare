using Microsoft.AspNetCore.SignalR;

namespace VisionCare.WebAPI.Hubs;

public class BookingHub : Hub
{

    public async Task JoinSlotsGroup(int doctorId, string date)
    {
        var groupName = $"slots:{doctorId}:{date}";
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("UserJoined", new { doctorId, date });
    }

    public async Task LeaveSlotsGroup(int doctorId, string date)
    {
        var groupName = $"slots:{doctorId}:{date}";
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task JoinAdminGroup()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "admin:bookings");
    }

    public async Task LeaveAdminGroup()
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "admin:bookings");
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}
