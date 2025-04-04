using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuizGenerator.Model.Entities;

namespace QuizGenerator.DAL;

public class ApplicationContext : DbContext
{
	public DbSet<Quiz> Quizzes { get; set; }
	public DbSet<Question> Questions { get; set; }
	public DbSet<QuestionDetail> QuestionDetails { get; set; }
	public DbSet<AnswerDetail> AnswerDetails { get; set; }

	public ApplicationContext(DbContextOptions<ApplicationContext> options)
		: base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Quiz>()
			.HasKey(q => q.Id);
		modelBuilder.Entity<Quiz>()
			.HasMany(q => q.Questions)
			.WithOne(question => question.Quiz)
			.HasForeignKey(question => question.QuizId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<Question>()
			.HasKey(question => question.Id);
		modelBuilder.Entity<Question>()
			.HasMany(question => question.QuestionDetails)
			.WithOne(qd => qd.Question)
			.HasForeignKey(qd => qd.QuestionId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<QuestionDetail>()
			.HasKey(qd => qd.Id);
		modelBuilder.Entity<QuestionDetail>()
			.HasMany(qd => qd.AnswerDetails)
			.WithOne(ad => ad.QuestionDetail)
			.HasForeignKey(ad => ad.QuestionDetailId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<AnswerDetail>()
			.HasKey(ad => ad.Id);
		modelBuilder.Entity<AnswerDetail>()
			.HasOne(ad => ad.QuestionDetail)
			.WithMany(qd => qd.AnswerDetails)
			.HasForeignKey(ad => ad.QuestionDetailId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
