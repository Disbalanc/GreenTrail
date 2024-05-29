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
        private GreanTrailEntities _context = new GreanTrailEntities();

        public AddRegionDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {

            // Создание нового региона
            Region region = new Region
            {
                name = NameTextBox.Text,
                geographical_coordinates = GeographicalCoordinatesTextBox.Text
            };

            // Добавление региона в БД
            _context.Region.Add(region);
            _context.SaveChanges();

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
