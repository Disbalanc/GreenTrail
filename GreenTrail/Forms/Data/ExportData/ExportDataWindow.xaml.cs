using GreenTrail.Forms.Settings;
using GreenTrail.Source.Funs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing.Printing;
using System.IO;
using Newtonsoft.Json;
using iTextSharp.text;
using iTextSharp.text.pdf;
using DocumentFormat.OpenXml.Spreadsheet;
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
using System.Xml.Linq;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using static System.Net.WebRequestMethods;
using System.Drawing;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using OfficeOpenXml;

namespace GreenTrail.Forms.Data.ExportData
{
    /// <summary>
    /// Логика взаимодействия для ExportDataWindow.xaml
    /// </summary>
    public partial class ExportDataWindow : Window
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

        public ExportDataWindow()
        {
            InitializeComponent();

            DataContext = new SaveFileDialogViewModel();

            cb_selectTable.ItemsSource = Tables;
        }

        private void OnBrowse(object sender, RoutedEventArgs e)
        {
            SaveFileDialogViewModel saveFileDialogViewModel = new SaveFileDialogViewModel();
            saveFileDialogViewModel.OnBrowse(dg_table);
        }
        private List<string> Tables = new List<string> { "Пробы", "Пользователи", "Обследования проб", "Мероприятия", "Загрязнения", "Экологические рекомендации" };


        private void cb_selectTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            // Получить выбранную таблицу
            var selectedTable = (string)e.AddedItems[0];

            // Запустить задачу для загрузки данных в отдельном потоке
            Task.Run(() =>
            {
                using (var context = new GreanTrailEntities())
                {
                    object data = context.Sample.ToList();

                    switch (selectedTable)
                    {
                        case "Пробы":
                            data = context.Sample
                                .Select(s => new { s.articul, typeName = s.Type.name, regionName = s.Region.name, full_name = s.Users.full_name })
                                .ToList();
                            break;
                        case "Пользователи":
                            data = context.Users
                                .Select(u => new { u.full_name, u.Roles.name, u.dateOfBirth, u.address, u.email })
                                .ToList();
                            break;
                        case "Обследования проб":
                            data = context.Contemplation
                                .Select(ss => new { ss.Sample.articul, ss.Users.full_name, ss.type_contemplation, ss.Norm.name, ss.Norm.norma, ss.result })
                                .ToList();
                            break;
                        case "Мероприятия":
                            data = context.Event
                                .Select(ep => new { ep.name, ep.data_time })
                                .ToList();
                            break;
                        case "Загрязнения":
                            data = context.Region_Pollution
                                .Select(p => new { p.Pollution.id_pollution, p.Pollution.levels, p.Pollution.source, p.Region.name, p.Region.geographical_coordinates })
                                .ToList();
                            break;
                        case "Экологические рекомендации":
                            data = context.EcologicalRecommendations
                                .Select(er => new { er.heading, er.text, er.Users.full_name })
                                .ToList();
                            break;
                    }

                    // Обновить пользовательский интерфейс с загруженными данными
                    Dispatcher.Invoke(() =>
                    {
                        dg_table.ItemsSource = (System.Collections.IEnumerable)data;
                    });
                }
            });
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (tb_path.Text == string.Empty)
            {
                MessageBox.Show("Не заполнен путь!", "Erorr", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }


        }

