using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Jamesnet.Wpf.Controls;

namespace GreenTrail.Source.Style
{
    public class ThemeSwitch : JamesToggleSwitch
    {
        static ThemeSwitch()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ThemeSwitch), new FrameworkPropertyMetadata(typeof(ThemeSwitch)));
        }
    }
}