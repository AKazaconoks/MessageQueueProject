using MassTransit;
using MessageQueueProject.Data;
using MessageQueueProject.MessageQueue;
using MessageQueueProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MessageQueueProject.Controllers;

[Authorize]
[ApiController]
[Route("api/private/[controller]")]
public class PrivateController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IBus _bus;

    public PrivateController(ApplicationDbContext context, IBus bus)
    {
        _context = context;
        _bus = bus;
    }

    [HttpGet("clients")]
    public async Task<ActionResult<IEnumerable<Client>>> GetClients(int pageNumber = 1, int pageSize = 10)
    {
        var query = _context.Clients
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        return await query.ToListAsync();
    }

    [HttpGet("clients/{id}")]
    public async Task<ActionResult<Client>> GetClient(int id)
    {
        var client = await _context.Clients.FindAsync(id);

        return client == null ? NotFound() : client;
    }

    [HttpGet("{notifications}")]
    public async Task<ActionResult<IEnumerable<Notifications>>> GetNotifications(int clientId = 0, int pageNumber = 1,
        int pageSize = 10)
    {
        var query = _context.Notifications
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
        if (clientId != 0)
        {
            query = query.Where(x => x.ClientId == clientId);
        }

        return await query.ToListAsync();
    }

    [HttpGet("notifications/{id}")]
    public async Task<ActionResult<Notifications>> GetNotification(int id)
    {
        var notification = await _context.Notifications.FindAsync(id);

        return notification == null ? NotFound() : notification;
    }

    [HttpPost("notifications")]
    public async Task<ActionResult<IEnumerable<Notifications>>> AddNotifications(
        [FromBody] IEnumerable<NotificationsCreateRequest> notificationsRequest)
    {
        var notifications = new List<Notifications>();
        foreach (var request in notificationsRequest)
        {
            if (request.Channel == "sms" && request.Content.Length > 140)
            {
                return BadRequest("SMS content should not contain more than 140 characters");
            }

            var clientExists = await _context.Clients.AnyAsync(x => x.Id == request.ClientId);
            if (!clientExists)
            {
                return BadRequest($"Client with ID ${request.ClientId} doesn't exist");
            }

            notifications.Add(new Notifications
            {
                Content = request.Content,
                ClientId = request.ClientId,
                Channel = request.Channel,
                Status = "Created"
            });
        }

        _context.Notifications.AddRange(notifications);
        await _context.SaveChangesAsync();

        var firstNotificationId = notifications.First().Id;
        await _bus.Publish<INotificationSent>(new { NotificationId = firstNotificationId });

        return CreatedAtAction(nameof(GetNotification), new { id = firstNotificationId }, notifications);
    }
}