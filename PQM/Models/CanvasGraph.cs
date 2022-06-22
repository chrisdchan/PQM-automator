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

        private Line[][] curves { get; set; }

        private SolidColorBrush axesColor = new SolidColorBrush(Colors.Black);

        private double LEFT_X = 50;
        private double BOTTOM_Y = 50;
        private double RIGHT_X;
        private double TOP_Y;

        private double NUM_XAXES_TICKS = 10;
        private double NUM_YAXES_TICKS = 10;

        private double AXES_TICK_SIZE = 5;
        private double NUM_XAXES_VALUES_SHOW = 4;
        private double NUM_YAXES_VALUES_SHOW = 4;

        private double xmin;
        private double xmax;


        private Func<double, double> mapXToGraph;
        private Func<double, double> mapYToGraph;

        public CanvasGraph()
        {
            canvas = new Canvas();
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
            canvas.LayoutTransform = new ScaleTransform(1, -1);
        }

        public void onLoad()
        {
            setDimensions();
            setGraph();
        }

        private void setGraph()
        {
            setAxesLines();
            setAxesTicks();
        }

        public void plotGraph(Graph graph)
        {
            xmin = 0;
            xmax = graph.maxX;

            curves = new Line[graph.structures.Count][];
            
            foreach(Structure structure in graph.structures)
            {
                plotStructure(structure);
            }
        }

        private void setDimensions()
        {
            RIGHT_X = canvas.ActualWidth - 50;
            TOP_Y = canvas.ActualHeight - 50;
            mapXToGraph = (x) => (x * (RIGHT_X - LEFT_X)) / (xmax - xmin) + LEFT_X;
            mapYToGraph = (y) => (y * (TOP_Y - BOTTOM_Y)) / 100.0 + BOTTOM_Y;
        }

        private void setAxesLines()
        {
            Line yaxes = createLine(LEFT_X, BOTTOM_Y, LEFT_X, TOP_Y, axesColor);
            Line xaxes = createLine(LEFT_X, BOTTOM_Y, RIGHT_X, BOTTOM_Y, axesColor);
            canvas.Children.Add(yaxes);
            canvas.Children.Add(xaxes);
        }

        private void setAxesTicks()
        {
            double x = LEFT_X;
            double dx = (double) (RIGHT_X - LEFT_X) / NUM_XAXES_TICKS;
            for(int i = 0; i < NUM_XAXES_TICKS; i++)
            {
                Line line = createLine(x, BOTTOM_Y, x, BOTTOM_Y - AXES_TICK_SIZE, axesColor);
                canvas.Children.Add(line);
                x += dx;
            }

            double y = BOTTOM_Y;
            double dy = (double)(TOP_Y - BOTTOM_Y) / NUM_YAXES_TICKS;
            for(int i = 0; i < NUM_YAXES_TICKS; i++)
            {
                Line line = createLine(LEFT_X, y, LEFT_X - AXES_TICK_SIZE, y, axesColor);
                canvas.Children.Add(line);
                y += dy;
            }
        }
        private void plotStructure(Structure structure)
        {
            List<Point> curve = structure.curve;
            curves[structure.id] = new Line[curve.Count];

            int minInd = indexOfXValue(xmin, curve);
            int maxInd = indexOfXValue(xmax, curve);

            for(int i = minInd; i < maxInd + 1; i++)
            {
                Line line = new Line();
                line.X1 = mapXToGraph(curve[i].X);
                line.Y1 = mapYToGraph(curve[i].Y);
                line.X2 = mapXToGraph(curve[i + 1].X);
                line.Y2 = mapYToGraph(curve[i + 1].Y);
                line.Stroke = structure.color;
                line.StrokeThickness = 1;
                curves[structure.id][i] = line;
                canvas.Children.Add(line);
            }
        }

        private int indexOfXValue(double x, List<Point> curve)
        {
            if (x == 0) return 0;
            if(x == curve[curve.Count - 1].X) return curve.Count - 1;

            int ind = (curve.Count / 2) - 1;

            int start = 0;
            int end = curve.Count - 2;

            double left = curve[ind].X;
            double right = curve[ind + 1].X;

            while(left - x > 0.01 || x - right > 0.01)
            {
                if(x < curve[ind].X)
                {
                    end = ind;
                }
                else
                {
                    start = ind + 1;
                }

                ind = (start + end) / 2 - 1;
                left = curve[ind].X;
                right = curve[ind + 1].X;
            }
            return ind;
        }
        private Line createLine(double x1, double y1, double x2, double y2, SolidColorBrush color)
        {
            Line line = new Line();
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;
            line.Stroke = color;
            line.StrokeThickness = 1;
            return line;
        }
    }
}
