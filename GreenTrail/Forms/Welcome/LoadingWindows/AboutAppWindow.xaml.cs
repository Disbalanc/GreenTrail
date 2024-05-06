using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace GreenTrail.Forms.Welcome.LoadingWindows
{
    /// <summary>
    /// Логика взаимодействия для AboutAppWindow.xaml
    /// </summary>
    public partial class AboutAppWindow : Window
    {
        private int currentIndex = 0;

        public AboutAppWindow()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            currentIndex--;

            if (currentIndex < 0)
            {
                currentIndex = 0;
            }

            UpdateUI();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            currentIndex++;

            if (currentIndex == 3)
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                // Устанавливаем флаг показа в true
                bool hasShownAboutAppWindow = true;

                // Сохраняем флаг показа в реестр
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\GreenTrail", true);
                if (key == null)
                {
                    key = Registry.CurrentUser.CreateSubKey("Software\\GreenTrail");
                }
                key.SetValue("HasShownAboutAppWindow", hasShownAboutAppWindow);
                this.Close();
            }

            UpdateUI();
        }

        private void UpdateUI()
        {
            img1.Visibility = Visibility.Collapsed;
            img2.Visibility = Visibility.Collapsed;
            img3.Visibility = Visibility.Collapsed;
            txt1.Visibility = Visibility.Collapsed;
            txt2.Visibility = Visibility.Collapsed;
            txt3.Visibility = Visibility.Collapsed;

            switch (currentIndex)
            {
                case 0:
                    img1.Visibility = Visibility.Visible;
                    txt1.Visibility = Visibility.Visible;
                    break;
                case 1:
                    img2.Visibility = Visibility.Visible;
                    txt2.Visibility = Visibility.Visible;
                    break;
                case 2:
                    img3.Visibility = Visibility.Visible;
                    txt3.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}
