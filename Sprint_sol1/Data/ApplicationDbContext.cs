using Sprint_sol1.Models;
using Microsoft.EntityFrameworkCore;

namespace Sprint_sol1.Data
{
    public class ApplicationDbContext: DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
            
        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<UserMaster> UserMasters { get; set; }
        public DbSet<GradeMaster> GradeMasters { get; set; }
    }
}
