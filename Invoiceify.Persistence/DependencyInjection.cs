using Invoiceify.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Invoiceify.Persistence
{
    public static class DependencyInjection
    {
        public static string DbConnection = @"Server=(localdb)\Invoiceify;Database=Invoiceify;Trusted_Connection=True;MultipleActiveResultSets=true";
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(DbConnection));
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
            return services;
        }
    }
}