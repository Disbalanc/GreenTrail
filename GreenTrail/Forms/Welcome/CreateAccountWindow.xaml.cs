using GreenTrail.Forms.Welcome.ForgotPasswprdPage;
using GreenTrail.Source.Funs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для CreateAccountWindow.xaml
    /// </summary>
    public partial class CreateAccountWindow : Window
    {
        public PasswordStrengthViewModel passwordStrengthViewModel = new PasswordStrengthViewModel();
        public CreateAccountWindow()
        {
            InitializeComponent();
            DataContext = passwordStrengthViewModel;
        }

        private void btn_regin_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                if(_isShowingPassword)
                {
                    DataBaseFuns.Regin(tb_FIO.Text, tb_email.Text, tb_login.Text, tb_Date_Birth.Text, tb_telephone_number.Text, tb_adress.Text, Funs.ComputeSHA256Hash(tb_pass.Text), this);
                }
                else
                {
                    DataBaseFuns.Regin(tb_FIO.Text, tb_email.Text, tb_login.Text, tb_Date_Birth.Text, tb_telephone_number.Text, tb_adress.Text, Funs.ComputeSHA256Hash(pb_pass.Password), this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Упс", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_loginWindow_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
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

        private  List<string> keywords = new List<string> { "ФИО", "Email", "Логин" , "Дата Рождения", "Телефонный номер", "Адресс", "Пароль", "Повторите пароль" };
        private void tb_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox TB = sender as TextBox;
            foreach (string keyword in keywords) //перебор из List
            {
                if (TB.Text.Contains(keyword)) //определение содержит ли из List
                {
                    TB.Text = string.Empty;
                }
            }
        }

        private void tb_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.Text == string.Empty) 
            {
                // Получаем ресурс SolidColorBrush
                SolidColorBrush errorColor = (SolidColorBrush)FindResource("Error");
                textBox.BorderBrush = errorColor;
                switch(textBox.Name)
                {
                    case ("tb_FIO"):
                        {
                            textBox.Text = "ФИО";
                            break;
                        }
                    case ("tb_email"):
                        {
                            textBox.Text = "Логин";
                            break;
                        }
                    case ("tb_login"):
                        {
                            textBox.Text = "Email";
                            break;
                        }
                    case ("tb_Date_Birth"):
                        {
                            textBox.Text = "Дата Рождения";
                            break;
                        }
                    case ("tb_telephone_number"):
                        {
                            textBox.Text = "Телефонный номер";
                            break;
                        }
                    case ("tb_adress"):
                        {
                            textBox.Text = "Адресс";
                            break;
                        }
                    case ("tb_pass"):
                        {
                            textBox.Text = "Пароль";
                            break;
                        }
                    case ("tb_confim_pass"):
                        {
                            textBox.Text = "Повторите пароль";
                            break;
                        }
                }
            } else 
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

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }

        private void UpdateSubmitButtonState()
        {
            // Список TextBox и PasswordBox
            List<TextBox> textBoxes = new List<TextBox> { tb_FIO, tb_email, tb_login, tb_Date_Birth, tb_telephone_number, tb_adress, tb_pass, tb_confim_pass };
            List<PasswordBox> passwordBoxes = new List<PasswordBox>{ pb_pass, pb_confim_pass };

            // Проверяем, заполнены ли все TextBox и PasswordBox
            bool areAllFieldsFilled = true;
            
            foreach (var textBox in textBoxes)
            {
                if (textBox != null)
                {
                    foreach (string keyword in keywords) //перебор из List
                    {
                        if (textBox.Text.Contains(keyword) || string.IsNullOrEmpty(textBox.Text)) //определение содержит ли из List
                        {
                            areAllFieldsFilled = false;
                            return;
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

                    if (textBox.Name == "tb_email")
                    {
                        // Проверка правильности электронной почты
                        if (!Funs.ValidateEmail(textBox.Text))
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
                    }
                }
            }

            foreach (var passwordBox in passwordBoxes)
            {
                if (passwordBox != null)
                {
                    if (string.IsNullOrEmpty(passwordBox.Password))
                    {
                        areAllFieldsFilled = false;
                        return;
                    }
                }
            }

            if (btn_regin != null)
            {
                // Проверяем, установлен ли CheckBox
                bool isCheckBoxChecked = cb_terms.IsChecked.HasValue && cb_terms.IsChecked.Value;

                // Включаем или отключаем кнопку "Отправить" в зависимости от результата проверки
                if (areAllFieldsFilled && isCheckBoxChecked)
                {

                    btn_regin.IsEnabled = true;
                    btn_regin.Opacity = 100;
                }
            }
        }

        private void tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Name == "tb_pass" && textBox.Text != "Пароль")
            {
                passwordStrengthViewModel.UpdatePasswordStrength(textBox.Text);
            }
            UpdateSubmitButtonState();
        }

        private void pb_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = (PasswordBox)sender;
            if (passwordBox.Password != "Пароль"|| passwordBox.Password != "Повторите пароль")
            passwordStrengthViewModel.UpdatePasswordStrength(passwordBox.Password);
            UpdateSubmitButtonState();
        }

        private void cb_terms_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateSubmitButtonState();
        }

        private void cb_terms_Checked(object sender, RoutedEventArgs e)
        {
            UpdateSubmitButtonState();
        }

        private bool _isShowingPassword = true;

        private void show_pass(object sender, RoutedEventArgs e)
        {
            Uri imageUriSlash = new Uri("G:\\VS\\GreenTrail\\GreenTrail\\Source\\Image\\Assets\\eye-slash.png", UriKind.Absolute);
            Uri imageUri = new Uri("G:\\VS\\GreenTrail\\GreenTrail\\Source\\Image\\Assets\\eye.png", UriKind.Absolute);

            if (_isShowingPassword)
            {
                // Скрыть пароль
                if(tb_pass.Text != "Пароль"|| tb_pass.Text != "Повторите пароль")
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

            UpdateSubmitButtonState();
            _isShowingPassword = !_isShowingPassword;
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

    public class PasswordStrengthViewModel : INotifyPropertyChanged
    {
        private double _passwordStrength;

        public double PasswordStrength
        {
            get { return _passwordStrength; }
            set
            {
                _passwordStrength = value;
                OnPropertyChanged("PasswordStrength");
            }
        }

        public PasswordStrengthViewModel()
        {
            // Инициализация начального значения силы пароля
            PasswordStrength = 0;
        }

        public void UpdatePasswordStrength(string password)
        {
            // Расчет силы пароля на основе правил сложности пароля

            // Примерный код для расчета силы пароля
            double strength = 0;

            if (password.Length > 8) strength += 16;
            if (password.Any(char.IsUpper)) strength += 16;
            if (password.Any(char.IsLower)) strength += 16;
            if (password.Any(char.IsDigit)) strength += 16;
            if (password.Any(char.IsSymbol)) strength += 16;
            if (password.Any(c => c == '!' || c == '$' || c == '^' || c == '%' || c == '&' || c == '#' || c == '@')) strength += 16;

            PasswordStrength = strength;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}