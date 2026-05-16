using PlanerDesktop.Models;

namespace PlanerDesktop;

public partial class TaskEditForm : Form
{
    public TaskItem? Task { get; private set; }
    private readonly int _userId;
    private readonly TaskItem? _existing;

    public TaskEditForm(int userId, TaskItem? existing = null)
    {
        InitializeComponent();
        _userId = userId;
        _existing = existing;

        this.Text = existing == null ? "Nowe zadanie" : "Edytuj zadanie";
        this.Size = new Size(450, 450);
        this.StartPosition = FormStartPosition.CenterParent;

        cmbStatus.Items.AddRange(new[] { "Nowe", "W trakcie", "Wykonane" });
        cmbPriority.Items.AddRange(new[] { "Niski", "Średni", "Wysoki" });

        if (existing != null) FillForm(existing);
        else
        {
            cmbStatus.SelectedItem = "Nowe";
            cmbPriority.SelectedItem = "Średni";
            dtpDueDate.Enabled = false;
        }

        chkDueDate.CheckedChanged += (s, e) =>
            dtpDueDate.Enabled = chkDueDate.Checked;
    }

    private void FillForm(TaskItem t)
    {
        txtTitle.Text = t.Title;
        txtDescription.Text = t.Description;
        cmbStatus.SelectedItem = t.Status;
        cmbPriority.SelectedItem = t.Priority;
        txtCategory.Text = t.Category;
        if (t.DueDate.HasValue)
        {
            chkDueDate.Checked = true;
            dtpDueDate.Value = t.DueDate.Value;
            dtpDueDate.Enabled = true;
        }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        // Walidacja
        if (string.IsNullOrWhiteSpace(txtTitle.Text))
        {
            MessageBox.Show("Tytuł jest wymagany!", "Błąd",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Task = new TaskItem
        {
            Title = txtTitle.Text.Trim(),
            Description = txtDescription.Text.Trim(),
            Status = cmbStatus.SelectedItem?.ToString() ?? "Nowe",
            Priority = cmbPriority.SelectedItem?.ToString() ?? "Średni",
            Category = txtCategory.Text.Trim(),
            DueDate = chkDueDate.Checked ? dtpDueDate.Value : null,
            UserId = _userId
        };

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void label1_Click(object sender, EventArgs e)
    {

    }
}