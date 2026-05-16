namespace PlanerMobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("tasks", typeof(Pages.TaskListPage));
        Routing.RegisterRoute("today", typeof(Pages.TodayPage));
        Routing.RegisterRoute("addtask", typeof(Pages.AddTaskPage));
    }
}