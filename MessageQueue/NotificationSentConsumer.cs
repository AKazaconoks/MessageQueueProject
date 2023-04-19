using MassTransit;
using MessageQueueProject.Data;
using Microsoft.EntityFrameworkCore;

namespace MessageQueueProject.MessageQueue;

public class NotificationSentConsumer : IConsumer<INotificationSent>
{
    private readonly ApplicationDbContext _context;

    public NotificationSentConsumer(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<INotificationSent> context)
    {
        await SendNotificationAsync(context.Message.NotificationId);
        await UpdateNotificationAsync(context.Message.NotificationId, "Sent");
    }

    private async Task SendNotificationAsync(int notificationId)
    {
        await Task.Delay(1000);
    }

    private async Task UpdateNotificationAsync(int notificationId, string status)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification != null)
        {
            notification.Status = status;
            _context.Entry(notification).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}