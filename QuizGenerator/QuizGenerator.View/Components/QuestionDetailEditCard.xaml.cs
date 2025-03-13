using QuizGenerator.ViewModel.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuizGenerator.View.Components
{
    /// <summary>
    /// Interaction logic for QuestionDetailEditCard.xaml
    /// </summary>
    public partial class QuestionDetailEditCard : UserControl
    {
		public QuestionDetailViewModel QuestionDetail
		{
			get { return (QuestionDetailViewModel)GetValue(QuestionDetailProperty); }
			set { SetValue(QuestionDetailProperty, value); }
		}

		public static readonly DependencyProperty QuestionDetailProperty =
			DependencyProperty.Register("QuestionDetail", typeof(QuestionDetailViewModel), typeof(QuestionDetailEditCard), new PropertyMetadata(null));



		public ICommand EditCommand
		{
			get { return (ICommand)GetValue(EditCommandProperty); }
			set { SetValue(EditCommandProperty, value); }
		}

		public static readonly DependencyProperty EditCommandProperty =
			DependencyProperty.Register("EditCommand", typeof(ICommand), typeof(QuestionDetailEditCard), new PropertyMetadata(null));



		public object EditCommandParameter
		{
			get { return (object)GetValue(EditCommandParameterProperty); }
			set { SetValue(EditCommandParameterProperty, value); }
		}

		public static readonly DependencyProperty EditCommandParameterProperty =
			DependencyProperty.Register("EditCommandParameter", typeof(object), typeof(QuestionDetailEditCard), new PropertyMetadata(null));



		public ICommand DeleteCommand
		{
			get { return (ICommand)GetValue(DeleteCommandProperty); }
			set { SetValue(DeleteCommandProperty, value); }
		}

		public static readonly DependencyProperty DeleteCommandProperty =
			DependencyProperty.Register("DeleteCommand", typeof(ICommand), typeof(QuestionDetailEditCard), new PropertyMetadata(null));



		public object DeleteCommandParameter
		{
			get { return (object)GetValue(DeleteCommandParameterProperty); }
			set { SetValue(DeleteCommandParameterProperty, value); }
		}

		public static readonly DependencyProperty DeleteCommandParameterProperty =
			DependencyProperty.Register("DeleteCommandParameter", typeof(object), typeof(QuestionDetailEditCard), new PropertyMetadata(null));


		public QuestionDetailEditCard()
        {
            InitializeComponent();
        }
    }
}
