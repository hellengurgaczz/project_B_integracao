using project_B_integracao.Models;
using Microsoft.EntityFrameworkCore;

namespace project_B_integracao.Data
{
    public class DataContext : DbContext 
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Folha> Folhas { get; set; }
        
    }
}