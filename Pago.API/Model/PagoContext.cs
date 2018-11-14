using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pago.API.Model
{
    public class PagoContext : DbContext
    {
        public PagoContext(DbContextOptions<PagoContext> options) : base(options)
        {
        }

        public DbSet<PagoResponse> Pagos { get; set; }
    }
}
