using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ClientApp.Services;
using ClientApp.Views;

namespace ClientApp.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly ApiService _apiService = new();

    [ObservableProperty]
    private string email = "petrenko@univ.edu.ua";

    [ObservableProperty]
    private string password = "123456";

    [ObservableProperty]
    private string statusMessage = string.Empty;

    [RelayCommand]
    private async Task LoginAsync()
    {
        StatusMessage = "Виконується вхід...";
        bool success = await _apiService.LoginAsync(Email, Password);
        if (success)
        {
            StatusMessage = string.Empty;
            await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
        }
        else
        {
            StatusMessage = "Помилка входу. Перевірте дані або запуск сервера.";
        }
    }
}