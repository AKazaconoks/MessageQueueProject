using System.ComponentModel.DataAnnotations;

namespace MessageQueueProject.Models;

public class ClientCreateRequest
{
    [Required, MinLength(2), MaxLength(32), RegularExpression(@"^[a-zA-Z]+$")]
    public string FirstName { get; set; }

    [Required, MinLength(2), MaxLength(32), RegularExpression(@"^[a-zA-Z]+$")]
    public string LastName { get; set; }

    [Required, EmailAddress] public string Email { get; set; }

    [Required, Phone, RegularExpression(@"^\+\d{1,4}\d{7,14}$")]
    public string PhoneNumber { get; set; }
}