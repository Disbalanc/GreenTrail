using GreenTrail.Forms.Welcome.LoadingWindows;
using GreenTrail.Forms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GreenTrail.Forms.Welcome;
using System.Windows.Threading;
using System.Windows.Controls;

namespace GreenTrail
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static long CurrentUserId { get; set; }
    }
}
