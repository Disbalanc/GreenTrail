using GreenTrail.Forms.Data.AddData;
using GreenTrail.Forms.Data.ExportData;
using GreenTrail.Forms.Data.ViewData;
using GreenTrail.Forms.Settings;
using GreenTrail.Source.Funs;
using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace GreenTrail.Forms
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double _verticalOffset;

        public MainWindow()
        {
            InitializeComponent();
            // Обработчик события прокрутки для ScrollBar
            scrollBar.Scroll += ScrollBar_Scroll;

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Вычисление высоты пустого пространства
            double emptySpaceHeight = scrollBar.ActualHeight - scrollBar.ViewportSize;

            // Установка максимального значения прокрутки
            scrollBar.Maximum = StackPanelMain.ActualHeight - scrollBar.ViewportSize - emptySpaceHeight;
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
            _verticalOffset -= e.Delta/5;

            // Ограничение вертикального смещения максимальным и минимальным значениями
            _verticalOffset = Math.Min(Math.Max(_verticalOffset, scrollBar.Minimum), scrollBar.Maximum);

            // Применение вертикального смещения к StackPanel
            StackPanelMain.Margin = new Thickness(0, -_verticalOffset, 0, 0);
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


        // Просмотр
        private void DataEcologicalClick(object sender, RoutedEventArgs e)
        {
            ViewDataWindow viewDataWindow = new ViewDataWindow();
            viewDataWindow.ShowDialog();
            this.Close();
        }


        // Карта
        private void MapClick(object sender, RoutedEventArgs e)
        {
            MapWindow mapWindow = new MapWindow();
            mapWindow.Show();
            this.Close();
        }


        // Добавление
        private void AddSample_Click(object sender, RoutedEventArgs e)
        {
            AddDataWindow addDataWindow = new AddDataWindow();
            addDataWindow.table = "Образец";
            addDataWindow.Show();
            this.Close();
        }

        private void AddRegion_Click(object sender, RoutedEventArgs e)
        {
            AddRegionDialog addRegionDialog = new AddRegionDialog();
            addRegionDialog.ShowDialog();
        }

        private void AddContemplation_Click(object sender, RoutedEventArgs e)
        {
            AddDataWindow addDataWindow = new AddDataWindow();
            addDataWindow.table = "Изучение пробы";
            addDataWindow.Show();
            this.Close();
        }

        private void AddNorm_Click(object sender, RoutedEventArgs e)
        {
            AddNormDialog addNormDialog = new AddNormDialog();
            addNormDialog.ShowDialog();
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            AddDataWindow addDataWindow = new AddDataWindow();
            addDataWindow.table = "Пользователи";
            addDataWindow.Show();
            this.Close();
        }


        //Экспорт
        private void ExportExel_Click(object sender, RoutedEventArgs e)
        {
            ExportDataWindow exportDataWindow = new ExportDataWindow();
            exportDataWindow.ExportType = "Exel";
            exportDataWindow.Show();
            this.Close();
        }

        private void ExportWord_Click(object sender, RoutedEventArgs e)
        {
            ExportDataWindow exportDataWindow = new ExportDataWindow();
            exportDataWindow.ExportType = "Word";
            exportDataWindow.Show();
            this.Close();
        }

        private void ExportPDF_Click(object sender, RoutedEventArgs e)
        {
            ExportDataWindow exportDataWindow = new ExportDataWindow();
            exportDataWindow.ExportType = "PDF";
            exportDataWindow.Show();
            this.Close();
        }
    }
}
