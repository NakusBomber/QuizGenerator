using Microsoft.EntityFrameworkCore;

namespace QuizGenerator.DAL;

public class InMemoryApplicationContext : ApplicationContext
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseInMemoryDatabase("databaseInMemory");
	}
}
