using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MicrosoftTeamsBot.Models
{
    public class Users
    {
        [Key]
        [Required]
        [StringLength(255)]
        public string Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public bool Role { get; set; }
        [Required]
        public Guid VouchersId { get; set; }
    }
}