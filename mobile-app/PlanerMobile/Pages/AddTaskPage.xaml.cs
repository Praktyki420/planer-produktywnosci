using PlanerMobile.Models;
using PlanerMobile.Services;

namespace PlanerMobile.Pages;

public partial class AddTaskPage : ContentPage
{
    private readonly ApiService _api = new();

    public AddTaskPage()
    {
        InitializeComponent();
        pickerPriority.SelectedIndex = 1; // Œredni
    }

    private async void btnSave_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(entryTitle.Text))
        {
            lblError.Text = "Tytu³ jest wymagany!";
            lblError.IsVisible = true;
            return;
        }

        var task = new TaskItem
        {
            Title = entryTitle.Text.Trim(),
            Description = editorDescription.Text?.Trim() ?? "",
            Priority = pickerPriority.SelectedItem?.ToString() ?? "Œredni",
            Category = entryCategory.Text?.Trim() ?? "",
            DueDate = datePicker.Date,
            Status = "Nowe",
            UserId = Preferences.Get("userId", 0)
        };

        var ok = await _api.CreateTaskAsync(task);
        if (ok)
            await Shell.Current.GoToAsync("..");
        else
        {
            lblError.Text = "B³¹d podczas dodawania zadania";
            lblError.IsVisible = true;
        }
    }
}