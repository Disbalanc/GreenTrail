using Microsoft.SqlServer.Server;
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
    /// Логика взаимодействия для AddNormDialog.xaml
    /// </summary>
    public partial class AddNormDialog : Window
    {

        private GreanTrailEntities _context = new GreanTrailEntities();

        public AddNormDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {

            // Создание новой нормы
            Norm norm = new Norm
            {
                name = NameTextBox.Text,
                norma = NormaTextBox.Text,
                id_norm = (int)TypeNormComboBox.SelectedIndex
            };

            // Добавление региона в БД
            _context.Norm.Add(norm);
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
