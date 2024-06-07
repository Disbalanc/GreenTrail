using GreenTrail.Forms.Settings;
using GreenTrail.Source.Funs;
using System;
using System.Collections.Generic;
using System.Linq;
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
using LiveCharts;
using LiveCharts.Wpf;
using System.Configuration;

namespace GreenTrail.Forms.Data.ViewData
{
    /// <summary>
    /// Логика взаимодействия для ViewDataWindow.xaml
    /// </summary>
    public partial class ViewDataWindow : Window
    {

        private static GreanTrailEntities _context = GreanTrailEntities.GetContext();

        public ViewDataWindow()
        {
            InitializeComponent();

            SwapRoles();

            LoadDate();
        }

        public void SwapRoles()
        {
            string currentRole = DataBaseFuns.GetCurrentRole();

            switch (currentRole)
            {
                case "Администратор":
                    ti_Employees.Visibility = Visibility.Visible;
                    btn_createEmployees.Visibility = Visibility.Visible;

                    ti_Pollution.Visibility = Visibility.Visible;

                    ti_samples.Visibility = Visibility.Visible;

                    ti_Region.Visibility = Visibility.Visible;
                    btn_createRegion.Visibility = Visibility.Visible;

                    ti_sampleStudies.Visibility = Visibility.Visible;

                    break;
                case "Лаборант":
                    ti_Pollution.Visibility = Visibility.Visible;
                    btn_createPollution.Visibility = Visibility.Visible;

                    ti_samples.Visibility = Visibility.Visible;

                    ti_Region.Visibility = Visibility.Visible;

                    ti_sampleStudies.Visibility = Visibility.Visible;
                    btn_createContemplation.Visibility = Visibility.Visible;

                    break;
                case "Эколог":
                    ti_Pollution.Visibility = Visibility.Visible;

                    ti_samples.Visibility = Visibility.Visible;
                    btn_createEmployees.Visibility = Visibility.Visible;

                    ti_Region.Visibility = Visibility.Visible;
                    btn_createRegion.Visibility = Visibility.Visible;

                    ti_news.Visibility = Visibility.Visible;
                    btn_createNews.Visibility = Visibility.Visible;

                    ti_Events.Visibility = Visibility.Visible;
                    btn_createEvents.Visibility = Visibility.Visible;
                    break;
                default:
                    
                    break;
            }
        }

        private void LoadDate()
        {
            DrawPollution();
            DrawStudy();

            //// Получение текущего пользователя
            //var currentUser = _context.Users.FirstOrDefault(x => x.Roles == "имя пользователя");

            //// Проверка, принадлежит ли пользователь определенной роли
            //if (currentUser.Roles.Any(x => x.name == "имя роли"))
            //{
            //    // Пользователь принадлежит роли
            //}
            //else
            //{
            //    // Пользователь не принадлежит роли
            //}

            // Фильтрация и сортировка данных для графика загрязнений
            PollutionFilterComboBox.ItemsSource = pollutionData.Select(x => x.source).Distinct();
            PollutionFilterComboBox.SelectedIndex = 0;
            PollutionSortComboBox.ItemsSource = new[] { "По уровню загрязнения", "По источнику загрязнения" };
            PollutionSortComboBox.SelectedIndex = 0;

            // Фильтрация и сортировка данных для графика результатов изучения проб
            StudyFilterComboBox.ItemsSource = studyData.Select(x => x.typecontemplation).Distinct();
            StudyFilterComboBox.SelectedIndex = 0;
            StudySortComboBox.ItemsSource = new[] { "По результату", "По типу исследования" };
            StudySortComboBox.SelectedIndex = 0;

            SortEmployeesComboBox.ItemsSource = _context.Users.Select(x => x.Roles).ToList();
            SortEmployeesComboBox.SelectedIndex = 0;

            //SortNewsComboBox.ItemsSource = _context.News.Select(x => x.Users.full_name).ToList();
            //SortNewsComboBox.SelectedIndex = 0;

            SortSampleComboBox.ItemsSource = _context.Sample.Select(x => x.Region).ToList();
            SortSampleComboBox.SelectedIndex = 0;

            dg_employees.ItemsSource = _context.Users.ToList();
            //dg_events.ItemsSource = _context.Event.ToList();
            //dg_news.ItemsSource = _context.News.ToList();
            dg_region.ItemsSource = _context.Region.ToList();
            dg_sample.ItemsSource = _context.Sample.ToList();
        }

