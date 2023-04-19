using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessageQueueProject.Models;

public class Notifications
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] public int ClientId { get; set; }

    [Required]
    [RegularExpression("(sms|email)")]
    public string Channel { get; set; }

    [Required] public string Content { get; set; }
    public string Status { get; set; }
}