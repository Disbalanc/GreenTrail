using GreenTrail.Forms.Settings;
using GreenTrail.Source.Funs;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Drawing;
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


namespace GreenTrail.Forms
{
    /// <summary>
    /// Логика взаимодействия для MapWindow.xaml
    /// </summary>
    public partial class MapWindow : Window
    {

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

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
        }

        private void LogOutClick(object sender, RoutedEventArgs e)
        {
            Funs.SaveRememberMe();
            this.Close();
        }

        private void sp_dragMove_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Funs.WindowDragMove(sender, e, this);
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        public MapWindow()
        {
            InitializeComponent();

            DrawMap();
        }

        private void DrawMap()
        {
            // Создайте слой карты
            MapLayer layer = new MapLayer();

            // Создайте маркеры для каждой таблицы
            using (var dbContext = new GreanTrailEntities())
            {
                var events = dbContext.Event.ToList();
                var pollution = dbContext.Pollution.ToList();
                var sample = dbContext.Sample.ToList();
                var contemplation = dbContext.Contemplation.ToList();

                foreach (var item in events)
                {
                    // Создайте маркер для новостей
                    Pushpin eventPin = new Pushpin();
                    string[] coordinates = item.Region.geographical_coordinates.Split(';');
                    double longitude = double.Parse(coordinates[1]);
                    double latitude = double.Parse(coordinates[0]);
                    eventPin.Location = new Location(latitude, longitude+0.0001);
                    eventPin.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Source/Image/Assets/event.png", UriKind.Absolute)));

                    // Add tooltip to display information on hover
                    ToolTip tooltip = new ToolTip();
                    tooltip.Content = $"Название мероприятия: {item.name}\nДата: {item.data_time}";
                    eventPin.ToolTip = tooltip;

                    myMap.Children.Add(eventPin);
                }

                foreach (var item in sample)
                {
                    // Создайте маркер для новостей
                    Pushpin samplePin = new Pushpin();
                    string[] coordinates = item.Region.geographical_coordinates.Split(';');
                    double longitude = double.Parse(coordinates[1]);
                    double latitude = double.Parse(coordinates[0]);
                    samplePin.Location = new Location(latitude, longitude);
                    samplePin.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Source/Image/Assets/sample.png", UriKind.Absolute)));

                    // Add tooltip to display information on hover
                    ToolTip tooltip = new ToolTip();
                    tooltip.Content = $"проба: {item.articul}\nСборщик пробы: {item.Users.full_name}\nТип: {item.Type.name}";
                    samplePin.ToolTip = tooltip;

                    myMap.Children.Add(samplePin);
                }

                foreach (var item in contemplation)
                {
                    // Создайте маркер для новостей
                    Pushpin contemplationPin = new Pushpin();
                    string[] coordinates = item.Sample.Region.geographical_coordinates.Split(';');
                    double longitude = double.Parse(coordinates[1]);
                    double latitude = double.Parse(coordinates[0]);
                    contemplationPin.Location = new Location(latitude, longitude);
                    contemplationPin.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Source/Image/Assets/studied_sample.png", UriKind.Absolute)));

                    // Add tooltip to display information on hover
                    ToolTip tooltip = new ToolTip();
                    tooltip.Content = $"Изучение пробы: {item.id_contemplation}\nЛаборант изучивший: {item.Users.full_name}\nРезультат: {item.result}";
                    contemplationPin.ToolTip = tooltip;

                    myMap.Children.Add(contemplationPin);
                }

                foreach (var item in pollution)
                {
                    // Создайте маркер для загрязнения
                    Pushpin pollutionPin = new Pushpin();
                    string[] coordinates = item.Region.geographical_coordinates.Split(';');
                    double longitude = double.Parse(coordinates[1]);
                    double latitude = double.Parse(coordinates[0]);
                    pollutionPin.Location = new Location(latitude, longitude);
                    pollutionPin.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Source/Image/Assets/pollution.png", UriKind.Absolute)));

                    // Add tooltip to display information on hover
                    ToolTip tooltip = new ToolTip();
                    tooltip.Content = $"Загрязнение №: {item.id_pollution}\nуровень загрязнения: {item.levels}\nТип пробы: {item.Contemplation.Sample.Type.name}\n Найденный элемент: {item.Contemplation.type_contemplation}";
                    pollutionPin.ToolTip = tooltip;

                    myMap.Children.Add(pollutionPin);
                }
                // Добавьте слой к карте
                myMap.Children.Add(layer);
            }
        }
    }
}
