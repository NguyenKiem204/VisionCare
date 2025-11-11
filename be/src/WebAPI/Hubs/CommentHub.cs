using Microsoft.AspNetCore.SignalR;

namespace VisionCare.WebAPI.Hubs;

public class CommentHub : Hub
{
    // Groups structure:
    // - "blog:{blogId}" - Users đang xem blog và muốn nhận comment real-time

    public async Task JoinBlogGroup(int blogId)
    {
        var groupName = $"blog:{blogId}";
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveBlogGroup(int blogId)
    {
        var groupName = $"blog:{blogId}";
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // Auto leave all groups khi disconnect
        await base.OnDisconnectedAsync(exception);
    }
}


