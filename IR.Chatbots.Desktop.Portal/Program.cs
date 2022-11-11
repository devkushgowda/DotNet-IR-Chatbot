using IR.Chatbots.Database.MongoDB;
using IR.Chatbots.Desktop.Portal.Configuration;
using System;
using System.Windows.Forms;

namespace IR.Chatbots.Desktop.Portal
{
    static class Program
    {
        public readonly static AppSettings AppConfiguration = AppSettings.LoadConfiguration();
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ConnectToDatabase();
            Application.Run(new Login());
        }

        private static void ConnectToDatabase()
        {
            MongoDbProvider.Connect(AppConfiguration.GetActiveDbConnectionString());
        }
    }
}
