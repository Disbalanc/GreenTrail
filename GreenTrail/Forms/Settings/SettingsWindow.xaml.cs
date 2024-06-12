using GreenTrail.Forms.Welcome.ForgotPasswprdPage;
using GreenTrail.Source.Funs;
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
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GreenTrail.Properties;
using System.ComponentModel;
using Jamesnet.Wpf.Controls;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Win32;
using System.IO;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Data.Entity;
using GreenTrail.Forms.ViewModel;

namespace GreenTrail.Forms.Settings
{
    public static class ThemeManager
    {
        private const string ThemeSettingKey = "Theme";

        public static event PropertyChangedEventHandler ThemeChanged;

        public static void SwitchTheme(bool isDarkTheme)
        {
            // Store the theme preference in settings
            Properties.Settings.Default[ThemeSettingKey] = isDarkTheme;
            Properties.Settings.Default.Save();

            // Clear the current theme resources
            var appResources = Application.Current.Resources;
            appResources.MergedDictionaries.Clear();

            // Load the new theme resources
            if (isDarkTheme)
            {
                var darkThemeResourceDictionary = new ResourceDictionary();
                darkThemeResourceDictionary.Source = new Uri("/Source/Theme/DarkTheme.xaml", UriKind.Relative);
                appResources.MergedDictionaries.Add(darkThemeResourceDictionary);
            }
            else
            {
                var lightThemeResourceDictionary = new ResourceDictionary();
                lightThemeResourceDictionary.Source = new Uri("/Source/Theme/LigthTheme.xaml", UriKind.Relative);
                appResources.MergedDictionaries.Add(lightThemeResourceDictionary);
            }

            // Raise the ThemeChanged event
            ThemeChanged?.Invoke(null, new PropertyChangedEventArgs("Theme"));
        }

        public static void InitializeTheme()
        {
            // Load the theme preference from settings
            bool isDarkTheme = (bool)Properties.Settings.Default[ThemeSettingKey];

            // Apply the theme
            SwitchTheme(isDarkTheme);
        }
    }

    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Funs.WindowDragMove(sender, e, this);
        }

        private void MinimizeClick(object sender, RoutedEventArgs e)
        {
            Funs.MinimizeToTaskbar(this);
        }

        private void MaximizeClick(object sender, RoutedEventArgs e)
        {
            Funs.ToggleMinimizeMaximize(this);
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private const string ThemeSettingKey = "Theme";

        public SettingsWindow()
        {
            InitializeComponent();

            // Load the theme preference from settings
            bool isDarkTheme = (bool)Properties.Settings.Default[ThemeSettingKey];

            jamesToggleSwitch.IsChecked = isDarkTheme;

            var user = DataBaseFuns.GetCurrentUser();

            tb_login.Text = user.login;
            tb_email.Text = user.email;
            tb_BirthDate.Text = user.dateOfBirth;
            tb_Address.Text = user.address;
            tb_telephoneNumber.Text = user.phoneNumber;
            tb_full_name.Text = user.full_name;

            image.Source = DataBaseFuns.LoadImage();
        }

        private void btn_newPass_Click(object sender, RoutedEventArgs e)
        {
            NewPasswordPage newPasswordPage = new NewPasswordPage();
            // Отображаем страницу Login в основном окне
            this.Content = newPasswordPage;
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            var user = DataBaseFuns.GetCurrentUser();

            // Check if the data is identical to the data in the database
            if (tb_login.Text == user.login &&
                tb_email.Text == user.email &&
                tb_BirthDate.Text == user.dateOfBirth &&
                tb_Address.Text == user.address &&
                tb_telephoneNumber.Text == user.phoneNumber &&
                tb_full_name.Text == user.full_name && 
                user.image == Convert.ToBase64String(photoBytes))
            {
                MessageBox.Show("No changes in data");
                return;
            }

            // Update the user object
            user.email = tb_email.Text;
            user.address = tb_Address.Text;
            user.dateOfBirth = tb_BirthDate.Text;
            user.phoneNumber = tb_telephoneNumber.Text;
            user.full_name = tb_full_name.Text;
            user.image = Convert.ToBase64String(photoBytes);

            // Assume 'originalContext' is the original instance of GreanTrailEntities
            GreanTrailEntities.GetContext().Entry(user).State = EntityState.Detached;

            // Get the database context
            using (GreanTrailEntities dbContext = new GreanTrailEntities())
            {
                // Update the user object in the database context
                dbContext.Users.Attach(user);
                dbContext.Entry(user).State = EntityState.Modified;

                // Save the changes to the database
                dbContext.SaveChanges();
            }

            MessageBox.Show("Изменения внесены в базу");
        }

        
        private void JamesToggleSwitch_Click(object sender, RoutedEventArgs e)
        {
            switch (jamesToggleSwitch.IsChecked)
            {
                case true:
                    {
                        // Switch to Dark theme
                        ThemeManager.SwitchTheme(true);
                        break;
                    }
                case false:
                    {
                        // Switch to Light theme
                        ThemeManager.SwitchTheme(false);
                        break;
                    }
            }
        }

        // Create a byte array to store the photo
        byte[] photoBytes;

        // Load photo button click event handler
        private void LoadPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a OpenFileDialog to select the photo
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            if (openFileDialog.ShowDialog() == true)
            {
                // Load the selected photo
                BitmapImage bitmapImage = new BitmapImage(new Uri(openFileDialog.FileName));
                image.Source = bitmapImage;

                // Convert the photo to a byte array
                using (Stream stream = openFileDialog.OpenFile())
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        stream.CopyTo(memoryStream);
                        photoBytes = memoryStream.ToArray();
                    }
                }
            }
        }
    }
}