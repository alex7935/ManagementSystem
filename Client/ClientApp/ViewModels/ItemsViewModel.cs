using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ClientApp.Models;
using ClientApp.Services;
using ClientApp.Views;

namespace ClientApp.ViewModels;

public partial class ItemsViewModel : ObservableObject
{
    private readonly ApiService _apiService = new();

    public ObservableCollection<CourseItem> Courses { get; } = new();

    [ObservableProperty]
    private string welcomeText = "Мої курси";

    [RelayCommand]
    public async Task LoadCoursesAsync()
    {
        var teacherName = Preferences.Get("TeacherName", "Викладач");
        WelcomeText = $"Курси викладача: {teacherName}";

        var items = await _apiService.GetCoursesAsync();
        Courses.Clear();
        foreach (var item in items)
        {
            Courses.Add(item);
        }
    }

    [RelayCommand]
    private async Task GoToCreateProposalAsync()
    {
        await Shell.Current.GoToAsync(nameof(ActionPage));
    }
}