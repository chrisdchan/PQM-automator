using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PQM.Models
{
    public class CanvasGraph
    {
        public Canvas canvas { get; set; }

        private double graphHeight { get; set; }
        private double graphWidth { get; set; }

        public CanvasGraph()
        {
            canvas = new Canvas();
            add();
            initCanvas();
        }

        public void add()
        {
            MainWindow.grid.Children.Add(canvas);
        }

        private void initCanvas()
        {
            Grid.SetRow(canvas, 2);
            Grid.SetColumn(canvas, 1);
            Grid.SetRowSpan(canvas, 2);
            Grid.SetColumnSpan(canvas, 2);

            canvas.Background = new SolidColorBrush(Colors.White);
        }
        public void addTest()
        {
            Line line = new Line();
            line.X1 = 0;
            line.X2 = canvas.ActualWidth;
            line.Y1 = 0;
            line.Y2 = canvas.ActualHeight;
            line.Stroke = new SolidColorBrush(Colors.Red);
            line.StrokeThickness = 1;
            canvas.Children.Add(line);

            Rectangle rect = new Rectangle();
            rect.Fill = new SolidColorBrush(Colors.Black);
            rect.Width = 20;
            rect.Height = 10;
            canvas.Children.Add(rect);
            Canvas.SetLeft(rect, 20);
            Canvas.SetRight(rect, 25);

        }

        
    }
}
