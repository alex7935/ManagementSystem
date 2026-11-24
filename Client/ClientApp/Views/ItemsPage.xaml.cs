using ClientApp.ViewModels;
namespace ClientApp.Views;
public partial class ItemsPage : ContentPage
{
    public ItemsPage() { InitializeComponent(); }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ItemsViewModel vm)
            await vm.LoadCoursesAsync();
    }
}