using QuizGenerator.Model.Entities;
using QuizGenerator.ViewModel.ViewModels.Bases;
using System.Collections.ObjectModel;

namespace QuizGenerator.ViewModel.ViewModels.Models;

public class QuizViewModel : ViewModelBase
{

    private Guid _id;

    public Guid Id
    {
        get => _id;
        set
        {
            _id = value;
            OnPropertyChanged();
        }
    }


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

    public DateTime DateTimeCreated
    {
        get => _dateTimeCreated;
        set
        {
            _dateTimeCreated = value;
            OnPropertyChanged();
        }
    }

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

    private ObservableCollection<QuestionViewModel> _questions;

    public ObservableCollection<QuestionViewModel> Questions
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
        _interval = quiz.IntervalPractice;
        _questions = new ObservableCollection<QuestionViewModel>(quiz.Questions.Select(q => new QuestionViewModel(q)));
    }

    public Quiz ToQuiz()
    {
        return new Quiz(
            _id,
            _name,
            _dateTimeCreated,
            _dateTimeChanged,
            null,
            _interval,
            _questions.Select(qVM => qVM.ToQuestion()));
    }
}
