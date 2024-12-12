using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IT2030FinalProject.Models
{
   public class Character
   {
       public int Id { get; set; }

       [Required]
       [StringLength(100)]
       public string Name { get; set; } = string.Empty;

       public List<string> Aliases { get; set; } = new List<string>();

       [Display(Name = "Eye Color")]
       [StringLength(50)] 
       public string EyeColor { get; set; } = string.Empty;

       [Required]
       [StringLength(2000)]
       public string About { get; set; } = string.Empty;

       [StringLength(500)]
       public string Powers { get; set; } = string.Empty;

       // Navigation property
       public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

       // Timestamp for concurrency check
       [Timestamp]
       public byte[] RowVersion { get; set; } = Array.Empty<byte>();
   }
}