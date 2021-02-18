using Invoiceify.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Invoiceify.Persistence
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
    
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(DependencyInjection.DbConnection);
            return new ApplicationDbContext(builder.Options);
        }
    }
}