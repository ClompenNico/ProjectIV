using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MicrosoftTeamsBot.Models
{
    public class Vouchers
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        public bool Received { get; set; }
    }
}