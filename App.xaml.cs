using Microsoft.Win32;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Sit_and_Stand_Reminder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        NotifyIcon nIcon = new NotifyIcon();

        public App()
        {
            // Setup context menu
            ContextMenu contextMenu = new ContextMenu();

            // Setup menuItem1
            MenuItem menuItem1 = new MenuItem();
            menuItem1.Index = 0;
            menuItem1.Text = "Add to startup";
            menuItem1.Click += new EventHandler(menuItem1_Click);

            // Add menuItem2 to context menu
            contextMenu.MenuItems.AddRange(new MenuItem[] { menuItem1 });

            // Setup menuItem2
            MenuItem menuItem2 = new MenuItem();
            menuItem2.Index = 0;
            menuItem2.Text = "Remove from startup";
            menuItem2.Click += new EventHandler(menuItem2_Click);

            // Add menuItem3 to context menu
            contextMenu.MenuItems.AddRange(new MenuItem[] { menuItem2 });

            // Setup menuItem3
            MenuItem menuItem3 = new MenuItem();
            menuItem3.Index = 0;
            menuItem3.Text = "Exit";
            menuItem3.Click += new EventHandler(menuItem3_Click);

            // Add menuItem1 to context menu
            contextMenu.MenuItems.AddRange(new MenuItem[] { menuItem3 });

            // Setup tray icon
            nIcon.ContextMenu = contextMenu;
            nIcon.Icon = Sit_and_Stand_Reminder.Properties.Resources.icon;
            nIcon.Visible = true;
            nIcon.Text = "Sit and Stand Reminder";
            //nIcon.DoubleClick += nIcon_Click;

            // Setup recurring reminders
            Task.Run(async () => {
                while (true)
                {
                    await Task.Delay(TimeSpan.FromMinutes(40));
                    nIcon.ShowBalloonTip(5000, "Time to Sit", "Take a load off!", System.Windows.Forms.ToolTipIcon.Info);
                    await Task.Delay(TimeSpan.FromMinutes(20));
                    nIcon.ShowBalloonTip(5000, "Time to Stand", "Stretch those legs!", System.Windows.Forms.ToolTipIcon.Info);
                }
            });
        }

        void menuItem1_Click(object sender, EventArgs e)
        {
            AddApplicationToCurrentUserStartup();

            System.Windows.MessageBox.Show("Sit and Stand Reminder has been added to startup.");
        }

        void menuItem2_Click(object sender, EventArgs e)
        {
            RemoveApplicationFromCurrentUserStartup();

            System.Windows.MessageBox.Show("Sit and Stand Reminder has been removed from startup.");
        }

        void menuItem3_Click(object sender, EventArgs e)
        {
            Current.Shutdown();
        }

        void nIcon_Click(object sender, EventArgs e)
        {
            // Restore window
            MainWindow.Visibility = Visibility.Visible;
            MainWindow.WindowState = WindowState.Normal;
        }

        public static void AddApplicationToCurrentUserStartup()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.SetValue("Sit and Stand Reminder", "\"" + System.Reflection.Assembly.GetExecutingAssembly().Location + "\"");
            }
        }

        public static void RemoveApplicationFromCurrentUserStartup()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.DeleteValue("Sit and Stand Reminder", false);
            }
        }
    }
}
