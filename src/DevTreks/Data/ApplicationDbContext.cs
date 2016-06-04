using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DevTreks.Models;

namespace DevTreks.Models
{
    /// <summary>
    ///Purpose:	Manage db logins
    ///Author   www.devtreks.org
    ///Date:    2016, March
    ///Note:	This is only used by UI layer to manage logins.
    ///         Rest of db is managed on data layer.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //the Identity model makes the commented out code unnecessary
            
        }
        //2.0.0 change: Aspnet Identity model removed and replaced with std services/reposit pattern
        //for inserting new member with 1 to 1 to aspnetuser insertion
    }
}
