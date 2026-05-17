using PlanerMobile.Services;

namespace PlanerMobile.Pages;

public partial class TodayPage : ContentPage
{
    private readonly ApiService _api = new();

    public TodayPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var userId = Preferences.Get("userId", 0);
        var tasks = await _api.GetTasksAsync(userId);
        var today = tasks.Where(t => t.DueDate?.Date == DateTime.Today).ToList();

        collectionToday.ItemsSource = today;
        lblEmpty.IsVisible = today.Count == 0;
    }
}