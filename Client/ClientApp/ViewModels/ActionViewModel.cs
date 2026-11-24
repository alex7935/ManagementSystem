using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ClientApp.Services;

namespace ClientApp.ViewModels;

public partial class ActionViewModel : ObservableObject
{
    private readonly ApiService _apiService = new();

    [ObservableProperty]
    private string courseName = string.Empty;

    [ObservableProperty]
    private string proposalType = "Create";

    [ObservableProperty]
    private string resultMessage = string.Empty;

    [RelayCommand]
    private async Task SubmitProposalAsync()
    {
        if (string.IsNullOrWhiteSpace(CourseName))
        {
            ResultMessage = "Введіть назву курсу!";
            return;
        }

        bool ok = await _apiService.CreateProposalAsync(CourseName, ProposalType);
        if (ok)
        {
            ResultMessage = "Пропозицію успішно надіслано!";
            CourseName = string.Empty;
            await Task.Delay(800);
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            ResultMessage = "Помилка при відправці пропозиції.";
        }
    }
}