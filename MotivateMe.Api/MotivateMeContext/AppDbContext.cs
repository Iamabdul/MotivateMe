using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MotivateMe.Api.MotivateMeContext
{
    //To use the ASP.NET Core Identity and create its default Identity database schema, we need to extend our class to use IdentityDbContext
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly DbContextOptions _options;

        public AppDbContext(DbContextOptions options) : base(options)
        {
            _options = options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
