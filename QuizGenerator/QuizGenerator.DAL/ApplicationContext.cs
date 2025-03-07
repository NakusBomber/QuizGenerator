using Microsoft.EntityFrameworkCore;
using QuizGenerator.Model.Entities;

namespace QuizGenerator.DAL;

public class ApplicationContext : DbContext
{
	public DbSet<Quiz> Quizes { get; set; }
	public DbSet<Question> Questions { get; set; }
	public DbSet<QuestionDetail> QuestionDetails { get; set; }
	public DbSet<AnswerDetail> AnswerDetails { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlite("Filename=database.db");
	}
}
