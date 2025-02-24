using System.ComponentModel.DataAnnotations;

namespace Application.Domain.Entities;

public class Customer
{
    public string? UserId { get; set; }
    public required Guid ContactId { get; set; }
    public required string Email { get; set; }
    public string? Salutation { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}