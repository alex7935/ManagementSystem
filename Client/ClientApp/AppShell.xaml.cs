using ClientApp.Views;
namespace ClientApp;
public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(ActionPage), typeof(ActionPage));
    }
}