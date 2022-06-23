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

        private Graph graph;
        private Line[][] curves;

        private SolidColorBrush axesColor = new SolidColorBrush(Colors.Black);

        private double LEFT_X = 50;
        private double BOTTOM_Y = 50;
        private double RIGHT_X;
        private double TOP_Y;

        private double NUM_XAXES_TICKS = 10;
        private double NUM_YAXES_TICKS = 10;

        private double AXES_TICK_SIZE = 5;

        private double viewMin;
        private double viewMax;

        private double graphMin;
        private double graphMax;

        private int NUM_POINTS = 100;

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
            setAxesLabels();
        }

        private void clearCurves()
        {
            if (curves == null) return;

            for(int i = 0; i < curves.Length; i++)
            {
                for(int j = 0; j < curves[i].Length; j++)
                {
                    canvas.Children.Remove(curves[i][j]);
                }
            }
        }

        public void plotGraph(Graph graph)
        {
            this.graph = graph;
            viewMin = 0;
            viewMax = graph.maxX;
            graphMin = 0;
            graphMax = graph.maxX;
            plotGraph();
        }
        private void plotGraph()
        {
            clearCurves();
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
            mapXToGraph = (x) => (x * (RIGHT_X - LEFT_X)) / (viewMax - viewMin) + LEFT_X;
            mapYToGraph = (y) => (y * (TOP_Y - BOTTOM_Y)) / 100.0 + BOTTOM_Y;
        }

        public void setDomain(double xmin, double xmax)
        {
            viewMin = xmin;
            viewMax = xmax;
            plotGraph();
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
            double dx = (double) (RIGHT_X - LEFT_X) / NUM_XAXES_TICKS;
            double x = LEFT_X + dx;
            for(int i = 0; i < NUM_XAXES_TICKS; i++)
            {
                Line line = createLine(x, BOTTOM_Y, x, BOTTOM_Y - AXES_TICK_SIZE, axesColor);
                canvas.Children.Add(line);
                x += dx;
            }

            double dy = (double)(TOP_Y - BOTTOM_Y) / NUM_YAXES_TICKS;
            double y = BOTTOM_Y + dy;
            for(int i = 0; i < NUM_YAXES_TICKS; i++)
            {
                Line line = createLine(LEFT_X, y, LEFT_X - AXES_TICK_SIZE, y, axesColor);
                canvas.Children.Add(line);
                y += dy;
            }
        }


        private void plotStructure(Structure structure)
        {
            double x1 = viewMin;
            double dx = (viewMax - viewMin) / NUM_POINTS;
            double x2 = x1 + dx;

            double y1, y2;

            curves[structure.id] = new Line[NUM_POINTS - 1];

            for(int i = 0; i < NUM_POINTS - 1; i++)
            {
                y1 = structure.interpolate(x1);
                y2 = structure.interpolate(x2);

                Line line = new Line();
                line.X1 = mapXToGraph(x1);
                line.Y1 = mapYToGraph(y1);
                line.X2 = mapXToGraph(x2);
                line.Y2 = mapYToGraph(y2);
                line.Stroke = structure.color;
                line.StrokeThickness = 1.5;
                curves[structure.id][i] = line;
                canvas.Children.Add(line);

                x1 = x2;
                x2 = x2 + dx;

            }
        }

        private void setAxesLabels()
        {
            double x = viewMin;
            double dx = (viewMax - viewMin) / NUM_XAXES_TICKS;
            
            for(int i = 0; i < NUM_XAXES_TICKS; i++)
            {
                TextBlock textblock = new TextBlock();
                textblock.Text = x.ToString();
                textblock.Foreground = axesColor;
                textblock.FontSize = 10;

                canvas.Children.Add(textblock);
                Canvas.SetLeft(textblock, mapXToGraph(x));
                Canvas.SetTop(textblock, BOTTOM_Y);

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
