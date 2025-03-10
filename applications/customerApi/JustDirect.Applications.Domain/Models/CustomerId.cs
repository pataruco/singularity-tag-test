namespace JustDirect.Applications.Domain.Models
{
    public class CustomerId
    {
        public Guid? Id { get; set; }
        public string? UserId { get; set; }
        public Guid? ContactId { get; set; }

        public bool IsContactId => ContactId.HasValue && ContactId.Value != Guid.Empty;
        public bool IsUserId => !string.IsNullOrEmpty(UserId);
        public bool IsId => Id.HasValue && Id.Value != Guid.Empty;
    }
}