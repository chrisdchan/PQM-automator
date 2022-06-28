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
        private TextBlock[] axesValues;
        private (TextBlock title, TextBlock x, TextBlock y) graphLabels;

        private SolidColorBrush axesColor = new SolidColorBrush(Colors.Black);
        private ScaleTransform scaleTransform = new ScaleTransform(1, -1);

        private double LEFT_X = 50;
        private double BOTTOM_Y = 50;
        private double RIGHT_X;
        private double TOP_Y;

        private int NUM_XAXES_TICKS = 5;
        private int NUM_YAXES_TICKS = 5;

        private int XAXES_MARGIN = 10;
        private int YAXES_MARGIN = 25;

        private double AXES_TICK_SIZE = 5;

        private double viewMin;
        private double viewMax;

        private int NUM_POINTS = 100;

        private Func<double, double> mapXToGraph;
        private Func<double, double> mapYToGraph;

        public CanvasGraph()
        {
            canvas = new Canvas();
            axesValues = new TextBlock[NUM_XAXES_TICKS + NUM_YAXES_TICKS];
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
            canvas.LayoutTransform = scaleTransform;
        }

        public void onLoad()
        {
            setDimensions();
            initAxesValues();
            initGraphLabels();
        }

        private void setGraph()
        {
            setAxesLines();
            setAxesTicks();
            setGraphLabels();
            setAxesValues();
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

            plotStructures();
            setGraph();
        }
        private void plotStructures()
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
            plotStructures();
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
            double dx = (double) (RIGHT_X - LEFT_X) / (NUM_XAXES_TICKS - 1);
            double x = LEFT_X + dx;
            for(int i = 0; i < NUM_XAXES_TICKS; i++)
            {
                Line line = createLine(x, BOTTOM_Y, x, BOTTOM_Y - AXES_TICK_SIZE, axesColor);
                canvas.Children.Add(line);
                x += dx;
            }

            double dy = (double)(TOP_Y - BOTTOM_Y) / (NUM_YAXES_TICKS - 1);
            double y = BOTTOM_Y + dy;
            for(int i = 0; i < NUM_YAXES_TICKS; i++)
            {
                Line line = createLine(LEFT_X, y, LEFT_X - AXES_TICK_SIZE, y, axesColor);
                canvas.Children.Add(line);
                y += dy;
            }
        }

        private void initGraphLabels()
        {
            graphLabels = (new TextBlock(), new TextBlock(), new TextBlock());

            SolidColorBrush testColor = new SolidColorBrush(Colors.LightPink);

            graphLabels.x.FontSize = 12;
            graphLabels.x.RenderTransform = scaleTransform;
            graphLabels.x.Background = testColor;
            graphLabels.x.SizeChanged += graphLabelsSizeChanged;

            graphLabels.y.FontSize = 12;
            graphLabels.y.RenderTransform = scaleTransform;
            graphLabels.y.LayoutTransform = new RotateTransform(270);
            graphLabels.y.Background = testColor;
            graphLabels.y.SizeChanged += graphLabelsSizeChanged;

            graphLabels.title.FontSize = 18;
            graphLabels.title.RenderTransform = scaleTransform;
            graphLabels.title.Background = testColor;
            graphLabels.title.SizeChanged += graphLabelsSizeChanged;

            canvas.Children.Add(graphLabels.x);
            canvas.Children.Add(graphLabels.y);
            canvas.Children.Add(graphLabels.title);
        }

        private void setGraphLabels()
        {
            (String title, String xtitle, String ytitle) = getGraphLabels();

            graphLabels.x.Text = xtitle;
            graphLabels.y.Text = ytitle;
            graphLabels.title.Text = title;
        }

        private void graphLabelsSizeChanged(object sender, RoutedEventArgs e)
        {
            double AXES_MARGIN_X = 30;
            double AXES_MARGIN_Y = 45;
            double TITLE_OFFSET = 10;

            double xAxesSetLeft = (RIGHT_X + LEFT_X) / 2.0 - graphLabels.x.ActualWidth / 2.0;
            double xAxesSetTop = BOTTOM_Y - AXES_MARGIN_X;

            double yAxesSetTop = (TOP_Y + BOTTOM_Y) / 2.0 + graphLabels.y.ActualHeight / 2.0;
            double yAxesSetLeft = LEFT_X - AXES_MARGIN_Y;

            double titleSetLeft = (RIGHT_X + LEFT_X) / 2.0 - graphLabels.title.ActualWidth / 2.0;
            double titleSetTop = TOP_Y + TITLE_OFFSET + graphLabels.title.ActualHeight; 

            Canvas.SetLeft(graphLabels.x, xAxesSetLeft);
            Canvas.SetTop(graphLabels.x, xAxesSetTop);

            Canvas.SetLeft(graphLabels.y, yAxesSetLeft);
            Canvas.SetTop(graphLabels.y, yAxesSetTop);

            Canvas.SetLeft(graphLabels.title, titleSetLeft);
            Canvas.SetTop(graphLabels.title, titleSetTop);

        }

        private (String title, String xAxes, String yAxes) getGraphLabels()
        {
            String xtitle = "";
            String graphTitle = "";
            String ytitle = "Percent Normalized Volume (%)";

            if(graph.metric == "CD")
            {
                xtitle = "Current Density (A/m^2)";
                graphTitle = "Current Denisty";

            }
            else if(graph.metric == "SAR")
            {
                xtitle = "SAR (W/kg)";
                graphTitle = "Specific Absorbance Rate";
            }
            else if(graph.metric == "E-field")
            {
                xtitle = "Electric Field Density (V/m)";
                graphTitle = "Electic Field Density";
            }

            return (graphTitle, xtitle, ytitle);
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

        private void initAxesValues()
        {
            double x_canvas = LEFT_X;
            double dx_canvas = (RIGHT_X - LEFT_X) / (NUM_XAXES_TICKS - 1);

            axesValues = new TextBlock[NUM_XAXES_TICKS + NUM_YAXES_TICKS];

            for(int i = 0; i < NUM_XAXES_TICKS; i++)
            {
                TextBlock textblock = createTextBlock();

                canvas.Children.Add(textblock);
                Canvas.SetLeft(textblock, x_canvas);
                Canvas.SetTop(textblock, BOTTOM_Y - XAXES_MARGIN);

                axesValues[i] = textblock;

                x_canvas += dx_canvas;
            }

            double y_canvas = BOTTOM_Y;
            double dy_canvas = (TOP_Y - BOTTOM_Y) / (NUM_YAXES_TICKS - 1);

            for(int i = 0; i < NUM_YAXES_TICKS; i++)
            {
                TextBlock textblock = createTextBlock();

                canvas.Children.Add(textblock);
                Canvas.SetLeft(textblock, LEFT_X - YAXES_MARGIN);
                Canvas.SetTop(textblock, y_canvas);

                axesValues[NUM_XAXES_TICKS + i] = textblock;
                y_canvas += dy_canvas;
            }
        }

        private TextBlock createTextBlock()
        {
            TextBlock textblock = new TextBlock();
            textblock.Foreground = axesColor;
            textblock.FontSize = 10;
            textblock.RenderTransform = scaleTransform;
            return textblock;
        }

        private void setAxesValues()
        {
            double x_graph = viewMin;
            double dx_graph = (viewMax - viewMin) / (NUM_XAXES_TICKS - 1);

            for(int i = 0; i < NUM_XAXES_TICKS; i++)
            {
                axesValues[i].Text = x_graph.ToString();
                x_graph += dx_graph;
            }

            double y_graph = 0;
            double dy_graph = 100 / (NUM_YAXES_TICKS - 1);

            for(int i = 0; i < NUM_YAXES_TICKS; i++)
            {
                if (y_graph != 0)
                {
                    axesValues[i + NUM_XAXES_TICKS].Text = y_graph.ToString();
                }
                y_graph += dy_graph;
            }
        }

        public Canvas generateExport()
        {
            Canvas exportCanvas = WPFCanvasCopier.Clone(canvas);

            int NUM_STRUCTURES_PER_COL = 50;

            double BLOCK_HEIGHT = 30;
            double RECT_SIDE = 25;

            int cols = (int)Math.Ceiling((double)graph.structures.Count / NUM_STRUCTURES_PER_COL);
            exportCanvas.Width = canvas.ActualWidth + cols * 100;
            exportCanvas.Height = canvas.ActualHeight;

            for(int col = 0; col < cols; col++)
            {
                for(int i = 0; i < NUM_STRUCTURES_PER_COL; i++)
                {
                    int structInd = (col + 1) * i;
                    if (structInd > graph.structures.Count - 1) break;

                    TextBlock textblock = new TextBlock();
                    Structure structure = graph.structures[structInd];

                    textblock.Text = structure.name;

                    Rectangle rect = new Rectangle();
                    rect.Fill = structure.color;
                    rect.Height = RECT_SIDE;
                    rect.Width = RECT_SIDE;

                    exportCanvas.Children.Add(textblock);

                    Canvas.SetLeft(textblock, RIGHT_X + col * 100 + RECT_SIDE);
                    Canvas.SetTop(textblock, TOP_Y - i * BLOCK_HEIGHT);


                    exportCanvas.Children.Add(rect);

                    Canvas.SetLeft(rect, RIGHT_X + col * 100);
                    Canvas.SetTop(rect, TOP_Y - i * BLOCK_HEIGHT);
                }
            }

            return exportCanvas;
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
