using QuizGenerator.Model.Entities;
using QuizGenerator.ViewModel.ViewModels.Bases;
using System.Collections.ObjectModel;

namespace QuizGenerator.ViewModel.ViewModels.Models;

public class QuizViewModel : ViewModelBase
{

    private Guid _id;

    public Guid Id => _id;


    private string _name;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    private DateTime _dateTimeCreated;

    public DateTime DateTimeCreated => _dateTimeCreated;

    private DateTime _dateTimeChanged;

    public DateTime DateTimeChanged
    {
        get => _dateTimeChanged;
        set
        {
            _dateTimeChanged = value;
            OnPropertyChanged();
        }
    }


    private bool _isNeedInterval;

    public bool IsNeedInterval
    {
        get => _isNeedInterval;
        set
        {
            if (value == false)
            {
                Interval = null;
            }
            _isNeedInterval = value;
            OnPropertyChanged();
        }
    }

    private TimeSpan? _interval;

    public TimeSpan? Interval
    {
        get => _interval;
        set
        {
            _interval = value;
            OnPropertyChanged();
        }
    }

    private DateTime? _dateTimeLastPractice;

    public DateTime? DateTimeLastPractice
    {
        get => _dateTimeLastPractice;
        set
        {
            _dateTimeLastPractice = value;
            OnPropertyChanged();
        }
    }



    private ICollection<QuestionViewModel> _questions;

    public ICollection<QuestionViewModel> Questions
    {
        get => _questions;
        set
        {
            _questions = value;
            OnPropertyChanged();
        }
    }

    public QuizViewModel()
        : this(new Quiz())
    {
    }

    public QuizViewModel(Quiz quiz)
    {
        _id = quiz.Id;
        _name = quiz.Name;
        _dateTimeCreated = quiz.DateTimeCreated;
        _dateTimeChanged = quiz.DateTimeChanged;
        _isNeedInterval = quiz.IntervalPractice != null;
        _dateTimeLastPractice = quiz.DateTimeLastPractice;
        _interval = quiz.IntervalPractice;
        _questions = new ObservableCollection<QuestionViewModel>(
            quiz.Questions.Select(q => new QuestionViewModel(q)));
    }

    public void CopyToQuiz(Quiz quiz)
    {
		ArgumentNullException.ThrowIfNull(quiz);

		quiz.Name = _name;
        quiz.DateTimeCreated = _dateTimeCreated;
        quiz.DateTimeChanged = _dateTimeChanged;
        quiz.IntervalPractice = _interval;
        quiz.DateTimeLastPractice = _dateTimeLastPractice;
    }
}
