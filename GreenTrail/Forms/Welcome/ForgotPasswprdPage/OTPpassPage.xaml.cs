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
using System.Windows.Threading;

namespace GreenTrail.Forms.Welcome.ForgotPasswprdPage
{
    /// <summary>
    /// Логика взаимодействия для OTPpassPage.xaml
    /// </summary>
    public partial class OTPpassPage : Page
    {
        private int _secondsRemaining = 60;
        private DispatcherTimer _timer;

        public OTPpassPage()
        {
            InitializeComponent();
            // Создаем таймер с интервалом в 1 секунду
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };

            // Подписываемся на событие Tick таймера
            _timer.Tick += Timer_Tick;

            // Запускаем таймер
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Уменьшаем количество оставшихся секунд
            _secondsRemaining--;

            // Обновляем текст метки обратного отсчета
            l_timer.Content = _secondsRemaining.ToString();

            // Проверяем, закончился ли обратный отсчет
            if (_secondsRemaining == 0)
            {
                // Останавливаем таймер
                _timer.Stop();

                // Делаем видимой кнопку отправки OTP
                btn_SendOTP.Visibility = Visibility.Visible;

                // Делаем невидимыми метки "Отправляем OTP..." и обратного отсчета
                l_send.Visibility = Visibility.Hidden;
                l_timer.Visibility = Visibility.Hidden;
            }
        }

        private void tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text.Length == 1)
            {
                // Переходим на следующий текстовый блок
                textBox.Focus();
            }
            UpdateSubmitButtonState();
        }

        private void UpdateSubmitButtonState()
        {
            // Проверяем, заполнены ли все TextBox и PasswordBox
            bool areAllFieldsFilled = true;

            if (!string.IsNullOrEmpty(tb_num1.Text) && !string.IsNullOrEmpty(tb_num2.Text) && !string.IsNullOrEmpty(tb_num3.Text) && !string.IsNullOrEmpty(tb_num4.Text) && !string.IsNullOrEmpty(tb_num5.Text) && !string.IsNullOrEmpty(tb_num6.Text))
            {
                areAllFieldsFilled = false;
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

        private void btn_NewPass_Click(object sender, RoutedEventArgs e)
        {
            string OTP = tb_num1.Text+ tb_num2.Text+ tb_num3.Text + tb_num4.Text + tb_num5.Text + tb_num6.Text;
            if (Funs.ComputeSHA256Hash(OTP) != ForgotPasswordPage.OTP)
            {
                MessageBox.Show("Неверный OTP код!", "Упс", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Создаем новую страницу NewPass
            NewPasswordPage newPass = new NewPasswordPage();

            // Отображаем страницу NewPass в основном окне
            this.Content = newPass;
        }

        private void btn_SendOTP_Click(object sender, RoutedEventArgs e)
        {
            ForgotPasswordPage forgotPasswordPage = new ForgotPasswordPage();
            this.Content = forgotPasswordPage;
        }
    }
}
