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

namespace PQM
{
    /// <summary>
    /// Interaction logic for ChangeColor.xaml
    /// </summary>
    public partial class ChangeColor : Window
    {
        public ChangeColor()
        {
            InitializeComponent();
        }

        private void RGB_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            byte rVal, gVal, bVal;

            if (!byte.TryParse(rtxt.Text, out rVal)) return;
            if (!byte.TryParse(gtxt.Text, out gVal)) return;
            if(!byte.TryParse(btxt.Text, out bVal)) return;   


            if(!validColor(rVal))
            {
                MessageBox.Show("R value must be between 0 and 255");
                return;
            }

            if(!validColor(rVal))
            {
                MessageBox.Show("R value must be between 0 and 255");
                return;
            }

            if (!validColor(rVal))
            {
                MessageBox.Show("R value must be between 0 and 255");
                return;
            }

            SolidColorBrush newColor = new SolidColorBrush(Color.FromRgb(rVal, gVal, bVal));


            ColorRect.Fill = newColor;
        }

        private Boolean validColor(double num)
        {
            return num >= 0 && num <= 255;
        }
    }
}
