using QuizGenerator.Model.Entities;
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
	/// Interaction logic for QuestionEditCard.xaml
	/// </summary>
	public partial class QuestionEditCard : UserControl
    {


        public QuestionViewModel Question
        {
            get { return (QuestionViewModel)GetValue(QuestionProperty); }
            set { SetValue(QuestionProperty, value); }
        }

        public static readonly DependencyProperty QuestionProperty =
            DependencyProperty.Register("Question", typeof(QuestionViewModel), typeof(QuestionEditCard), new PropertyMetadata(null));



        public ICommand EditCommand
        {
            get { return (ICommand)GetValue(EditCommandProperty); }
            set { SetValue(EditCommandProperty, value); }
        }

        public static readonly DependencyProperty EditCommandProperty =
            DependencyProperty.Register("EditCommand", typeof(ICommand), typeof(QuestionEditCard), new PropertyMetadata(null));



        public object EditCommandParameter
        {
            get { return (object)GetValue(EditCommandParameterProperty); }
            set { SetValue(EditCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty EditCommandParameterProperty =
            DependencyProperty.Register("EditCommandParameter", typeof(object), typeof(QuestionEditCard), new PropertyMetadata(null));




        public QuestionEditCard()
        {
            InitializeComponent();
        }
    }
}
