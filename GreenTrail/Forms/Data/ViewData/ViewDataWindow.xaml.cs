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

namespace GreenTrail.Forms.Data.ViewData
{
    /// <summary>
    /// Логика взаимодействия для ViewDataWindow.xaml
    /// </summary>
    public partial class ViewDataWindow : Window
    {
        public ViewDataWindow()
        {
            InitializeComponent();

            dg_sample.ItemsSource = GreanTrailEntities.GetContext().Sample.ToList();
            


        }

        private void createPollution()
        {
            cartesianChart.Series = new SeriesCollection
                {
                    new LineSeries
                    {
                        Title = "Результат",
                        Values = query.Select(q => q.Result)
                    }
                };

            // Создание осей
            cartesianChart.AxisX = new Axis
            {
                Title = "Пробы",
                Labels = query.Select(q => q.FullName)
            };

            cartesianChart.AxisY = new Axis
            {
                Title = "Норма",
                LabelFormatter = value => value.ToString("N2")
            };
        }
    }
}
