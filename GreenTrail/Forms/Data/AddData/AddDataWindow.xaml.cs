using GreenTrail.Forms.Settings;
using GreenTrail.Forms.Welcome;
using GreenTrail.Source.Funs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

namespace GreenTrail.Forms.Data.AddData
{
    /// <summary>
    /// Логика взаимодействия для AddDataWindow.xaml
    /// </summary>
    public partial class AddDataWindow : Window
    {
        public string table { get; set; }

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

        public AddDataWindow()
        {
            InitializeComponent();

            Entity_SelectionChanged();
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

            // Заполнение выпадающего списка норм
            ContemplationNormComboBox.ItemsSource = _context.Norm.ToList();
            ContemplationNormComboBox.DisplayMemberPath = "name";
            ContemplationNormComboBox.SelectedValuePath = "id_norm";

            // Заполнение выпадающего списка пользователи
            ContemplationNormComboBox.ItemsSource = _context.Users.ToList();
            ContemplationNormComboBox.DisplayMemberPath = "name";
            ContemplationNormComboBox.SelectedValuePath = "id_user";

            // Отображение и скрытие элементов управления в зависимости от выбранной сущности
            switch (table)
            {
                case "Образец":
                    SampleRegionTextBlock.Visibility = Visibility.Visible;
                    SampleRegionComboBox.Visibility = Visibility.Visible;
                    AddRegionButton.Visibility = Visibility.Visible;

                    ContemplationSampleTextBlock.Visibility = Visibility.Collapsed;
                    ContemplationSampleComboBox.Visibility = Visibility.Collapsed;
                    ContemplationNormTextBlock.Visibility = Visibility.Collapsed;
                    ContemplationNormComboBox.Visibility = Visibility.Collapsed;
                    AddNormButton.Visibility = Visibility.Collapsed;
                    break;
                case "Изучение пробы":
                    ContemplationSampleTextBlock.Visibility = Visibility.Visible;
                    ContemplationSampleComboBox.Visibility = Visibility.Visible;
                    ContemplationNormTextBlock.Visibility = Visibility.Visible;
                    ContemplationNormComboBox.Visibility = Visibility.Visible;
                    AddNormButton.Visibility = Visibility.Visible;

                    SampleRegionTextBlock.Visibility = Visibility.Collapsed;
                    SampleRegionComboBox.Visibility = Visibility.Collapsed;
                    AddRegionButton.Visibility = Visibility.Collapsed;
                    break;
                case "Пользователи":
                    ContemplationSampleTextBlock.Visibility = Visibility.Visible;
                    ContemplationSampleComboBox.Visibility = Visibility.Visible;
                    ContemplationNormTextBlock.Visibility = Visibility.Visible;
                    ContemplationNormComboBox.Visibility = Visibility.Visible;
                    AddNormButton.Visibility = Visibility.Visible;

                    SampleRegionTextBlock.Visibility = Visibility.Collapsed;
                    SampleRegionComboBox.Visibility = Visibility.Collapsed;
                    AddRegionButton.Visibility = Visibility.Collapsed;
                    break;
                default:
                    SampleRegionTextBlock.Visibility = Visibility.Collapsed;
                    SampleRegionComboBox.Visibility = Visibility.Collapsed;
                    AddRegionButton.Visibility = Visibility.Collapsed;

                    ContemplationSampleTextBlock.Visibility = Visibility.Collapsed;
                    ContemplationSampleComboBox.Visibility = Visibility.Collapsed;
                    ContemplationNormTextBlock.Visibility = Visibility.Collapsed;
                    ContemplationNormComboBox.Visibility = Visibility.Collapsed;
                    AddNormButton.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void AddRegionButton_Click(object sender, RoutedEventArgs e)
        {
            // Создание нового модального диалогового окна для добавления региона
            AddRegionDialog addRegionDialog = new AddRegionDialog();

            // Отображение диалогового окна
            addRegionDialog.ShowDialog();

            // Если пользователь нажал кнопку "OK" в диалоговом окне
            if (addRegionDialog.DialogResult == true)
            {
                // Получение введенных данных
                string regionName = addRegionDialog.RegionName;
                string regionGeographicalCoordinates = addRegionDialog.RegionGeographicalCoordinates;

                // Создание нового региона
                Region region = new Region
                {
                    name = regionName,
                    geographical_coordinates = regionGeographicalCoordinates
                };

                // Добавление региона в БД
                _context.Region.Add(region);
                _context.SaveChanges();

                // Обновление выпадающего списка регионов
                SampleRegionComboBox.ItemsSource = _context.Region.ToList();
            }
        }

        private void AddNormButton_Click(object sender, RoutedEventArgs e)
        {
            // Создание нового модального диалогового окна для добавления региона
            AddNormDialog addNormDialog = new AddNormDialog();

            // Отображение диалогового окна
            addNormDialog.ShowDialog();

            // Если пользователь нажал кнопку "OK" в диалоговом окне
            if (addNormDialog.DialogResult == true)
            {
                // Получение введенных данных
                string normName = addNormDialog.NormName;
                string norma = addNormDialog.Norma;
                string typeNorma = addNormDialog.TypeNorma;

                // Создание нового региона
                Norm norm = new Norm
                {
                    name = normName,
                    norma = norma,
                    //typeNorm = typeNorma
                };

                // Добавление региона в БД
                _context.Norm.Add(norm);
                _context.SaveChanges();

                // Обновление выпадающего списка регионов
                SampleRegionComboBox.ItemsSource = _context.Norm.ToList();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Получение выбранной сущности
            string selectedEntity = table;

            switch (selectedEntity)
            {
                case "Образец":
                    Sample sample = new Sample
                    {
                        id_region = (int)SampleRegionComboBox.SelectedValue,
                        id_user = 1 // Взять из текущего пользователя
                    };
                    _context.Sample.Add(sample);
                    break;
                case "Изучение пробы":
                    Contemplation contemplation = new Contemplation
                    {
                        id_sample = (int)ContemplationSampleComboBox.SelectedValue,
                        id_user = 1, // Взять из текущего пользователя
                        type_contemplation = "Новый тип изучения пробы", // Временно задано произвольное значение
                        result = 0.ToString(), // Временно задано произвольное значение
                        id_norm = (int)ContemplationNormComboBox.SelectedValue
                    };
                    _context.Contemplation.Add(contemplation);
                    break;
                case "Пользователи":
                    Users users = new Users
                    {

                    };
                    _context.Users.Add(users);
                    break;
            }
            _context.SaveChanges();
            // Вывод сообщения об успешном добавлении
            MessageBox.Show("Данные успешно добавлены в базу данных.");
        }
    }
}
