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
        private double canvasHeight { get; set; }
        private double canvasWidth { get; set; }


        private const double START_GRAPH_X = 20;
        private const double START_GRAPH_Y = 40;


        private List<List<Line>> curves { get; set; }

        public CanvasGraph()
        {
            canvas = new Canvas();
            curves = new List<List<Line>>();
            initCanvas();
        }
        private void initCanvas()
        {
            MainWindow.grid.Children.Add(canvas);

            Grid.SetRow(canvas, 2);
            Grid.SetColumn(canvas, 1);
            Grid.SetRowSpan(canvas, 2);
            Grid.SetColumnSpan(canvas, 2);

            canvas.Background = new SolidColorBrush(Colors.White);
        }

        public void setHeightandWidth()
        {
            canvasHeight = canvas.ActualHeight;
            canvasWidth = canvas.ActualWidth;
        }

        public void graph(Graph graph)
        {
        }

        private void initStructureCurve(Structure structure)
        {
            List<Point> curve = structure.curve;
            curves[structure.id] = new List<Line>();

            for(int i = 0; i < curve.Count - 1; i++)
            {
                Line line = new Line();
                line.X1 = curve[i].X;
                line.Y1 = curve[i].Y;
                line.X2 = curve[i].X;
                line.Y2 = curve[i].Y;
                line.Stroke = structure.color;
                line.StrokeThickness = 1;
                curves[structure.id].Add(line);
                canvas.Children.Add(line);
            }
        }
    }
}
