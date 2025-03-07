using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizGenerator.Model.Entities;

[Table("Quizes")]
public class Quiz : Entity
{
    [Key]
    public override Guid Id { get; set; }

    [Required]
    public string Name { get; set; }
    public DateTime DateTimeCreated { get; set; }
    public DateTime DateTimeChanged { get; set; }
    public DateTime? DateTimeLastPractice { get; set; }
    public TimeSpan? IntervalPractice { get; set; }

    public IEnumerable<Question> Questions { get; set; }

    public Quiz()
        : this(string.Empty)
    {
    }

    public Quiz(string name, IEnumerable<Question>? questions = null) :
        this(Guid.NewGuid(),
            name,
            DateTime.Now,
            DateTime.Now,
            null,
            null,
            questions ?? new List<Question>())
    {
    }

    public Quiz(Guid id, string name, DateTime dateTimeCreated, DateTime dateTimeChanged, DateTime? dateTimeLastPractice, TimeSpan? intervalPractice, IEnumerable<Question> questions)
    {
        Id = id;
        Name = name;
        DateTimeCreated = dateTimeCreated;
        DateTimeChanged = dateTimeChanged;
        DateTimeLastPractice = dateTimeLastPractice;
        IntervalPractice = intervalPractice;
        Questions = questions;
    }

	public override int GetHashCode()
	{
		return (Id, Name, DateTimeCreated, DateTimeChanged, DateTimeLastPractice, IntervalPractice).GetHashCode();
	}
}
