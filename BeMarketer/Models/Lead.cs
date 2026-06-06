using BeMarketer.Models;
using System.ComponentModel.DataAnnotations;

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

        [Required, StringLength(20)]
        public string Name { get; set; }

        [Required, StringLength(64)]
        public string Email { get; set; }

        [Required, StringLength(20)]
        public string Phone { get; set; }

        [Required, StringLength(64)]
        public string Address { get; set; }

        [Required, StringLength(255)]
        public string Description { get; set; }

        public LeadStatus Status { get => status; set => status = value; }
        public DateTime CreatedAt { get; set; }
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
