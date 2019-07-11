using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TravelExpenses.Core;

namespace TravelExpenses.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TravelExpenses.Core.Destinos> Destinos { get; set; }
        public DbSet<TravelExpenses.Core.Solicitud> Solicitud { get; set; }
    }
}