        // Получение данных о загрязнениях из БД
        private List<PollutionData> pollutionData = _context.Region_Pollution
            .ToList()
            .Select(x => new PollutionData
            {
                levels = x.Pollution.levels,
                name = x.Region.name,
                geographicalcoordinates = x.Region.geographical_coordinates
            })
            .ToList();

        // Получение данных об изучении проб из БД
        private List<StudyData> studyData = _context.Contemplation
            .ToList()
            .Select(x => new StudyData
            {
                fullname = x.Users.full_name,
                idsample = Convert.ToInt32(x.id_sample), //это костыль и надо бы починить на самописный артикул к каждой пробе
                typecontemplation = x.type_contemplation,
                name = x.Norm.name,
                result = Convert.ToInt32(x.result)
            })
            .ToList();

        // Отрисовка графика загрязнений
        private void DrawPollution()
        {
            // Фильтрация данных по источнику загрязнения
            var filteredPollutionData = pollutionData.Where(x => x.source == (string)PollutionFilterComboBox.SelectedValue);

            // Очистка графика
            PollutionChart.Series.Clear();

            // Добавление столбчатого графика
            PollutionChart.Series.Add(new ColumnSeries
            {
                Title = "Загрязнения",
                Values = new ChartValues<double>(filteredPollutionData.Select(x => Convert.ToDouble(x.levels)))
            });

            // Установка осей графика
            PollutionChart.AxisX.Clear();
            PollutionChart.AxisX.Add(new Axis
            {
                Title = "Загрязнители",
                Labels = filteredPollutionData.Select(x => x.source).ToList()
            });
            PollutionChart.AxisY.Clear();
            PollutionChart.AxisY.Add(new Axis
            {
                Title = "Уровень загрязнения",
                LabelFormatter = value => value.ToString("N")
            });
        }

        // Отрисовка графика результатов изучения проб
        private void DrawStudy()
        {

            // Фильтрация данных по типу исследования
            var filteredStudyData = studyData.Where(x => x.typecontemplation == (string)StudyFilterComboBox.SelectedValue);

            // Очистка графика
            StudyChart.Series.Clear();

            // Добавление столбчатого графика
            StudyChart.Series.Add(new ColumnSeries
            {
                Title = "Результаты изучения проб",
                Values = new ChartValues<double>(filteredStudyData.Select(x => Convert.ToDouble(x.result)))
            });

            // Установка осей графика
            StudyChart.AxisX.Clear();
            StudyChart.AxisX.Add(new Axis
            {
                Title = "Пробы",
                Labels = filteredStudyData.Select(x => x.idsample.ToString()).ToList()
            });
            StudyChart.AxisY.Clear();
            StudyChart.AxisY.Add(new Axis
            {
                Title = "Результат",
                LabelFormatter = value => value.ToString("N")
            });
        }

        private void PollutionFilterComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DrawPollution();
        }

        // Обработчик события изменения сортировки для графика загрязнений
        private void PollutionSortComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получение индекса выбранного элемента
            var selectedIndex = PollutionSortComboBox.SelectedIndex;

            // Сортировка данных по уровню загрязнения
            if (selectedIndex == 0)
            {
                pollutionData = pollutionData.OrderBy(x => x.levels).ToList();
            }
            // Сортировка данных по источнику загрязнения
            else if (selectedIndex == 1)
            {
                pollutionData = pollutionData.OrderBy(x => x.source).ToList();
            }

