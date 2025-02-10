using System.ComponentModel.DataAnnotations;

namespace Application.Domain.Entities;

public class Customer
{
    public required string UserId { get; set; }
    public required string ContactId { get; set; }
    public required string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}