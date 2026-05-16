using CsvHelper;
using PlanerDesktop.Models;
using PlanerDesktop.Services;
using System.Globalization;

namespace PlanerDesktop;

public partial class MainForm : Form
{
    private readonly ApiService _api;
    private readonly User _user;
    private List<TaskItem> _tasks = new();

    public MainForm(User user, ApiService api)
    {
        InitializeComponent();
        _user = user;
        _api = api;

        this.Text = $"Planer — {user.Username}";
        this.Size = new Size(900, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

        SetupGrid();
        SetupFilters();
    }

    private void SetupGrid()
    {
        dgvTasks.ReadOnly = true;
        dgvTasks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvTasks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvTasks.AllowUserToAddRows = false;
    }

    private void SetupFilters()
    {
        cmbStatus.Items.AddRange(new[] { "Wszystkie", "Nowe", "W trakcie", "Wykonane" });
        cmbStatus.SelectedIndex = 0;
        cmbStatus.SelectedIndexChanged += async (s, e) => await LoadTasks();
        txtSearch.TextChanged += FilterGrid;
    }

    private async void MainForm_Load(object sender, EventArgs e)
    {
        await LoadTasks();
    }

    private async Task LoadTasks()
    {
        var status = cmbStatus.SelectedItem?.ToString();
        if (status == "Wszystkie") status = null;
        _tasks = await _api.GetTasksAsync(_user.Id, status);
        RefreshGrid(_tasks);
    }

    private void RefreshGrid(List<TaskItem> tasks)
    {
        dgvTasks.DataSource = null;
        dgvTasks.DataSource = tasks.Select(t => new
        {
            t.Id,
            Tytuł = t.Title,
            Status = t.Status,
            Priorytet = t.Priority,
            Kategoria = t.Category,
            Termin = t.DueDate?.ToString("dd.MM.yyyy") ?? "—",
            Utworzono = t.CreatedAt.ToString("dd.MM.yyyy")
        }).ToList();
    }

    private void FilterGrid(object? sender, EventArgs e)
    {
        var search = txtSearch.Text.ToLower();
        var filtered = _tasks.Where(t =>
            t.Title.ToLower().Contains(search) ||
            t.Category.ToLower().Contains(search)).ToList();
        RefreshGrid(filtered);
    }

    private TaskItem? GetSelectedTask()
    {
        if (dgvTasks.SelectedRows.Count == 0) return null;
        var id = (int)dgvTasks.SelectedRows[0].Cells["Id"].Value;
        return _tasks.FirstOrDefault(t => t.Id == id);
    }

    private async void btnAdd_Click(object sender, EventArgs e)
    {
        var form = new TaskEditForm(_user.Id);
        if (form.ShowDialog() == DialogResult.OK && form.Task != null)
        {
            await _api.CreateTaskAsync(form.Task);
            await LoadTasks();
        }
    }

    private async void btnEdit_Click(object sender, EventArgs e)
    {
        var task = GetSelectedTask();
        if (task == null) { MessageBox.Show("Wybierz zadanie!"); return; }

        var form = new TaskEditForm(_user.Id, task);
        if (form.ShowDialog() == DialogResult.OK && form.Task != null)
        {
            await _api.UpdateTaskAsync(task.Id, form.Task);
            await LoadTasks();
        }
    }

    private async void btnDelete_Click(object sender, EventArgs e)
    {
        var task = GetSelectedTask();
        if (task == null) { MessageBox.Show("Wybierz zadanie!"); return; }

        var confirm = MessageBox.Show($"Usunąć zadanie \"{task.Title}\"?",
            "Potwierdzenie", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

        if (confirm == DialogResult.Yes)
        {
            await _api.DeleteTaskAsync(task.Id);
            await LoadTasks();
        }
    }

    private void btnReport_Click(object sender, EventArgs e)
    {
        using var dialog = new SaveFileDialog();
        dialog.Filter = "CSV|*.csv";
        dialog.FileName = $"zadania_{DateTime.Now:yyyyMMdd}.csv";

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            using var writer = new StreamWriter(dialog.FileName, false,
                System.Text.Encoding.UTF8);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(_tasks);
            MessageBox.Show("Raport CSV zapisany!", "Sukces",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}