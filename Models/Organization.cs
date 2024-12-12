using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IT2030FinalProject.Models
{
   public class Organization
   {
       public int Id { get; set; }

       [Required]
       [StringLength(200)]
       public string Name { get; set; } = string.Empty;

       [Required]
       [StringLength(2000)]
       public string Description { get; set; } = string.Empty;

       [Required]
       [Display(Name = "Leader")]
       public int LeaderId { get; set; }

       // Navigation property for the leader
       [ForeignKey("LeaderId")]
       public virtual Character Leader { get; set; } = null!;

       // Navigation property for comments
       public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

       // Timestamp for concurrency check
       [Timestamp]
       public byte[] RowVersion { get; set; } = Array.Empty<byte>();
   }
}