using System;
using System.ComponentModel.DataAnnotations;
using IT2030FinalProject.Models;

namespace IT2030FinalProject.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public string DisplayName { get; set; } = string.Empty;

        public DateTime PostedDate { get; set; }

        public int? CharacterId { get; set; }
        public virtual Character? Character { get; set; }

        public int? OrganizationId { get; set; }
        public virtual Organization? Organization { get; set; }
    }
}