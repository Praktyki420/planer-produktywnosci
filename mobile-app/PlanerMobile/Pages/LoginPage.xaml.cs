using PlanerMobile.Services;

namespace PlanerMobile.Pages;

public partial class LoginPage : ContentPage
{
    private readonly ApiService _api = new();

    public LoginPage()
    {
        InitializeComponent();
    }

    private async void btnLogin_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(entryEmail.Text) ||
            string.IsNullOrWhiteSpace(entryPassword.Text))
        {
            lblError.Text = "Wype³nij wszystkie pola!";
            lblError.IsVisible = true;
            return;
        }

        btnLogin.IsEnabled = false;
        btnLogin.Text = "Logowanie...";

        var user = await _api.LoginAsync(entryEmail.Text, entryPassword.Text);

        if (user == null)
        {
            lblError.Text = "B³êdny email lub has³o!";
            lblError.IsVisible = true;
            btnLogin.IsEnabled = true;
            btnLogin.Text = "Zaloguj siê";
            return;
        }

        // Zapisz dane u¿ytkownika
        Preferences.Set("userId", user.Id);
        Preferences.Set("username", user.Username);

        // PrzejdŸ do listy zadañ
        await Shell.Current.GoToAsync("tasks");
    }
}