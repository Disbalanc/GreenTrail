using GreenTrail.Forms.Welcome.ForgotPasswprdPage;
using GreenTrail.Source.Funs;
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

namespace GreenTrail.Forms.Welcome
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            string hashedPassword;
            if (_isShowingPassword)
            {
                hashedPassword = Funs.ComputeSHA256Hash(tb_pass.Text);

                DataBaseFuns.AuthenticateUser(tb_login.Text, hashedPassword, this);
            }
            else
            {
                hashedPassword = Funs.ComputeSHA256Hash(pb_pass.Password);

                DataBaseFuns.AuthenticateUser(tb_login.Text, hashedPassword, this);
            }
            if (cb_rememberMe.IsChecked.Value)
            {
                // Сохраняем флаг показа в реестр
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\GreenTrail", true);
                key.SetValue("RememberMe", cb_rememberMe.IsChecked.Value);
            }
        }

        private void btn_reginWindow_Click(object sender, RoutedEventArgs e)
        {
            CreateAccountWindow createAccountWindow = new CreateAccountWindow();
            createAccountWindow.Show();
            this.Close();
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

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Funs.WindowDragMove(sender, e, this);
        }

        private void tb_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox TB = sender as TextBox;
            if (TB.Text == "Логин" || TB.Text == "Пароль") TB.Text = string.Empty;
        }

        private void tb_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.Text == string.Empty)
            {
                // Получаем ресурс SolidColorBrush
                SolidColorBrush errorColor = (SolidColorBrush)FindResource("Error");
                textBox.BorderBrush = errorColor;
                switch (textBox.Name)
                {
                    case ("tb_login"):
                        {
                            textBox.Text = "Логин";
                            break;
                        }
                    case ("tb_pass"):
                        {
                            textBox.Text = "Пароль";
                            break;
                        }
                }
            }
            else
            {
                // Получаем ресурс SolidColorBrush
                SolidColorBrush additionalColor = (SolidColorBrush)FindResource("AdditionalColor");
                textBox.BorderBrush = additionalColor;
            }
        }

        private void pb_LostFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;

            if (passwordBox.Password == string.Empty)
            {
                // Получаем ресурс SolidColorBrush
                SolidColorBrush errorColor = (SolidColorBrush)FindResource("Error");
                passwordBox.BorderBrush = errorColor;
            }
            else
            {
                // Получаем ресурс SolidColorBrush
                SolidColorBrush additionalColor = (SolidColorBrush)FindResource("AdditionalColor");
                passwordBox.BorderBrush = additionalColor;
            }
        }

        private bool _isShowingPassword = true;

        private void show_pass(object sender, RoutedEventArgs e)
        {
            Uri imageUriSlash = new Uri("Image/Assets/eye-slash.png", UriKind.RelativeOrAbsolute);
            Uri imageUri = new Uri("Image/Assets/eye.png", UriKind.RelativeOrAbsolute);

            if (_isShowingPassword)
            {
                // Скрыть пароль
                if (tb_pass.Text != "Пароль" || tb_pass.Text != "Повторите пароль")
                {
                    pb_pass.Password = tb_pass.Text; // скопируем в TextBox из PasswordBox
                }
                pb_pass.Visibility = Visibility.Hidden; // TextBox - отобразить
                tb_pass.Visibility = Visibility.Visible; // PasswordBox - скрыть
                im_pass.Source = new BitmapImage(imageUriSlash);
            }
            else
            {
                // Показать пароль

                tb_pass.Text = pb_pass.Password; // скопируем в TextBox из PasswordBox
                pb_pass.Visibility = Visibility.Visible; // TextBox - отобразить
                tb_pass.Visibility = Visibility.Hidden; // PasswordBox - скрыть
                im_pass.Source = new BitmapImage(imageUri);
            }

            _isShowingPassword = !_isShowingPassword;
        }

        private void UpdateSubmitButtonState()
        {
            // Проверяем, заполнены ли все TextBox и PasswordBox
            bool areAllFieldsFilled = true;

            if (string.IsNullOrEmpty(tb_login.Text) || tb_login.Text == "Логин")
            {
                    areAllFieldsFilled = false;
            }

            if (_isShowingPassword)
            {
                if (tb_pass != null)
                {
                    if (string.IsNullOrEmpty(tb_pass.Text) || tb_pass.Text == "Пароль")
                    {
                        areAllFieldsFilled = false;
                    }
                }
                else { return; }
            }
            else
            {
                if (string.IsNullOrEmpty(pb_pass.Password) || pb_pass.Password == "Пароль")
                {
                    areAllFieldsFilled = false;
                }
            }

            if (btn_login != null)
            {
                // Включаем или отключаем кнопку "Отправить" в зависимости от результата проверки
                if (areAllFieldsFilled)
                {
                    btn_login.IsEnabled = true;
                    btn_login.Opacity = 100;
                }
            }
        }
        private void tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateSubmitButtonState();
        }
        private void pb_PasswordChanged(object sender, RoutedEventArgs e)
        {
            UpdateSubmitButtonState();
        }
        
        private void btn_forgotPass_Click(object sender, RoutedEventArgs e)
        {
            ForgotPasswordPage forgotPass = new ForgotPasswordPage();
            forgotPass.GoBack += ForgotPass_GoBack;
            this.Content = forgotPass;
        }

        private void ForgotPass_GoBack(object sender, EventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close(); // закрыть текущее окно
        }
    }
}
