using FASTSocietiesSystem.UI.Forms;

namespace FASTSocietiesSystem;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        
        try
        {
            Application.Run(new LoginForm());
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Fatal Error: {ex.Message}\n{ex.StackTrace}", "Application Error");
            Environment.Exit(1);
        }
    }    
}