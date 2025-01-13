using QuizApp.MAUI.Views;

namespace QuizApp.MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(ResultsPage),typeof(ResultsPage));
            Routing.RegisterRoute(nameof(GamePage), typeof(GamePage));

        }
    }
}
