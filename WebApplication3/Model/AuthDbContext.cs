using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApplication3.Model
{



    public class AuthDbContext : IdentityDbContext<UserProfile>
    {
        private readonly IConfiguration _configuration;
		//public AuthDbContext(DbContextOptions<AuthDbContext> options):base(options){ }

		public DbSet<UserProfile> UserProfiles { get; set; }
		public DbSet<AuditLog> AuditLogs { get; set; }
		public AuthDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = _configuration.GetConnectionString("AuthConnectionString"); optionsBuilder.UseSqlServer(connectionString);
        }
    }




}
