using MessageQueueProject.Models;
using Microsoft.EntityFrameworkCore;

namespace MessageQueueProject.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Client> Clients { get; set; }
    public DbSet<Notifications> Notifications { get; set; }
}