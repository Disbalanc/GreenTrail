using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GreenTrail.Forms.Welcome.LoadingWindows
{
    /// <summary>
    /// Логика взаимодействия для BootWindow.xaml
    /// </summary>
    public partial class BootWindow : Window
    {
        public static bool hasShownAboutAppWindow = false;

        public BootWindow()
        {
            InitializeComponent();

            // Считываем значение флага показа из реестра
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\GreenTrail", true);
            if (key != null)
            {
                hasShownAboutAppWindow = bool.Parse((string)key.GetValue("HasShownAboutAppWindow"));
            }
                // Создаем и запускаем таймер
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(0.5);
                timer.Tick += Timer_Tick;
                timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Обновляем прогресс загрузки
            pb.Value += 1;

            // Если загрузка завершена, останавливаем таймер и закрываем окно
            if (pb.Value >= pb.Maximum)
            {
                ((DispatcherTimer)sender).Stop();
                if (hasShownAboutAppWindow)
                {
                    // Считываем значение флага показа из реестра
                    RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\GreenTrail", true);
                    if (key != null)
                    {
                        if (bool.Parse((string)key.GetValue("RememberMe")))
                        {
                            MainWindow mainWindow = new MainWindow();
                            mainWindow.Show();
                            this.Close();
                        }
                        else
                        {
                            LoginWindow loginWindow = new LoginWindow();
                            loginWindow.Show();
                        }
                    }
                    else
                    {
                        LoginWindow loginWindow = new LoginWindow();
                        loginWindow.Show();
                    }
                }
                else
                {
                    AboutAppWindow aboutAppWindow = new AboutAppWindow();
                    aboutAppWindow.Show();
                }
                this.Close();
            }
        }
    }
}
