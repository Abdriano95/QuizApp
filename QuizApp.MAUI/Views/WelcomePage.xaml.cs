using QuizApp.MAUI.ViewModels;

namespace QuizApp.MAUI.Views;

public partial class WelcomePage : ContentPage
{
	public WelcomePage(WelcomeViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}