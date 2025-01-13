using QuizApp.MAUI.ViewModels;
namespace QuizApp.MAUI.Views;

public partial class HighscorePage : ContentPage
{
	public HighscorePage(HighscoreViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Unfourtnately, I had to implement this in the code-behind in order or the highescore to be loaded
        if (BindingContext is HighscoreViewModel viewModel)
        {
            await viewModel.LoadResults();
        }
    }
}