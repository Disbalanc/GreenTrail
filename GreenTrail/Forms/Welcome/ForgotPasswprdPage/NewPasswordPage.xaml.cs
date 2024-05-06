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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GreenTrail.Forms.Welcome.ForgotPasswprdPage
{
    /// <summary>
    /// Логика взаимодействия для NewPasswordPage.xaml
    /// </summary>
    public partial class NewPasswordPage : Page
    {
        private bool _isShowingPassword = true;

        public NewPasswordPage()
        {
            InitializeComponent();
        }

        private void btn_NewPass_Click(object sender, RoutedEventArgs e)
        {
                ForgotPasswordPage forgotPasswordPage = new ForgotPasswordPage();
            if (_isShowingPassword)
            {
                DataBaseFuns.RessetPass(forgotPasswordPage.tb_Email.Text, Funs.ComputeSHA256Hash(tb_pass.Text));
            }
            else
            {
                DataBaseFuns.RessetPass(forgotPasswordPage.tb_Email.Text,Funs.ComputeSHA256Hash(pb_pass.Password));
            }
                

                LoginWindow loginWindow = new LoginWindow();
                this.Content = loginWindow;
        }

        private void tb_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.Text == string.Empty)
            {
                // Получаем ресурс SolidColorBrush
                SolidColorBrush errorColor = (SolidColorBrush)FindResource("Error");
                textBox.BorderBrush = errorColor;
                if (tb_pass.Name == "tb_pass")
                {
                    textBox.Text = "Пароль";
                }
                else
                {
                    textBox.Text = "Повторите пароль";
                }
            }
            else
            {
                // Получаем ресурс SolidColorBrush
                SolidColorBrush additionalColor = (SolidColorBrush)FindResource("AdditionalColor");
                textBox.BorderBrush = additionalColor;
            }
        }

        private void tb_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox TB = sender as TextBox;
            if (TB.Text == "Повторите пароль" || TB.Text == "Пароль") TB.Text = string.Empty;
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

        private void tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateSubmitButtonState();
        }
        private void pb_PasswordChanged(object sender, RoutedEventArgs e)
        {
            UpdateSubmitButtonState();
        }

        private void show_pass(object sender, RoutedEventArgs e)
        {
            Uri imageUriSlash = new Uri("G:\\VS\\GreenTrail\\GreenTrail\\Source\\Image\\Assets\\eye-slash.png", UriKind.Absolute);
            Uri imageUri = new Uri("G:\\VS\\GreenTrail\\GreenTrail\\Source\\Image\\Assets\\eye.png", UriKind.Absolute);

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

            if (_isShowingPassword)
            {
                if (tb_pass != null)
                {
                    if (string.IsNullOrEmpty(tb_pass.Text) || tb_pass.Text == "Новый пароль" && string.IsNullOrEmpty(tb_confim_pass.Text) || tb_confim_pass.Text == "Повторите пароль")
                    {
                        areAllFieldsFilled = false;
                    }
                }
                if (tb_pass.Text != tb_confim_pass.Text || pb_pass.Password != pb_confim_pass.Password)
                {
                    l_pb_errorPass.Visibility = Visibility.Visible;
                    areAllFieldsFilled = false;
                    return;
                }
                else
                {
                    l_pb_errorPass.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(pb_pass.Password) || pb_pass.Password == "Пароль" && string.IsNullOrEmpty(pb_confim_pass.Password) || pb_confim_pass.Password == "Повторите пароль")
                {
                    areAllFieldsFilled = false;
                }
            }

            if (btn_NewPass != null)
            {
                // Включаем или отключаем кнопку "Отправить" в зависимости от результата проверки
                if (areAllFieldsFilled)
                {
                    btn_NewPass.IsEnabled = true;
                    btn_NewPass.Opacity = 100;
                }
            }
        }

        private void show_confim_pass(object sender, RoutedEventArgs e)
        {
            Uri imageUriSlash = new Uri("G:\\VS\\GreenTrail\\GreenTrail\\Source\\Image\\Assets\\eye-slash.png", UriKind.Absolute);
            Uri imageUri = new Uri("G:\\VS\\GreenTrail\\GreenTrail\\Source\\Image\\Assets\\eye.png", UriKind.Absolute);

            if (_isShowingPassword)
            {
                // Скрыть пароль
                pb_confim_pass.Password = tb_confim_pass.Text; // скопируем в TextBox из PasswordBox
                pb_confim_pass.Visibility = Visibility.Hidden; // TextBox - отобразить
                tb_confim_pass.Visibility = Visibility.Visible; // PasswordBox - скрыть
                im_confim_pass.Source = new BitmapImage(imageUriSlash);
            }
            else
            {
                // Показать пароль
                tb_confim_pass.Text = pb_confim_pass.Password; // скопируем в TextBox из PasswordBox
                pb_confim_pass.Visibility = Visibility.Visible; // TextBox - отобразить
                tb_confim_pass.Visibility = Visibility.Hidden; // PasswordBox - скрыть
                im_confim_pass.Source = new BitmapImage(imageUri);
            }

            UpdateSubmitButtonState();
            _isShowingPassword = !_isShowingPassword;
        }
    }
}
