using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Logic;

namespace Logic
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Friend> Friends { get; set; }
    }

}