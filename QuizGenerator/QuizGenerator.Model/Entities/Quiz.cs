namespace QuizGenerator.Model.Entities;

public class Quiz
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime DateTimeCreated { get; set; }
    public DateTime DateTimeChanged { get; set; }
    public DateTime? DateTimeLastPractice { get; set; }
    public TimeSpan? IntervalPractice { get; set; }

    public List<Question> Questions { get; set; }

    public Quiz(string name, List<Question>? questions = null) :
        this(Guid.NewGuid(),
            name,
            DateTime.Now,
            DateTime.Now,
            null,
            null,
            questions ?? new List<Question>())
    {
    }

    public Quiz(Guid id, string name, DateTime dateTimeCreated, DateTime dateTimeChanged, DateTime? dateTimeLastPractice, TimeSpan? intervalPractice, List<Question> questions)
    {
        Id = id;
        Name = name;
        DateTimeCreated = dateTimeCreated;
        DateTimeChanged = dateTimeChanged;
        DateTimeLastPractice = dateTimeLastPractice;
        IntervalPractice = intervalPractice;
        Questions = questions;
    }


}
