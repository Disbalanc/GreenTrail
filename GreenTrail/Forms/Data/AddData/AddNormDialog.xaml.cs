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
        public string NormName { get; set; }
        public string Norma { get; set; }
        public string TypeNorma { get; set; }
        public bool DialogResult { get; set; }

        public AddNormDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Получение введенных данных
            NormName = NameTextBox.Text;
            Norma = NormaTextBox.Text;
            TypeNorma = TypeNormComboBox.SelectedIndex.ToString()+1;

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