        public class SaveFileDialogViewModel : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            private string _path;
            public string Path
            {
                get => _path;
                set
                {
                    _path = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Path)));
                }
            }

            private string _selectedFormat;
            public string SelectedFormat
            {
                get => _selectedFormat;
                set
                {
                    _selectedFormat = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedFormat)));
                }
            }

            public List<string> Formats { get; set; }

            public SaveFileDialogViewModel()
            {
                Formats = new List<string> { "PDF", "JSON", "TXT", "DOC", "DOCX", "XLS", "XLSX", "CSV" };
                SelectedFormat = Formats[0];
            }

            public void OnBrowse(DataGrid dataGrid)
            {
                // Создать диалоговое окно сохранения файла
                var dialog = new SaveFileDialog();
                dialog.Filter = "Файлы (*.pdf, *.json, *.txt, *.docx, *.xlsx)|*.pdf;*.json;*.txt;*.docx;*.xlsx";

                // Показать диалоговое окно и получить выбранный путь к файлу
                if (dialog.ShowDialog() == true)
                {

                    // Сохранить данные DataGrid в выбранном формате
                    switch (SelectedFormat)
                    {
                        case "pdf":
                            SaveToPdf(dialog.FileName, dataGrid);
                            break;
                        case "json":
                            SaveToJson(dialog.FileName, dataGrid);
                            break;
                        case "txt":
                            SaveToTxt(dialog.FileName, dataGrid);
                            break;
                        case "docx":
                            SaveToDocx(dialog.FileName, dataGrid);
                            break;
                        case "xlsx":
                            SaveToXlsx(dialog.FileName, dataGrid);
                            break;
                        case "csv":
                            SaveToXlsx(dialog.FileName, dataGrid);
                            break;
                    }
                }
            }
            private void SaveToCsv(string filePath, DataGrid dataGrid)
            {
                var data = dataGrid.ItemsSource.Cast<object>().ToList();

                // Создать объект StringBuilder для построения содержимого CSV
                var csv = new StringBuilder();

                // Добавить заголовки столбцов
                foreach (var column in dataGrid.Columns)
                {
                    csv.Append(column.Header + ",");
                }
                csv.AppendLine();

                // Добавить данные строк
                foreach (var item in data)
                {
                    var properties = item.GetType().GetProperties();

                    foreach (var property in properties)
                    {
                        csv.Append(property.GetValue(item) + ",");
                    }
                    csv.AppendLine();
                }

                // Сохранить содержимое CSV в файл
                System.IO.File.WriteAllText(filePath, csv.ToString());
            }

            private void SaveToPdf(string filePath, DataGrid dataGrid)
            {
                var data = dataGrid.ItemsSource.Cast<object>().ToList();

                // Создать объект Document для генерации PDF
                var document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 25, 25, 25, 25);
                var writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                document.Open();

                // Добавить заголовки столбцов
                var table = new PdfPTable(dataGrid.Columns.Count);
                foreach (var column in dataGrid.Columns)
                {
                    table.AddCell(new PdfPCell(new Phrase(column.Header.ToString())));
                }

                // Добавить данные строк
                foreach (var item in data)
                {
                    var properties = item.GetType().GetProperties();

                    foreach (var property in properties)
                    {
                        table.AddCell(new PdfPCell(new Phrase(property.GetValue(item).ToString())));
                    }
                }

                // Добавить таблицу в документ
                document.Add(table);

                // Закрыть документ
                document.Close();
            }

            private void SaveToTxt(string filePath, DataGrid dataGrid)
            {
                // Получить данные DataGrid
                var data = dataGrid.ItemsSource.Cast<object>().ToList();

                // Создать объект StringBuilder для построения содержимого TXT
                var txt = new StringBuilder();

                // Добавить заголовки столбцов
                foreach (var column in dataGrid.Columns)
                {
                    txt.Append(column.Header + "\t");
                }
                txt.AppendLine();

                // Добавить данные строк
                foreach (var item in data)
                {
                    var properties = item.GetType().GetProperties();

                    foreach (var property in properties)
                    {
                        txt.Append(property.GetValue(item) + "\t");
                    }
                    txt.AppendLine();
                }

                // Сохранить содержимое TXT в файл
                System.IO.File.WriteAllText(filePath, txt.ToString());
            }

            private void SaveToJson(string filePath, DataGrid dataGrid)
            {
                // Получить данные DataGrid
                var data = dataGrid.ItemsSource.Cast<object>().ToList();

                // Преобразовать данные в JSON
                var json = JsonConvert.SerializeObject(data);

                // Сохранить JSON в файл
                System.IO.File.WriteAllText(filePath, json);
            }

            private void SaveToDocx(string filePath, DataGrid dataGrid)
            {
                // Создать новый документ
                var document = new Spire.Doc.Document();

                // Добавить заголовки столбцов
                var table = document.Sections[0].AddTable(true);
                table.PreferredWidth = new Unit(100, Spire.Doc.UnitType.Percentage);
                var headerRow = table.Rows[0];
                foreach (var column in dataGrid.Columns)
                {
                    headerRow.Cells[headerRow.Cells.Count].AddParagraph().AppendText(column.Header.ToString());
                }

                // Добавить данные строк
                foreach (var item in dataGrid.Items)
                {
                    var properties = item.GetType().GetProperties();
                    var dataRow = table.AddRow();

                    foreach (var property in properties)
                    {
                        dataRow.Cells[dataRow.Cells.Count].AddParagraph().AppendText(property.GetValue(item).ToString());
                    }
                }

                // Сохранить файл
                document.SaveToFile(filePath, Spire.Doc.FileFormat.Docx);
            }

            private void SaveToXlsx(string filePath, DataGrid dataGrid)
            {
                using (var package = new ExcelPackage())
                {
                    // Добавить новый лист
                    var worksheet = package.Workbook.Worksheets.Add("Лист1");

                    // Добавить заголовки столбцов
                    for (int i = 0; i < dataGrid.Columns.Count; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = dataGrid.Columns[i].Header;
                    }

                    // Добавить данные строк
                    for (int i = 0; i < dataGrid.Items.Count; i++)
                    {
                        var item = dataGrid.Items[i];
                        var properties = item.GetType().GetProperties();

                        for (int j = 0; j < properties.Length; j++)
                        {
                            worksheet.Cells[i + 2, j + 1].Value = properties[j].GetValue(item);
                        }
                    }

                    // Сохранить файл
                    package.SaveAs(new FileInfo(filePath));
                }
            }
        }
    }
}