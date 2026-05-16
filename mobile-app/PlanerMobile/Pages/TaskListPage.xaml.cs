using PlanerMobile.Models;
using PlanerMobile.Services;

namespace PlanerMobile.Pages;

public partial class TaskListPage : ContentPage
{
    private readonly ApiService _api = new();
    private List<TaskItem> _tasks = new();
    private readonly int _userId;

    public TaskListPage()
    {
        InitializeComponent();
        _userId = Preferences.Get("userId", 0);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadTasks();
    }

    private async Task LoadTasks()
    {
        var status = pickerStatus.SelectedItem?.ToString();
        if (status == "Wszystkie") status = null;
        _tasks = await _api.GetTasksAsync(_userId, status);
        collectionTasks.ItemsSource = null;
        collectionTasks.ItemsSource = _tasks;
    }

    private async void pickerStatus_Changed(object sender, EventArgs e)
    {
        await LoadTasks();
    }

    private async void btnAdd_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("addtask");
    }

    private async void btnDone_Clicked(object sender, EventArgs e)
    {
        var btn = (Button)sender;
        var task = (TaskItem)btn.CommandParameter;
        await _api.MarkAsDoneAsync(task.Id, task);
        await LoadTasks();
    }
}