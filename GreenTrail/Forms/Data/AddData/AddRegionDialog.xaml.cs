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
using System.Windows.Shapes;

namespace GreenTrail.Forms.Data.AddData
{
    /// <summary>
    /// Логика взаимодействия для AddRegionDialog.xaml
    /// </summary>
    public partial class AddRegionDialog : Window
    {
        public string RegionName { get; set; }
        public string RegionGeographicalCoordinates { get; set; }
        public bool DialogResult { get; set; }

        public AddRegionDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Получение введенных данных
            RegionName = NameTextBox.Text;
            RegionGeographicalCoordinates = GeographicalCoordinatesTextBox.Text;

            // Установка результата диалогового окна на "OK"
            DialogResult = true;

            // Закрытие диалогового окна
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Установка результата диалогового окна на "Отмена"
            DialogResult = false;

            // Закрытие диалогового окна
            this.Close();
        }
    }
}
