using BeMarketer.Models;

namespace BeMarketer.Models
{
    public enum LeadStatus
    {
        New,
        Contacted,
        Qualified,
        Lost
    }
    public class Lead
    {
        private LeadStatus status = LeadStatus.New;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Description { get; set; } = string.Empty;
        public LeadStatus Status { get => status; set => status = value; }
        public DateTime CreatedAt { get; set; }
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
