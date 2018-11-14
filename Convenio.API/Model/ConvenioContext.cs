using Microsoft.EntityFrameworkCore;

namespace Convenio.API.Model
{
    public class ConvenioContext : DbContext
    {
        public ConvenioContext(DbContextOptions<ConvenioContext> options) : base(options)
        {
        }      

        public DbSet<Convenio> Convenios { get; set; }
    }
}
