using PlanerDesktop.Services;

namespace PlanerDesktop;

public partial class LoginForm : Form
{
    private readonly ApiService _api = new();

    public LoginForm()
    {
        InitializeComponent();
        this.Text = "Logowanie — Planer Produktywnoœci";
        this.Size = new Size(400, 300);
        this.StartPosition = FormStartPosition.CenterScreen;
    }

    private async void btnLogin_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtEmail.Text) ||
            string.IsNullOrWhiteSpace(txtPassword.Text))
        {
            lblError.Text = "Wype³nij wszystkie pola!";
            lblError.ForeColor = Color.Red;
            return;
        }

        btnLogin.Enabled = false;
        btnLogin.Text = "Logowanie...";

        var user = await _api.LoginAsync(txtEmail.Text, txtPassword.Text);

        if (user == null)
        {
            lblError.Text = "B³êdny email lub has³o!";
            lblError.ForeColor = Color.Red;
            btnLogin.Enabled = true;
            btnLogin.Text = "Zaloguj siê";
            return;
        }

        // Otwórz g³ówne okno
        var mainForm = new MainForm(user, _api);
        mainForm.Show();
        this.Hide();
    }
}