            DrawPollution();
        }

        private void EmployeesFilterTextBoxSelectionChanged(object sender, TextChangedEventArgs e)
        {
            // Получение введенного текста фильтра
            var filterText = (sender as TextBox).Text;

            // Фильтрация данных по введенному тексту
            var filteredEmployees = _context.Users
                .Where(x => x.full_name.Contains(filterText))
                .ToList();

            // Обновление источника данных `DataGrid` отфильтрованными данными
            dg_employees.ItemsSource = filteredEmployees;
        }

        // Обработчик события изменения сортировки
        private void EmployeesSortComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получение выбранного поля сортировки
            var selectedField = (sender as ComboBox).SelectedValue.ToString();

            //// Сортировка данных по выбранному полю
            //var sortedEmployees = _context.Users
            //    .OrderBy(x => x.GetType().GetProperty(selectedField).GetValue(x))
            //    .ToList();

            //// Обновление источника данных `DataGrid` отсортированными данными
            //dg_employees.ItemsSource = sortedEmployees;
        }

        private void SampleFilterTextBoxSelectionChanged(object sender, TextChangedEventArgs e)
        {
            // Получение введенного текста фильтра
            var filterText = (sender as TextBox).Text;

            // Фильтрация данных по введенному тексту
            var filteredEmployees = _context.Sample
                .Where(x =>  x.id_sample.ToString().Contains(filterText))
                .ToList();

            // Обновление источника данных `DataGrid` отфильтрованными данными
            dg_sample.ItemsSource = filteredEmployees;
        }

        // Обработчик события изменения сортировки
        private void SampleSortComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получение выбранного поля сортировки
            var selectedField = (sender as ComboBox).SelectedValue.ToString();

            //// Сортировка данных по выбранному полю
            //var sortedEmployees = _context.Sample
            //    .OrderBy(x => x.GetType().GetProperty(selectedField).GetValue(x))
            //    .ToList();

            //// Обновление источника данных `DataGrid` отсортированными данными
            //dg_sample.ItemsSource = sortedEmployees;
        }

        private void NewsFilterTextBoxSelectionChanged(object sender, TextChangedEventArgs e)
        {
            //// Получение введенного текста фильтра
            //var filterText = (sender as TextBox).Text;

            //// Фильтрация данных по введенному тексту
            //var filteredEmployees = _context.News
            //    .Where(x => x.heading.Contains(filterText))
            //    .ToList();

            //// Обновление источника данных `DataGrid` отфильтрованными данными
            //dg_news.ItemsSource = filteredEmployees;
        }

        // Обработчик события изменения сортировки
        private void NewsSortComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получение выбранного поля сортировки
            var selectedField = (sender as ComboBox).SelectedValue.ToString();

            //// Сортировка данных по выбранному полю
            //var sortedEmployees = _context.News
            //    .OrderBy(x => x.GetType().GetProperty(selectedField).GetValue(x))
            //    .ToList();

            //// Обновление источника данных `DataGrid` отсортированными данными
            //dg_news.ItemsSource = sortedEmployees;
        }

        private void EventsFilterTextBoxSelectionChanged(object sender, TextChangedEventArgs e)
        {
            //// Получение введенного текста фильтра
            //var filterText = (sender as TextBox).Text;

            //// Фильтрация данных по введенному тексту
            //var filteredEmployees = _context.Event
            //    .Where(x => x.name.Contains(filterText))
            //    .ToList();

            //// Обновление источника данных `DataGrid` отфильтрованными данными
            //dg_events.ItemsSource = filteredEmployees;
        }

        private void RegionFilterTextBoxSelectionChanged(object sender, TextChangedEventArgs e)
        {
            // Получение введенного текста фильтра
            var filterText = (sender as TextBox).Text;

            // Фильтрация данных по введенному тексту
            var filteredEmployees = _context.Region
                .Where(x => x.name.Contains(filterText))
                .ToList();

            // Обновление источника данных `DataGrid` отфильтрованными данными
            dg_region.ItemsSource = filteredEmployees;
        }

        // Обработчик события изменения фильтра для графика результатов изучения проб
        private void StudyFilterComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DrawStudy();
        }

        // Обработчик события изменения сортировки для графика результатов изучения проб
        private void StudySortComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получение индекса выбранного элемента
            var selectedIndex = StudySortComboBox.SelectedIndex;

            // Сортировка данных по результату
            if (selectedIndex == 0)
            {
                studyData = studyData.OrderBy(x => x.result).ToList();
            }
            // Сортировка данных по типу исследования
            else if (selectedIndex == 1)
            {
                studyData = studyData.OrderBy(x => x.typecontemplation).ToList();
            }

            DrawStudy();
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
    }
}