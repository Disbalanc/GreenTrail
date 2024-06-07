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
    /// Логика взаимодействия для ForgotPasswordPage.xaml
    /// </summary>
    public partial class ForgotPasswordPage : Page
    {
        public ForgotPasswordPage()
        {
            InitializeComponent();
        }
        private void btn_forgotPass_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tb_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox TB = sender as TextBox;
            if (TB.Text == "Email") TB.Text = string.Empty;
        }

        private void tb_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.Text == string.Empty)
            {
                // Получаем ресурс SolidColorBrush
                SolidColorBrush errorColor = (SolidColorBrush)FindResource("Error");
                textBox.BorderBrush = errorColor;
                textBox.Text = "Email";
            }
        }

        private void tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Проверяем, заполнены ли все TextBox и PasswordBox
            bool areAllFieldsFilled = true;

            if (string.IsNullOrEmpty(tb_Email.Text) || tb_Email.Text == "Email")
            {
                areAllFieldsFilled = false;
                return;
            }

            // Проверка правильности электронной почты
            if (!Funs.ValidateEmail(tb_Email.Text))
            {
                l_pb_errorEmail.Visibility = Visibility.Visible;
                areAllFieldsFilled = false;
                return;
            }
            else
            {
                // Если электронная почта правильная, скрываем сообщение об ошибке
                l_pb_errorEmail.Visibility = Visibility.Collapsed;
            }

            if (btn_SendOTP != null)
            {
                // Включаем или отключаем кнопку "Отправить" в зависимости от результата проверки
                if (areAllFieldsFilled)
                {
                    btn_SendOTP.IsEnabled = true;
                    btn_SendOTP.Opacity = 100;
                }
            }
        }
        private void btn_LoginWin_Click(object sender, RoutedEventArgs e)
        {
            // Создаем новую страницу Login
            LoginWindow login = new LoginWindow();

            // Отображаем страницу Login в основном окне
            this.Content = login;
        }

        public static string OTP;

        private void btn_SendOTP_Click(object sender, RoutedEventArgs e)
        {

                if (!DataBaseFuns.SeartchEmail(tb_Email.Text)) 
                {
                    MessageBox.Show("Упс, такого Email не зарегестрировано!", "Упс", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
                else
                {
                    OTP = Funs.GenerateOTP();
                    DataBaseFuns.SendOTP(tb_Email.Text, OTP);

                    // Создаем новую страницу OTPPass
                    OTPpassPage otpPass = new OTPpassPage();

                    // Отображаем страницу OTPPass в основном окне
                    this.Content = otpPass;
                }
        }
    }
}