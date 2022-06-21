using LiveCharts;
using LiveCharts.Defaults;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.RootFinding;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PQM.Models
{
    public class Structure
    {
        // Initialized on 
        public string name { get; set; }

        public int id { get; set; }
        public string metric { get; set; }
        public int numSplines { get; }
        private int[] splineTypes { get; }
        private (double, double)[] derivatives { get; }
        private double[] coefficients { get; }
        public Point[] rawPoints { get; }

        public double[] splineArea {get;}
            
        public double maxX { get; set; }

        public List<Point> curve { get; }

        public List<double> xRaw { get; set; } // All data points
        public List<double> yRaw { get; set; } // All data points

        public double[] graphXRaw { get; set; }
        public double[] graphYRaw { get; set; }

        public SolidColorBrush color { get; set; }


        public Structure(string filePath)
        {
            (List<double> X, List<double> Y) =  parseCSV(filePath);
            
            if(X == null && Y == null)
            {
                // default case of nothing happens
                numSplines = 0;
                rawPoints = new Point[0];
                curve = new List<Point>();
                coefficients = new double[0];
                splineTypes = new int[0];


            }
            else
            {
                Y = normalize(Y);
                rawPoints = initRaw(X, Y);

                numSplines = X.Count;


                coefficients = setCoefficients(X, Y);
                derivatives = getDerivatives(X, Y); 
                splineTypes = getSplineTypes(X);


                curve = initCurve();
            }

            for(int i = curve.Count - 1; i >= 0; i--)
            {
                if(curve[i].Y > 0.05)
                {
                    maxX = curve[i].X;
                    break;
                }
            }

        }

        private (List<double>, List<double>) parseCSV(string filePath)
        {

            List<double> newX = new List<double>(); 
            List<double> newY = new List<double>();

            //TODO: parse file here
            using(StreamReader sr = new StreamReader(filePath))
            {
                string line;
                Regex rxIdX = new Regex("ec.normJ|ec.normE|SAR/"); // Identify X value row
                Regex rxExtractX = new Regex("[<>=]([0-9.]*)"); // Extract X  values
                Regex rxExtractY = new Regex("[0-9.E-]*"); // Extract Y values

                // Search for data line
                while((line = sr.ReadLine()) != null)
                {
                    if (rxIdX.IsMatch(line)) break;
                }

                MatchCollection matches = rxExtractX.Matches(line);
                foreach(Match match in matches)
                {
                    string value = match.Value.ToString();
                    value = value.Substring(1);
                    if(float.TryParse(value, out float newVal))
                    {
                        newX.Add(newVal);
                    }
                }

                line = sr.ReadLine();

                matches = rxExtractY.Matches(line);
                
                foreach(Match match in matches)
                {
                    string value = match.Value.ToString();
                    Trace.WriteLine(value);

                    if(double.TryParse(value, out double newVal))
                    {
                        if(newVal < 0) newVal = 0;
                        newY.Add(newVal);
                        if(newVal < 0)
                        {
                            throw new Exception("Negative Value");
                        }
                    }
                }

            }

            // Get Metric 
            string fileName = Path.GetFileName(filePath);
            string[] fileNameParts = fileName.Split(' ');
            metric = fileNameParts[0];

            string last = fileNameParts.Last();
            if(last == "Raw.csv")
            {
                string myName = "";
                for(int i = 1; i < fileNameParts.Length - 1; i++)
                {
                    myName += fileNameParts[i] + " ";
                }
                name = myName;
            }
            else
            {
                name = last;
            }

            if(newX.Count == 0 || newY.Count == 0)
            {
                MessageBox.Show("Can not parse graph " + fileName);
                return (null, null);
            }
            else
            {
                newY.RemoveAt(0);
            }

            return (newX, newY);

        }

        private static List<double> normalize(List<double> Y)
        {
            double max = Y[0];
            List<double> normalized = new List<double>();

            foreach(double y in Y)
            {
                normalized.Add(y * 100 / max);
            }

            return normalized;
        }

        private static double[] setCoefficients(List<double> X, List<double> Y)
        {
            int n = X.Count;
            int u = 4 * n - 4;

            double[,] A = new double[u, u];
            double[] b = new double[u];

            int row = 0;

            for(int i = 0; i < u; i++)
            {
                for(int j = 0; j < u; j++)
                {
                    A[i, j] = 0;
                }
                b[i] = 0;
            }

            for(int i = 0; i < n - 1; i++)
            {
                double x1 = X[i];
                double x2 = X[i + 1];

                // spline touches left point
                A[row, i * 4] = Math.Pow(x1, 3);
                A[row, i * 4 + 1] = Math.Pow(x1, 2);
                A[row, i * 4 + 2] = x1;
                A[row, i * 4 + 3] = 1;
                b[row] = Y[i];

                // spline touches right point
                A[row + 1, i * 4] = Math.Pow(x2, 3);
                A[row + 1, i * 4 + 1] = Math.Pow(x2, 2);
                A[row + 1, i * 4 + 2] = x2;
                A[row + 1, i * 4 + 3] = 1;
                b[row + 1] = Y[i + 1];

                row += 2;
            }

            // f'(xi) = f'(xi+1) and f''(xi) = f''(xi+1)
            for(int i = 1; i < n - 1; i++)
            {
                double x1 = X[i];
                A[row, i * 4 - 4] = 3 * Math.Pow(x1, 2);
                A[row, i * 4 - 3] = 2 * x1;
                A[row, i * 4 - 2] = 1;
                A[row, i * 4] = -3 * Math.Pow(x1, 2);
                A[row, i * 4 + 1] = -2 * x1;
                A[row, i * 4 + 2] = -1;
                b[row] = 0;

                A[row + 1, i * 4 - 4] = 6 * x1;
                A[row + 1, i * 4 - 3] = 2;
                A[row + 1, i * 4] = -6 * x1;
                A[row + 1, i * 4 + 1] = -2;
                b[row + 1] = 0;

                row += 2;
            }


            // f''(x0) = f''(xn - 1) = 0
            A[row, 0] = 6 * X[0];
            A[row, 1] = 2;
            b[row] = 0;

            A[row + 1, u - 4] = 6 * X[n - 1];
            A[row + 1, u - 3] = 2;
            b[row + 1] = 0;
            
            Matrix<double> matA = Matrix<double>.Build.DenseOfArray(A);
            Vector<double> vecB = Vector<double>.Build.DenseOfArray(b);

            Vector<double> coefficients = matA.Solve(vecB);
            return coefficients.ToArray();

        }


        private (double, double)[] getDerivatives(List<double> X, List<double> Y)
        {
            // start with an initial alpha and beta
            // then you figure out alpha and beta based on the last value
            // All consecutive splines have the same derivative

            double h = 0.1;
            double f0, f1, f2;
            (double, double)[] D = new (double, double)[X.Count];

            f0 = X[0];
            f1 = interpolate(h, 0, 0);
            f2 = interpolate(2 * h, 0, 0);

            double fd1 = (1 / (2 * h)) * (-3 * f0 + 4 * f1 - f2);
            if (fd1 > 0) fd1 = 0;

            f0 = interpolate(X[1] - h, 0, 0);
            f2 = interpolate(Y[1] + h, 1, 0);

            double fd2 = (1 / (2 * h)) * (-f0 + f2);
            if (fd2 >  0) fd2 = 0;

            double delta = getDelta(0, X, Y);

            double alpha, beta, d1, d2;

            if(delta == 0)
            {
                D[0] = (0, 0);
                d2 = 0;
            }
            else
            {
                alpha = fd1 / delta;
                beta = fd2 / delta;

                (alpha, beta) = f2Remap(alpha, beta);

                d1 = alpha * delta;
                d2 = beta * delta;

                D[0] = (d1, d2);
            }

            for(int i = 1; i <= numSplines - 2; i++)
            {
                delta = getDelta(i, X, Y);
                if(delta == 0)
                {
                    D[i] = (0, 0);
                    d2 = 0;
                    continue;
                }
                d1 = d2;    // derivative from consecutive splines must be the same
                alpha = d1 / delta;

                if(alpha > 3)
                {
                    d1 = 3 * delta; // reset d1 with alpha = 3
                    beta = 0;
                }
                else
                {
                    beta = Math.Sqrt(9 - alpha * alpha);
                }

                
                d2 = beta * delta;
                D[i] = (d1, d2);
            }

            return D;
        }

        private (double, double) f2Remap(double a, double b)
        {
            if(a == 0 && b == 0)
            {
                return (0, 0);
            }
            else
            {
                double tau = 3 / Math.Sqrt(a * a + b * b);
                return (a * tau, b * tau);
            }
        }

        private double getDelta(int spline, List<double> X, List<double> Y)
        {
            if (spline > numSplines - 1) throw new Exception("Invalid Spline");
            return (Y[spline + 1] - Y[spline]) / (X[spline + 1] - X[spline]);
        }

        private double getDelta(int spline)
        {
            return (rawPoints[spline + 1].Y - rawPoints[spline].Y) / (rawPoints[spline + 1].X - rawPoints[spline].X);
        }

        private double getH(int spline)
        {
            return rawPoints[spline + 1].X - rawPoints[spline].X;
        }

        private int[] getSplineTypes(List<double> X)
        {
            int n = X.Count;
            int[] types = new int[n];
            for (int i = 0; i < n; i++) types[i] = 0;
            for(int i = 0; i < n - 1; i++)
            {
                double dx = (X[i + 1] - X[i]) / 20;
                double last = double.PositiveInfinity;
                double y;
                for(double x = X[i]; x < X[i + 1]; x += dx)
                {
                    y = interpolate(x, i, 0);
                    if(y > last || y < 0)
                    {
                        types[i] = 1;
                        break;
                    }
                    last = y;
                }
            }
            return types;
        }
        public double interpolate(double x, int spline, int method)
        {
            //method = 1 f-2
            //method = 0 cubic
            if (method != 0 && method != 1) throw new Exception("Invalid Method Param");

            double y;
            if(method == 1) // f-2 method
            {
                double h = rawPoints[spline + 1].X - rawPoints[spline].X;
                double delt = (rawPoints[spline + 1].Y - rawPoints[spline].Y) / h;
                double di1 = derivatives[spline].Item1;
                double di2 = derivatives[spline].Item2;

                double a = (di1 + di2 - 2 * delt) / Math.Pow(h, 2);
                double b = (-2 * di1 - di2 + 3 * delt) / h;
                double c = di1;
                double d = rawPoints[spline].Y;
                double newX = x - rawPoints[spline].X;

                y = a * Math.Pow(newX, 3) + b * Math.Pow(newX, 2) + c * newX + d;
            }
            else // cubic method
            {
                if (coefficients == null) throw new Exception("Coefficients not defined");
                double a = coefficients[spline * 4];
                double b = coefficients[spline * 4 + 1];
                double c = coefficients[spline * 4 + 2];
                double d = coefficients[spline * 4 + 3];

                y = a * Math.Pow(x, 3) + b * Math.Pow(x, 2) + c * x + d;
            }

            return y;
        }

        public double interpolateDerivative(double x, int spline, int method)
        {
            double dydx;
            if(method == 0) // Cubic
            {
                double a = coefficients[spline * 4];
                double b = coefficients[spline * 4 + 1];
                double c = coefficients[spline * 4 + 2];

                dydx = 3 * a * Math.Pow(x, 2) + 2 * b * x + c;
            }
            else // F-C
            {
                double delta = getDelta(spline);
                double h = getH(spline);

                double a = 3 * (derivatives[spline].Item1 - 2 * delta) / Math.Pow(h, 2);
                double b = 2 * (-2 * derivatives[spline].Item1 - derivatives[spline].Item2 + 3 * delta) / h;
                double c = derivatives[spline].Item1;

                double xadj = x - rawPoints[spline].X;

                dydx = a * Math.Pow(xadj, 2) + b * xadj + c;
            }
            
            return dydx;
        }

        public double interpolate(double x)
        {
            //Assume that splines are assigned 
            if (splineTypes == null) throw new Exception("did not define splineTypes yet");

            int xptr = getSpline(x);
            return interpolate(x, xptr, splineTypes[xptr]);
        }

        public double getJerk(int spline)
        {
            double jerk;

            if(splineTypes[spline] == 0)
            {
                jerk = coefficients[spline * 4];
            }
            else
            {
                double delta = getDelta(spline);
                double h = getH(spline);
                jerk = (6 * (derivatives[spline].Item1 + derivatives[spline].Item2 - 2 * delta)) / (h * h);
            }
            return jerk;
            

        }

        public double interpolateDerivative(double x)
        {
            int xptr = getSpline(x);
            return interpolateDerivative(x, xptr, splineTypes[xptr]);
        }

        private int getSpline(double x)
        {
            // returns the spline of the associated x value

            int xptr = 0;
            while (x > rawPoints[xptr + 1].X) xptr++;
            return xptr;
        }

        private int getInvSpline(double y)
        {
            // returns the spline of the associated y values
            if (y < 0 || y > 100) throw new Exception("Invalid y value"); 

            int yptr = 0;
            while (y < rawPoints[yptr + 1].Y) yptr++;
            return yptr;

        }

        public double invInterpolate(double y)
        {
            int yptr = getInvSpline(y);
            double s = rawPoints[yptr].X;
            double e = rawPoints[yptr + 1].X;

            double x = (s + e) / 2;
            double guess = Math.Round(interpolate(x), 4);
            y = Math.Round(y, 4);

            while(guess != y)
            {
                if(guess > y)
                {
                    s = x;
                }
                else
                {
                    e = x;
                }
                x = (s + e) / 2;
                guess = Math.Round(interpolate(x), 4);
            }

            return x;

        }        

        public double findArea(double lower, double upper)
        {
            int xptr = getSpline(upper);

            double area = 0;
            double a, b, c, d;
            double Fxi, Fxf, xi, xf, splineArea;

            if(upper > rawPoints[rawPoints.Length - 1].X)
            {
                upper = rawPoints[rawPoints.Length - 1].X;
            }

            for(int i = 0; i <= xptr; i++)
            {

                if (lower >= rawPoints[i + 1].X) continue;

                xi = rawPoints[i].X;
                xf = rawPoints[i + 1].X;

                if (upper < rawPoints[i + 1].X) xf = upper;
                if (lower > rawPoints[i].X) xi = lower;
                
                if(splineTypes[i] == 0)
                {

                    a = coefficients[i * 4];
                    b = coefficients[i * 4 + 1];
                    c = coefficients[i * 4 + 2];
                    d = coefficients[i * 4 + 3];


                    Fxf = (a / 4) * Math.Pow(xf, 4) + (b / 3) * Math.Pow(xf, 3) + (c / 2) * Math.Pow(xf, 2) + d * xf;
                    Fxi = (a / 4) * Math.Pow(xi, 4) + (b / 3) * Math.Pow(xi, 3) + (c / 2) * Math.Pow(xi, 2) + d * xi;


                }
                else
                {
                    double delta = getDelta(i);
                    double h = getH(i);

                    a = (derivatives[i].Item1 + derivatives[i].Item2 - 2 * delta) / Math.Pow(h, 2);
                    b = (-2 * derivatives[i].Item1 - derivatives[i].Item2 + 3 * delta) / h;
                    c = derivatives[i].Item1;
                    d = rawPoints[i].Y;

                    Fxf = (a / 4) * Math.Pow(xf - rawPoints[i].X, 4) + (b / 3) * Math.Pow(xf - rawPoints[i].X, 3) + (c / 2) * Math.Pow(xf - rawPoints[i].X , 2) + d * (xf - rawPoints[i].X);
                    Fxi = (a / 4) * Math.Pow(xi - rawPoints[i].X, 4) + (b / 3) * Math.Pow(xi - rawPoints[i].X, 3) + (c / 2) * Math.Pow(xi - rawPoints[i].X, 2) + d * (xi - rawPoints[i].X);

                }

                splineArea = Fxf - Fxi;
                area += splineArea;

            }
            return area;
        }

        private ChartValues<ObservablePoint> initCurve1()
        {
            ChartValues<ObservablePoint> points = new ChartValues<ObservablePoint>();

            double type = 2;

            // 1 = evenly spaced

            int totalPoints = 0;

            for(int i = 0; i < numSplines - 1; i++)
            {
                double height = rawPoints[i].Y - rawPoints[i + 1].Y;
                double width = rawPoints[i + 1].X - rawPoints[i].X;

                double jerk = getJerk(i);
                jerk = Math.Abs(jerk);

                double graphN = Math.Ceiling(jerk);

                if(width / graphN < 1)
                {
                    graphN = width;
                }

                double dx = (rawPoints[i + 1].X - rawPoints[i].X) / graphN;
                double x = rawPoints[i].X;
                double y;

                for(int j = 0; j < graphN; j++)
                {
                    y = interpolate(x);
                    points.Add(new ObservablePoint(x, y));
                    totalPoints++;
                    x += dx;
                }
            }

            return points;
        }

        private List<Point> initCurve()
        {
            List<Point> points = new List<Point>();

            for(int i = 0; i < numSplines - 1; i++)
            {
                double delta = getDelta(i);
                double width = rawPoints[i + 1].X - rawPoints[i].X;

                int graphN;

                if(delta > -0.1)
                {
                    graphN = 3;
                }
                if(delta > -0.5)
                {
                    graphN = 5;
                }
                else if (delta > -1)
                {
                    graphN = 6;
                }
                else
                {
                    graphN = 8;
                }


                double dx = (rawPoints[i + 1].X - rawPoints[i].X) / graphN;
                double x = rawPoints[i].X;
                double y;
                
                for(int j = 0; j < graphN; j++)
                {
                    y = interpolate(x);
                    
                    points.Add(new Point(x, y));

                    x += dx;
                }

            }

            return points;

        }


        

        private Point[] initRaw(List<double> X, List<double> Y)
        {
            Point[] R = new Point[X.Count];

            for(int i = 0; i < X.Count; i++)
            {
                R[i] = new Point(X[i], Y[i]);
            }

            return R;
        }

        public List<Point> getCurve(double min, double max)
        {
            if (numSplines == 0) return new List<Point>();

            if(min < rawPoints[0].X || max > rawPoints[numSplines - 1].X || max < min)
            {
                throw new Exception("Error: min or max X value out of range");
            }

            List<Point> chart = new List<Point>();

            foreach(Point p in curve)
            {
                if (p.X < min) continue;
                if (p.X > max) break;
                
                chart.Add(p);
            }
            return chart;
        }

        public ChartValues<Point> getRaw(double min, double max)
        {
            if(min < rawPoints[0].X || max > rawPoints[numSplines - 1].X || max < min)
            {
                throw new Exception("Error: min or max X value out of range");
            }

            ChartValues<Point> chart = new ChartValues<Point>();

            foreach(Point p in rawPoints)
            {
                if (p.X < min) continue;
                if (p.X > max) break;
                
                chart.Add(p);
            }

            return chart;

        }
        private void setRawGraph(double minX, double maxX)
        {
            int xptrMin = 0;
            int xptrMax = 0;

            while(minX < xRaw[xptrMin]) xptrMin++;
            while (maxX > xRaw[xptrMax + 1]) xptrMax++;

            int n = xptrMax - xptrMin;

            graphXRaw = new double[n];
            graphYRaw = new double[n];

            for(int i = 0; i < n; i++)
            {
                graphXRaw[i] = xRaw[i + xptrMin];
                graphYRaw[i] = yRaw[i + xptrMin];
            }
        }

    }
}
