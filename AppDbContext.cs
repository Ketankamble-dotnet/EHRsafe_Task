using Microsoft.EntityFrameworkCore;
using EHRsafe_Task.Models;



namespace EHRsafe_Task
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
		public DbSet<User> Users { get; set; }
		public DbSet<Medication> Medications { get; set; }
	}
}
