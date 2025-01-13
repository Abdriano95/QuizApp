using QuizApp.MAUI.ViewModels;

namespace QuizApp.MAUI.Views;

public partial class HighscorePage : ContentPage
{
	public HighscorePage(HighscoreViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}