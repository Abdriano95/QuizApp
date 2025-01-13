using Microsoft.Maui.Controls;
using QuizApp.MAUI.ViewModels;

namespace QuizApp.MAUI.Views;

public partial class GamePage : ContentPage
{
	public GamePage(GameViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}