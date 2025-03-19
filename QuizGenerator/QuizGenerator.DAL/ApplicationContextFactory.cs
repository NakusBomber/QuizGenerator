using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace QuizGenerator.DAL;

public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
{
	public ApplicationContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();

		var path = Path.Combine(Environment.CurrentDirectory, "database.db");
		optionsBuilder.UseSqlite($"Filename={path}");

		return new ApplicationContext(optionsBuilder.Options);
	}
}
