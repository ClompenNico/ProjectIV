using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MicrosoftTeamsBot.Models;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MicrosoftTeamsBot.Data
{
    public class MicrosoftBotDbContext : IdentityDbContext<ApplicationUser>
    {
        public MicrosoftBotDbContext(DbContextOptions<MicrosoftBotDbContext> options)
            : base(options)
        {
        }

        public DbSet<Users> users { get; set; }
        public DbSet<Vouchers> vouchers{ get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}