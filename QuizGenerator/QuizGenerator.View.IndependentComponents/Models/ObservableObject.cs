using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuizGenerator.View.IndependentComponents.Models;

public class ObservableObject : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
