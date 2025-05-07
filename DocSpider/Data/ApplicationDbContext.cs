using DocSpider.Models;
using Microsoft.EntityFrameworkCore;

namespace DocSpider.Data

{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<DocumentosModel> Documentos { get; set; }
    }
}
