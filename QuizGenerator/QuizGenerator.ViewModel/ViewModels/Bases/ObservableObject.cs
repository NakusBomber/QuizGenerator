using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuizGenerator.ViewModel.ViewModels.Bases;

public class ObservableObject : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
