using QuizApp.MAUI.ViewModels;

namespace QuizApp.MAUI.Views;

public partial class ResultsPage : ContentPage
{
	public ResultsPage(ResultsViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}