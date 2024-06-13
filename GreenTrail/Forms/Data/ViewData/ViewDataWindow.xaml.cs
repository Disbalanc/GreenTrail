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
using DocumentFormat.OpenXml.Bibliography;
using System.Drawing;
using GreenTrail.Forms.Data.AddData;

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

                    dg_Employees.Visibility = Visibility.Visible;

                    ti_Pollution.Visibility = Visibility.Visible;

                    ti_samples.Visibility = Visibility.Visible;

                    ti_Region.Visibility = Visibility.Visible;
                    btn_createRegion.Visibility = Visibility.Visible;

                    ti_news.Visibility = Visibility.Visible;
                    btn_createNews.Visibility = Visibility.Visible;

                    ti_Events.Visibility = Visibility.Visible;
                    btn_createEvents.Visibility = Visibility.Visible;

                    ti_sampleStudies.Visibility = Visibility.Visible;

                    ti_EcologicalRecommendations.Visibility = Visibility.Visible;
                    btn_createEcologicalRecommendations.Visibility = Visibility.Visible;

                    ti_Norma.Visibility = Visibility.Visible;
                    break;
                case "Лаборант":
                    ti_Pollution.Visibility = Visibility.Visible;

                    ti_samples.Visibility = Visibility.Visible;

                    ti_Region.Visibility = Visibility.Visible;

                    ti_sampleStudies.Visibility = Visibility.Visible;
                    btn_createContemplation.Visibility = Visibility.Visible;

                    ti_EcologicalRecommendations.Visibility = Visibility.Visible;
                    btn_createEcologicalRecommendations.Visibility = Visibility.Visible;

                    ti_Norma.Visibility = Visibility.Visible;
                    btn_createNorma.Visibility = Visibility.Visible;
                    break;
                case "Эколог":
                    ti_Pollution.Visibility = Visibility.Visible;

                    ti_Norma.Visibility = Visibility.Visible;

                    ti_samples.Visibility = Visibility.Visible;
                    btn_createSamle.Visibility = Visibility.Visible;

                    ti_Region.Visibility = Visibility.Visible;
                    btn_createRegion.Visibility = Visibility.Visible;

                    ti_news.Visibility = Visibility.Visible;
                    btn_createNews.Visibility = Visibility.Visible;

                    ti_Events.Visibility = Visibility.Visible;
                    btn_createEvents.Visibility = Visibility.Visible;

                    ti_Norma.Visibility = Visibility.Visible;

                    ti_EcologicalRecommendations.Visibility = Visibility.Visible;
                    btn_createEcologicalRecommendations.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void LoadDate()
        {
            DrawPollution();
            DrawStudy();
            DrawEmployess();

            // Фильтрация и сортировка данных для графика загрязнений
            PollutionFilterComboBox.ItemsSource = _context.Region.Select(x => x.name).ToList();
            PollutionFilterComboBox.SelectedIndex = 0;
            PollutionSortComboBox.ItemsSource = new[] { "По уровню загрязнения", "По источнику загрязнения" };
            PollutionSortComboBox.SelectedIndex = 0;

            // Фильтрация и сортировка данных для графика результатов изучения проб
            StudyFilterComboBox.ItemsSource = _context.Norm.Select(x => x.name).ToList();
            StudyFilterComboBox.SelectedIndex = 0;
            StudyFilterTypeComboBox.ItemsSource = _context.Type.Select(x => x.name).ToList();
            StudyFilterTypeComboBox.SelectedIndex = 0;
            StudyFilterRegionComboBox.ItemsSource = _context.Region.Select(x => x.name).ToList();
            StudyFilterRegionComboBox.SelectedIndex = 0;
            StudySortComboBox.ItemsSource = new[] { "По результату", "По типу исследования" };
            StudySortComboBox.SelectedIndex = 0;

            SortEmployeesComboBox.ItemsSource = SortEmployeesComboBox.ItemsSource = _context.Roles
    .Select(x => x.name)
    .Distinct()
    .OrderBy(role => role)
    .Concat(new[] { "Все" })
    .ToList();
            SortEmployeesComboBox.SelectedIndex = 0;

            SortSampleComboBox.ItemsSource = _context.Region.Select(x => x.name).ToList();
            SortSampleComboBox.SelectedIndex = 0;
            SortSampleTypeComboBox.ItemsSource = _context.Type.Select(x => x.name).ToList();
            SortSampleTypeComboBox.SelectedIndex = 0;

            dg_employees.ItemsSource = _context.Users.ToList();
            dg_contemplation.ItemsSource = _context.Contemplation.ToList();
            dg_pollution.ItemsSource = _context.Pollution.ToList();
            dg_events.ItemsSource = _context.Event.ToList();
            dg_news.ItemsSource = _context.News.ToList();
            dg_region.ItemsSource = _context.Region.ToList();
            dg_sample.ItemsSource = _context.Sample.ToList();
            dg_Norma.ItemsSource = _context.Norm.ToList();
            dg_EcologicalRecommendations.ItemsSource = _context.EcologicalRecommendations.ToList();
        }

        // Получение данных о загрязнениях из БД
        private List<PollutionData> pollutionData = _context.Pollution
            .ToList()
            .Select(x => new PollutionData
            {
                id = x.id_pollution,
                levels = x.levels.ToString(),
                source = x.Region.name,
                geographicalcoordinates = x.Region.geographical_coordinates
            })
            .ToList();

        // Получение данных об изучении проб из БД
        private List<StudyData> studyData = _context.Contemplation
            .ToList()
            .Select(x => new StudyData
            {
                fullname = x.Users.full_name,
                articulSample = x.Sample.articul,
                type = x.Norm?.name,
                region = x.Sample.Region.name,
                name = x.Norm.name,
                result = Convert.ToDouble(x.result)
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
            var filteredStudyData = studyData.Where(x => x.type == (string)StudyFilterComboBox.SelectedValue);

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
                Labels = filteredStudyData.Select(x => x.articulSample.ToString()).ToList()
            });
            StudyChart.AxisY.Clear();
            StudyChart.AxisY.Add(new Axis
            {
                Title = "Результат",
                LabelFormatter = value => value.ToString("N")
            });
        }

        private void DrawEmployess()
        {

            // Фильтрация данных по типу исследования
            var filteredEmployeeData = (string)SortEmployeesComboBox.SelectedValue;
            var employees = _context.Users.ToList();
            if (filteredEmployeeData != "Все")
            {
                employees = _context.Users.Where(r => r.Roles.name == filteredEmployeeData).ToList();
            }

            // Очистка графика
            EmployeeWorkChart.Series.Clear();

            // Generate a single list of unique dates for the X-axis labels
            var allDates = employees
                .SelectMany(employee => _context.Sample
                    .Where(s => s.id_user == employee.id_user)
                    .Select(s => s.date_sample)
                    .Union(_context.News
                        .Where(n => n.id_user == employee.id_user)
                        .Select(n => n.data_time))
                    .Union(_context.Contemplation
                        .Where(c => c.id_user == employee.id_user)
                        .Select(c => c.date_contemplation)))
                .Distinct()
                .OrderBy(date => date)
                .ToList();

            // Set up the chart axes
            EmployeeWorkChart.AxisX.Clear();
            EmployeeWorkChart.AxisX.Add(new Axis
            {
                Title = "Дата",
                Labels = allDates.Select(date => string.Format("{0:dd-MM-yyyy HH:mm}", date)).ToList()
            });

            EmployeeWorkChart.AxisY.Clear();
            EmployeeWorkChart.AxisY.Add(new Axis
            {
                Title = "Объем работы",
                LabelFormatter = value => value.ToString("N")
            });

            foreach (var employee in employees)
            {
                var employeeData = allDates
                    .Select(date => new DataPoint
                    {
                        Date = (DateTime)date, // explicit cast to DateTime
                        Value = _context.Sample.Count(s => s.id_user == employee.id_user && s.date_sample == date) +
                                _context.News.Count(n => n.id_user == employee.id_user && n.data_time == date) +
                                _context.Contemplation.Count(c => c.id_user == employee.id_user && c.date_contemplation == date),
                        Type = "Sample"
                    })
                    .ToList();

                // Create a line series for this employee
                EmployeeWorkChart.Series.Add(new LineSeries
                {
                    Title = $"{employee.full_name}\n",
                    Values = new ChartValues<double>(employeeData.Select(x => x.Value)),
                });
            }
        }

        public class DataPoint
        {
            public DateTime Date { get; set; }
            public double Value { get; set; }
            public string Type { get; set; }
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
                .Where(x => x.full_name.ToLower().Contains(filterText.ToLower())|| x.Roles.name.ToLower().Contains(filterText.ToLower()) || x.dateOfBirth.ToLower().Contains(filterText.ToLower()) || x.email.ToLower().Contains(filterText.ToLower()) || x.phoneNumber.ToLower().Contains(filterText.ToLower()) || x.address.ToLower().Contains(filterText.ToLower()))
                .ToList();

            if (filterText == string.Empty) filteredEmployees = _context.Users.ToList();

            // Обновление источника данных `DataGrid` отфильтрованными данными
            dg_employees.ItemsSource = filteredEmployees;
        }

        // Обработчик события изменения сортировки
        private void EmployeesSortComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            // Получение выбранного поля сортировки
            var selectedField = (sender as ComboBox).SelectedValue.ToString();
            var sortedEmployees = _context.Users.ToList();
            if (selectedField != "Все")
            {
                // Сортировка данных по выбранному полю
                sortedEmployees = _context.Users.Where(x => x.Roles.name.Contains(selectedField)).ToList();
            }
            

            // Обновление источника данных `DataGrid` отсортированными данными
            dg_employees.ItemsSource = sortedEmployees;

            DrawEmployess();
        }

        private void SampleFilterTextBoxSelectionChanged(object sender, TextChangedEventArgs e)
        {
            // Получение введенного текста фильтра
            var filterText = (sender as TextBox).Text;

            // Фильтрация данных по введенному тексту
            var filteredSample = _context.Sample
                .Where(x =>  x.articul.ToString().ToLower().Contains(filterText.ToLower()))
                .ToList();

            if (filterText == string.Empty) filteredSample = _context.Sample.ToList();

            // Обновление источника данных `DataGrid` отфильтрованными данными
            dg_sample.ItemsSource = filteredSample;
        }

        // Обработчик события изменения сортировки
        private void SampleSortComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получение выбранного поля сортировки
            var selectedField = (sender as ComboBox).SelectedValue.ToString();

            // Сортировка данных по выбранному полю
            var sortedEmployees = _context.Sample
                .Where(x => x.Region.name.Contains(selectedField)||x.Type.name.Contains(selectedField))
                .ToList();

            // Обновление источника данных `DataGrid` отсортированными данными
            dg_sample.ItemsSource = sortedEmployees;
        }

        private void NewsFilterTextBoxSelectionChanged(object sender, TextChangedEventArgs e)
        {
            // Получение введенного текста фильтра
            var filterText = (sender as TextBox).Text;

            // Фильтрация данных по введенному тексту
            var filteredNews = _context.News.Where(x => x.heading.ToLower().Contains(filterText.ToLower())|| x.text.ToLower().Contains(filterText.ToLower())).ToList();

            if (filterText == string.Empty) filteredNews = _context.News.ToList();

            // Обновление источника данных `DataGrid` отфильтрованными данными
            dg_news.ItemsSource = filteredNews;
        }

        private void EventsFilterTextBoxSelectionChanged(object sender, TextChangedEventArgs e)
        {
            // Получение введенного текста фильтра
            var filterText = (sender as TextBox).Text;

            // Фильтрация данных по введенному тексту
            var filteredEvents = _context.Event
                .Where(x => x.name.ToLower().Contains(filterText.ToLower()))
                .ToList();

            if (filterText == string.Empty) filteredEvents = _context.Event.ToList();

            // Обновление источника данных `DataGrid` отфильтрованными данными
            dg_events.ItemsSource = filteredEvents;
        }

        private void RegionFilterTextBoxSelectionChanged(object sender, TextChangedEventArgs e)
        {
            // Получение введенного текста фильтра
            var filterText = (sender as TextBox).Text;

            // Фильтрация данных по введенному тексту
            var filteredRegion = _context.Region
                .Where(x => x.name.Contains(filterText))
                .ToList();

            if (filterText == string.Empty) filteredRegion = _context.Region.ToList();

            // Обновление источника данных `DataGrid` отфильтрованными данными
            dg_region.ItemsSource = filteredRegion;
        }

        private void NormaTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Получение введенного текста фильтра
            var filterText = (sender as TextBox).Text;

            // Фильтрация данных по введенному тексту
            var filteredNorma = _context.Norm
                .Where(x => x.name.Contains(filterText)|| x.Type.name.Contains(filterText) || x.norma.Contains(filterText))
                .ToList();

            if (filterText == string.Empty) filteredNorma = _context.Norm.ToList();

            // Обновление источника данных `DataGrid` отфильтрованными данными
            dg_Norma.ItemsSource = filteredNorma;
        }
        private void FilterEcologicalRecommendationsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Получение введенного текста фильтра
            var filterText = (sender as TextBox).Text;

            // Фильтрация данных по введенному тексту
            var filteredEcologicalRecommendations = _context.EcologicalRecommendations
                .Where(x => x.heading.Contains(filterText)|| x.Users.full_name.Contains(filterText) || x.text.Contains(filterText))
                .ToList();

            if (filterText == string.Empty) filteredEcologicalRecommendations = _context.EcologicalRecommendations.ToList();

            // Обновление источника данных `DataGrid` отфильтрованными данными
            dg_region.ItemsSource = filteredEcologicalRecommendations;
        }

        // Обработчик события изменения фильтра для графика результатов изучения проб
        private void StudyFilterComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DrawStudy();
        }

        private void StudyFilterTextBoxSelectionChanged(object sender, TextChangedEventArgs e)
        {
            // Получение введенного текста фильтра
            var filterText = (sender as TextBox).Text;

            // Фильтрация данных по введенному тексту
            var filteredStudy = _context.Contemplation
                .Where(x => x.Users.full_name.ToLower().Contains(filterText.ToLower()))
                .ToList();

            if (filterText == string.Empty) filteredStudy = _context.Contemplation.ToList();

            // Обновление источника данных `DataGrid` отфильтрованными данными
            dg_employees.ItemsSource = filteredStudy;
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
                studyData = studyData.OrderBy(x => x.type).ToList();
            }

            DrawStudy();
        }

        private void StudyFilterRegionComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получение выбранного поля сортировки
            var selectedField = (sender as ComboBox).SelectedValue.ToString();

            // Сортировка данных по выбранному полю
            var sortedStudy = _context.Contemplation
                .Where(x => x.Sample.Region.name.Contains(selectedField))
                .ToList();

            // Обновление источника данных `DataGrid` отсортированными данными
            dg_sample.ItemsSource = sortedStudy;
        }

        private void StudyFilterTypeComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получение выбранного поля сортировки
            var selectedField = (sender as ComboBox).SelectedValue.ToString();

            // Сортировка данных по выбранному полю
            var sortedStudy = _context.Contemplation
                .Where(x => x.Sample.Type.name.Contains(selectedField))
                .ToList();

            // Обновление источника данных `DataGrid` отсортированными данными
            dg_sample.ItemsSource = sortedStudy;
        }

        private void NormaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получение выбранного поля сортировки
            var selectedField = (sender as ComboBox).SelectedValue.ToString();

            // Сортировка данных по выбранному полю
            var sortedNorma = _context.Norm
                .Where(x => x.Type.name.Contains(selectedField))
                .ToList();

            // Обновление источника данных `DataGrid` отсортированными данными
            dg_sample.ItemsSource = sortedNorma;
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

        private void BackClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void btn_createSamle_Click(object sender, RoutedEventArgs e)
        {
            AddDataWindow addDataWindow = new AddDataWindow();
            addDataWindow.table = "Образец";
            addDataWindow.Show();
            this.Close();
        }

        private void btn_createContemplation_Click(object sender, RoutedEventArgs e)
        {
            AddDataWindow addDataWindow = new AddDataWindow();
            addDataWindow.table = "Изучение пробы";
            addDataWindow.Show();
            this.Close();
        }

        private void btn_createNews_Click(object sender, RoutedEventArgs e)
        {
            AddDataWindow addDataWindow = new AddDataWindow();
            addDataWindow.table = "Новость";
            addDataWindow.Show();
            this.Close();
        }

        private void btn_createEvents_Click(object sender, RoutedEventArgs e)
        {
            AddDataWindow addDataWindow = new AddDataWindow();
            addDataWindow.table = "Мероприятие";
            addDataWindow.Show();
            this.Close();
        }

        private void btn_createRegion_Click(object sender, RoutedEventArgs e)
        {
            AddRegionDialog addRegionDialog = new AddRegionDialog();
            addRegionDialog.ShowDialog();
        }

        private void btn_createEmployees_Click(object sender, RoutedEventArgs e)
        {
            AddDataWindow addDataWindow = new AddDataWindow();
            addDataWindow.table = "Пользователи";
            addDataWindow.Show();
            this.Close();
        }

        private void btn_Norma_Click(object sender, RoutedEventArgs e)
        {
            AddNormDialog addNormDialog = new AddNormDialog();
            addNormDialog.ShowDialog();
        }

        private void btn_createEcologicalRecommendations_Click(object sender, RoutedEventArgs e)
        {
            AddDataWindow addDataWindow = new AddDataWindow();
            addDataWindow.table = "Рекомендация";
            addDataWindow.Show();
            this.Close();
        }
    }
}