using GreenTrail.Forms.Settings;
using GreenTrail.Forms.Welcome;
using GreenTrail.Source.Funs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GreenTrail.Forms.Data.AddData
{
    /// <summary>
    /// Логика взаимодействия для AddDataWindow.xaml
    /// </summary>
    public partial class AddDataWindow : Window
    {
        public string table { get; set; }

        private double _verticalOffset;

        private GreanTrailEntities _context = new GreanTrailEntities();

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

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }

        private void LogOutClick(object sender, RoutedEventArgs e)
        {
            Funs.SaveRememberMe();
            this.Close();
        }

    private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        // Вычисление высоты пустого пространства
        double emptySpaceHeight = scrollBar.ActualHeight - scrollBar.ViewportSize;

        // Установка максимального значения прокрутки
        scrollBar.Maximum = StackPanelMain.ActualHeight - scrollBar.ViewportSize - emptySpaceHeight;
    }

    private void ScrollBar_Scroll(object sender, ScrollEventArgs e)
    {
        // Обновление вертикального смещения
        _verticalOffset = e.NewValue;

        // Применение вертикального смещения к контенту
        StackPanelMain.Margin = new Thickness(0, -_verticalOffset, 0, 0);
    }

    private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        // Обновление вертикального смещения
        _verticalOffset -= e.Delta / 5;

        // Ограничение вертикального смещения максимальным и минимальным значениями
        _verticalOffset = Math.Min(Math.Max(_verticalOffset, scrollBar.Minimum), scrollBar.Maximum);

        // Применение вертикального смещения к StackPanel
        StackPanelMain.Margin = new Thickness(0, -_verticalOffset, 0, 0);
    }

    public AddDataWindow()
        {
            InitializeComponent();

            // Обработчик события прокрутки для ScrollBar
            scrollBar.Scroll += ScrollBar_Scroll;
        }

        private void Entity_SelectionChanged()
        {
            tblock_entity.Text = $"Сущность: {table}";

            // Заполнение выпадающего списка регионов
            SampleRegionComboBox.ItemsSource = _context.Region.ToList();
            SampleRegionComboBox.DisplayMemberPath = "name";
            SampleRegionComboBox.SelectedValuePath = "id_region";

            // Заполнение выпадающего списка проб
            ContemplationSampleComboBox.ItemsSource = _context.Sample.ToList();
            ContemplationSampleComboBox.DisplayMemberPath = "name";
            ContemplationSampleComboBox.SelectedValuePath = "id_sample";
            
            // Заполнение выпадающего списка Тип проб
            SampleTypeComboBox.ItemsSource = _context.Type.ToList();
            SampleTypeComboBox.DisplayMemberPath = "name";
            SampleTypeComboBox.SelectedValuePath = "id_type";

            // Заполнение выпадающего списка норм
            ContemplationNormComboBox.ItemsSource = _context.Norm.ToList();
            ContemplationNormComboBox.DisplayMemberPath = "name";
            ContemplationNormComboBox.SelectedValuePath = "id_norm";

            // Заполнение выпадающего списка ролей
            UserRolesComboBox.ItemsSource = _context.Users.ToList();
            UserRolesComboBox.DisplayMemberPath = "name";
            UserRolesComboBox.SelectedValuePath = "id_roles";

            // Отображение и скрытие элементов управления в зависимости от выбранной сущности
            switch (table)
            {
                case "Образец":
                    SampleRegionTextBlock.Visibility = Visibility.Visible;
                    SampleRegionComboBox.Visibility = Visibility.Visible;
                    SampleTypeTextBlock.Visibility = Visibility.Visible;
                    SampleTypeComboBox.Visibility = Visibility.Visible;
                    SampleArticulTextBox.Visibility = Visibility.Visible;
                    SampleArticulTextBlock.Visibility = Visibility.Visible;
                    AddRegionButton.Visibility = Visibility.Visible;
                    break;
                case "Изучение пробы":
                    ContemplationSampleTextBlock.Visibility = Visibility.Visible;
                    ContemplationSampleComboBox.Visibility = Visibility.Visible;
                    ContemplationNormTextBlock.Visibility = Visibility.Visible;
                    ContemplationNormComboBox.Visibility = Visibility.Visible;
                    ContemplationResultTextBlock.Visibility = Visibility.Visible;
                    ContemplationResultTextBox.Visibility = Visibility.Visible;
                    ContemplationTypeContemplationTextBlock.Visibility = Visibility.Visible;
                    ContemplationTypeContemplationTextBox.Visibility = Visibility.Visible;
                    AddNormButton.Visibility = Visibility.Visible;
                    break;
                case "Пользователи":
                    UserAddressTextBlock.Visibility = Visibility.Visible;
                    UserAddressTextBox.Visibility = Visibility.Visible;
                    UserDateOfBirthTextBlock.Visibility = Visibility.Visible;
                    UserDateOfBirthTextBox.Visibility = Visibility.Visible;
                    UserEmailTextBlock.Visibility = Visibility.Visible;
                    UserEmailTextBox.Visibility = Visibility.Visible;
                    UserFullNameTextBlock.Visibility = Visibility.Visible;
                    UserFullNameTextBox.Visibility = Visibility.Visible;
                    UserLoginTextBlock.Visibility = Visibility.Visible;
                    UserLoginTextBox.Visibility = Visibility.Visible;
                    UserPassTextBlock.Visibility = Visibility.Visible;
                    UserPassTextBox.Visibility = Visibility.Visible;
                    UserPhoneNumberTextBlock.Visibility = Visibility.Visible;
                    UserPhoneNumberTextBox.Visibility = Visibility.Visible;
                    UserRolesComboBox.Visibility = Visibility.Visible;
                    UserRolesTextBlock.Visibility = Visibility.Visible;
                    break;
                case "Мероприятие":
                    tblock_heading.Visibility = Visibility.Visible;
                    TextBox_heading.Visibility = Visibility.Visible;
                    TextBlock_text.Visibility = Visibility.Visible;
                    TextBox_text.Visibility = Visibility.Visible;
                    tblock_heading.Text = "Название";

                    Calendar.Visibility = Visibility.Visible;

                   
                    break;
                case "Новость":
                    tblock_heading.Visibility = Visibility.Visible;
                    TextBox_heading.Visibility = Visibility.Visible;
                    TextBlock_text.Visibility = Visibility.Visible;
                    TextBox_text.Visibility = Visibility.Visible;

                    Calendar.Visibility = Visibility.Visible;
                    break;
                case "Рекомендация":
                    tblock_heading.Visibility = Visibility.Visible;
                    TextBox_heading.Visibility = Visibility.Visible;
                    TextBlock_text.Visibility = Visibility.Visible;
                    TextBox_text.Visibility = Visibility.Visible;

                    break;
            }
        }

        private void AddRegionButton_Click(object sender, RoutedEventArgs e)
        {
            // Создание нового модального диалогового окна для добавления региона
            AddRegionDialog addRegionDialog = new AddRegionDialog();

            // Отображение диалогового окна
            addRegionDialog.ShowDialog();

            // Заполнение выпадающего списка регионов
            SampleRegionComboBox.ItemsSource = _context.Region.ToList();
            SampleRegionComboBox.DisplayMemberPath = "name";
            SampleRegionComboBox.SelectedValuePath = "id_region";
        }

        private void AddNormButton_Click(object sender, RoutedEventArgs e)
        {
            // Создание нового модального диалогового окна для добавления региона
            AddNormDialog addNormDialog = new AddNormDialog();

            // Отображение диалогового окна
            addNormDialog.ShowDialog();

            // Заполнение выпадающего списка норм
            ContemplationNormComboBox.ItemsSource = _context.Norm.ToList();
            ContemplationNormComboBox.DisplayMemberPath = "name";
            ContemplationNormComboBox.SelectedValuePath = "id_norm";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Получение выбранной сущности
            string selectedEntity = table;

            switch (selectedEntity)
            {
                case "Образец":
                    if (SampleArticulTextBox.Text != string.Empty && SampleRegionComboBox.SelectedItem != null && SampleTypeComboBox.SelectedItem != null)
                    {
                        Sample sample = new Sample
                        {
                            id_region = (int)SampleRegionComboBox.SelectedValue,
                            id_user = 1, // Взять из текущего пользователя
                            articul = SampleArticulTextBox.Text,
                            id_type = (int)SampleTypeComboBox.SelectedValue
                        };
                        _context.Sample.Add(sample);
                    }
                    else { MessageBox.Show("Вы не ввели все данные!", "Erorr", MessageBoxButton.OK, MessageBoxImage.Information); return; }

                    break;
                case "Изучение пробы":
                    if (ContemplationTypeContemplationTextBox.Text != string.Empty && ContemplationSampleComboBox.SelectedItem != null && ContemplationResultTextBox.Text != string.Empty && ContemplationNormComboBox.SelectedItem != null)
                    {
                        Contemplation contemplation = new Contemplation
                        {
                            id_sample = (int)ContemplationSampleComboBox.SelectedValue,
                            id_user = 1, // Взять из текущего пользователя
                            type_contemplation = ContemplationTypeContemplationTextBox.Text,
                            result = ContemplationResultTextBox.Text, // Временно задано произвольное значение
                            id_norm = (int)ContemplationNormComboBox.SelectedValue
                        };
                        Sample sample = GreanTrailEntities.GetContext().Sample.FirstOrDefault(r => r.articul == (string)SampleRegionComboBox.SelectedItem); 
                        Norm norma = GreanTrailEntities.GetContext().Norm.FirstOrDefault(r => r.id_norm == (long)ContemplationNormComboBox.SelectedValue && r.id_type == sample.id_type);
                        if (Convert.ToInt32(norma.norma) < Convert.ToInt32(ContemplationResultTextBox.Text))
                        {
                            string v = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                            Pollution pollution = new Pollution
                            {
                                id_contemplation = contemplation.id_contemplation,
                                levels = (Convert.ToInt32(norma.norma) - Convert.ToInt32(ContemplationResultTextBox.Text)).ToString(),
                                //id_region = GreanTrailEntities.GetContext().Region.FirstOrDefault(r => r.id_region == sample.id_region).ToString(),
                                //data_time = v
                            };
                        }
                        _context.Contemplation.Add(contemplation);
                    }
                    else { MessageBox.Show("Вы не ввели все данные!", "Erorr", MessageBoxButton.OK, MessageBoxImage.Information); return; }
                    break;
                case "Пользователи":
                    if (UserLoginTextBox.Text !=string.Empty && UserLoginTextBox.Text != string.Empty)
                    {
                        Users users = new Users
                        {
                            full_name = UserFullNameTextBox.Text,
                            login = UserLoginTextBox.Text,
                            password = Funs.ComputeSHA256Hash(UserPassTextBox.Text),
                            id_roles = (int)UserRolesComboBox.SelectedValue,
                            dateOfBirth = UserDateOfBirthTextBox.Text,
                            phoneNumber = UserPhoneNumberTextBox.Text,
                            address = UserAddressTextBox.Text,
                            email = UserEmailTextBox.Text,
                        };
                        _context.Users.Add(users);
                    }
                    else { MessageBox.Show("Вы не ввели все обязательные данные!", "Erorr", MessageBoxButton.OK, MessageBoxImage.Information); return; }

                    break;
                case "Мероприятие":
                    if (UserLoginTextBox.Text != string.Empty && UserLoginTextBox.Text != string.Empty)
                    {
                        Event events = new Event
                        {
                            name = tblock_heading.Text,
                            data_time = Calendar.SelectedDate
                        };
                        _context.Event.Add(events);
                    }
                    else { MessageBox.Show("Вы не ввели все обязательные данные!", "Erorr", MessageBoxButton.OK, MessageBoxImage.Information); return; }
                    break;
                case "Новость":
                    if (UserLoginTextBox.Text != string.Empty && UserLoginTextBox.Text != string.Empty)
                    {
                        News news = new News
                        {
                            //id_user = GreanTrailEntities.CurrentUserId,
                            heading = tblock_heading.Text,
                            text = TextBox_text.Text,
                            data_time = Calendar.SelectedDate
                        };
                        _context.News.Add(news);
                    }
                    else { MessageBox.Show("Вы не ввели все обязательные данные!", "Erorr", MessageBoxButton.OK, MessageBoxImage.Information); return; }
                    break;
                case "Рекомендация":
                    if (UserLoginTextBox.Text != string.Empty && UserLoginTextBox.Text != string.Empty)
                    {
                        EcologicalRecommendations ecologicalRecommendations = new EcologicalRecommendations
                        {
                            //id_user = GreanTrailEntities.CurrentUserId,
                            heading = tblock_heading.Text,
                            text = TextBox_text.Text,

                        };
                        _context.EcologicalRecommendations.Add(ecologicalRecommendations);
                    }
                    else { MessageBox.Show("Вы не ввели все обязательные данные!", "Erorr", MessageBoxButton.OK, MessageBoxImage.Information); return; }
                    break;
            }
            _context.SaveChanges();
            // Вывод сообщения об успешном добавлении
            MessageBox.Show("Данные успешно добавлены в базу данных.");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Entity_SelectionChanged();

            // Вычисление высоты пустого пространства
            double emptySpaceHeight = scrollBar.ActualHeight - scrollBar.ViewportSize;

            // Установка максимального значения прокрутки
            scrollBar.Maximum = StackPanelMain.ActualHeight - scrollBar.ViewportSize - emptySpaceHeight;
        }
    }
}