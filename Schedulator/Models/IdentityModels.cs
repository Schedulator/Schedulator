using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Schedulator.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual Program Program { get; set; }
    }
  
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Tutorial> Tutorials { get; set; }
        public DbSet<Lab> Labs { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<Enrollment> Enrollment { get; set; }
        public DbSet<Section> Section { get; set; }
        public DbSet<Program> Program { get; set; }
        public DbSet<CourseSequence> CourseSequence { get; set; }
        public DbSet<Prerequisite> Prerequisite { get; set; }
        public static ApplicationDbContext Create()
        {

            return new ApplicationDbContext();
        }


        
    }
}