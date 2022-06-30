using Microsoft.Win32;
using PQM.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using LiveCharts.Configurations;
using CheckBox = System.Windows.Controls.CheckBox;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PQM
{
    public partial class MainWindow : INotifyPropertyChanged
    {
        private Graph graph { get; set; }
        private Structure selectedStructure { get; set; }

        private double xmin = 0;
        private double xmax = 100;

        public static Grid grid { get; set; }

        public Boolean[] curvesDisplayed { get; set; }
        
        public Func<double, string> Formatter { get; set; }
        public string[] Labels { get; set; }

        public CanvasGraph canvasGraph;
        public MainWindow()
        {
            InitializeComponent();

            setVisibiilityStructMetrics(false);

            DataContext = this;
            grid = mainGrid;

            canvasGraph = new CanvasGraph();

            Loaded += delegate
            {
                canvasGraph.onLoad();
            };

        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void selectFilesBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv";
            openFileDialog.Title = "Select Files";
            openFileDialog.Multiselect = true;
            if(openFileDialog.ShowDialog() == true)
            {
                List<Structure> myStructures = new List<Structure>();
                string metric = null;
                int i = 0;
                foreach(string fileName in openFileDialog.FileNames)
                {
                    if(File.Exists(fileName))
                    {
                        Structure newStructure = new Structure(fileName);
                        newStructure.id = i;
                        myStructures.Add(newStructure);
                        string structureMetric = newStructure.metric;

                        // Check to make sure all metrics are the same
                        if (metric == null) metric = structureMetric;
                        else if(metric.ToLower() != structureMetric.ToLower())
                        {
                            string msg = "ERROR: Metrics to not Match. File " + fileName + " has metric: " + structureMetric + " not equal to " + metric;
                            System.Windows.MessageBox.Show(msg);
                            return;
                        }
                    }
                    i++;
                }
                graph = new Graph(metric, myStructures);
                plotGraph();
            }
        }

        private void selectFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog openFolderDialog = new FolderBrowserDialog();
            DialogResult dialogResult = openFolderDialog.ShowDialog();
            if(dialogResult.ToString() != string.Empty)
            {
                string path = openFolderDialog.SelectedPath.ToString();
                if(path != String.Empty)
                {
                    List<Structure> myStructures = new List<Structure>();
                    string metric = null;
                    int i = 0;
                    foreach(string fileName in Directory.GetFiles(path))
                    {
                        if(File.Exists(fileName))
                        {
                            Structure newStructure = new Structure(fileName);
                            newStructure.id = i;
                            myStructures.Add(newStructure);
                            string structureMetric = newStructure.metric;

                            // Check to make sure all metrics are same
                            if (metric == null) metric = structureMetric;
                            else if(metric.ToLower() != structureMetric.ToLower())
                            {
                                string msg = "ERROR: Metrics to not Match. File " + newStructure.name + " has metric: " + structureMetric + " not equal to " + metric;
                                System.Windows.MessageBox.Show(msg);
                                return;
                            }
                        }
                        i++;
                    }
                    graph = new Graph(metric, myStructures);
                    plotGraph();
                }
            }
        }

        private void plotGraph()
        {
            canvasGraph.plotGraph(graph);
            addLegends();
            initCurvesDisplayed();
        }

        private void initCurvesDisplayed()
        {
            curvesDisplayed = new Boolean[graph.structures.Count];
            for(int i = 0; i < graph.structures.Count; i++)
            {
                curvesDisplayed[i] = true;
            }

        }

        private void applyXrangeBtn_Click(object sender, RoutedEventArgs e)
        {
            double minX;
            double maxX;

            if(double.TryParse(setXMaxTxt.Text, out maxX) && double.TryParse(setXMinTxt.Text, out minX))
            {
                xmin = minX;
                xmax = maxX;

                double range = xmax - xmin;
                double freeSpace = graph.maxX - range;

                if(freeSpace == 0)
                {
                    xPosSlider.Value = 0;
                }
                else
                {
                    xPosSlider.Value = (minX / freeSpace) * 100;
                }

                canvasGraph.setDomain(xmin, xmax);

            }
            else
            {
                System.Windows.MessageBox.Show("An Error Occured: Make sure values are numbers");
            }
        }
        
        private void showRaw_Checked(object sender, RoutedEventArgs e)
        {

        }
        private void showRaw_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void addLegends()
        {

            int i = 0;
            foreach(Structure structure in graph.structures)
            {
                if (structure.numSplines == 0) continue;

                System.Windows.Controls.Label newLabel = new System.Windows.Controls.Label();

                newLabel.Content = structure.name;
                newLabel.Height = 40;
                newLabel.Width = 200;

                CheckBox newCheckBox = new System.Windows.Controls.CheckBox();
                newCheckBox.Name = String.Concat("_", i.ToString());
                newCheckBox.IsChecked = true;
                newCheckBox.AddHandler(CheckBox.ClickEvent, new RoutedEventHandler(structureCheckedChanged));


                Ellipse newRect = new Ellipse();
                newRect.Height = 20;
                newRect.Width = 20;
                newRect.Margin = new Thickness(5, 0, 0, 10);
                newRect.Fill = structure.color;

                StackPanel newStackPanel = new StackPanel();
                newStackPanel.Orientation = System.Windows.Controls.Orientation.Horizontal;
                newStackPanel.Children.Add(newCheckBox);
                newStackPanel.Children.Add(newRect);
                newStackPanel.Children.Add(newLabel);
                newStackPanel.Name = String.Concat("_", i.ToString());

                newStackPanel.AddHandler(StackPanel.MouseEnterEvent, new RoutedEventHandler(legendStackPanelEnter));
                newStackPanel.AddHandler(StackPanel.MouseLeaveEvent, new RoutedEventHandler(legendStackPanelLeave));
                newStackPanel.AddHandler(StackPanel.MouseLeftButtonDownEvent, new RoutedEventHandler(legendSPClick));

                structuresSP.Children.Add(newStackPanel);
                i++;
            }
        }
        private void structureCheckedChanged(object sender, RoutedEventArgs e)
        {
            DependencyObject dpobj = sender as DependencyObject;
            string name = dpobj.GetValue(FrameworkElement.NameProperty) as string;
            string idString = name.Substring(1, name.Length - 1);
            int ind;

            if (!Int32.TryParse(idString, out ind)) throw new Exception("IDK");

            Boolean? ischecked = dpobj.GetValue(CheckBox.IsCheckedProperty) as Boolean?;

            if (ischecked == null) throw new Exception("It's null");

            if((bool)ischecked)
            {
                canvasGraph.addCurve(ind);
                curvesDisplayed[ind] = true;
            }
            else
            {
                canvasGraph.removeCurve(ind);
                curvesDisplayed[ind] = false;
            }

        }

        private (int, string) getStructureInd(object sender)
        {
            DependencyObject dpobj = sender as DependencyObject;
            string name = dpobj.GetValue(FrameworkElement.NameProperty) as string;
            string idString = name.Substring(1, name.Length - 1);
            int ind;

            if (!Int32.TryParse(idString, out ind)) throw new Exception("IDK");

            return (ind, name);
        }


        private System.Windows.Controls.Label lastSelectedLbl;

        private void legendSPClick(object sender, RoutedEventArgs e)
        {
            (int ind, string name) = getStructureInd(sender);

            selectedStructure = graph.structures[ind];

            if (lastSelectedLbl != null)
            {
                lastSelectedLbl.FontWeight = FontWeights.Normal;
                lastSelectedLbl = null;
                setVisibiilityStructMetrics(false);
            }

            StackPanel sp = sender as StackPanel;
            foreach(object child in sp.Children)
            {
                if(child.GetType() == typeof(System.Windows.Controls.Label))
                {
                    System.Windows.Controls.Label lbl = child as System.Windows.Controls.Label;
                    if (lbl == lastSelectedLbl) break;

                    lbl.FontWeight = FontWeights.Bold;
                    lastSelectedLbl = lbl;
                    setVisibiilityStructMetrics(true);
                    selectedStructureLabel.Content = selectedStructure.name;
                    break;
                }
            }

            updateInterpX();
            updateInterpY();

        }

        private void setVisibiilityStructMetrics(Boolean makeVisibile)
        {
            Visibility vis = makeVisibile ? Visibility.Visible : Visibility.Hidden;

            colorPickerSP.Visibility = vis;
            setColor_btn.Visibility = vis;
            xinterpSP.Visibility = vis;
            xinterpOutputSP.Visibility = vis;
            interpolateSP.Visibility = vis;
            aucInputSP.Visibility = vis;
            interpolateYSP.Visibility = vis;
            yinterpOutputSP.Visibility = vis;

        }


        private SolidColorBrush highlight = new SolidColorBrush(Color.FromArgb(255, 227, 227, 227));
        private void legendStackPanelEnter(object sender, RoutedEventArgs e)
        {
            StackPanel sp = sender as StackPanel;
            sp.Background = highlight; 
        }
        private void legendStackPanelLeave(object sender, RoutedEventArgs e)
        {
            StackPanel sp = sender as StackPanel;
            sp.Background = Brushes.White;
        }

        private void interpXtxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(selectedStructure == null)
            {
                System.Windows.MessageBox.Show("Please Select a Structure");
            }
            else
            {
                updateInterpX();
            }
        }

        private void interpYtxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(selectedStructure == null)
            {
                System.Windows.MessageBox.Show("Please Select a Structure");
            }
            else
            {
                updateInterpY();
            }
        }

        private void auctxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(selectedStructure == null)
            {
                System.Windows.MessageBox.Show("Please Select a Structure");
            }
            else
            {
                updateAUC();
            }
        }

        private void updateInterpX()
        {
            double x;
            if(double.TryParse(interpXtxt.Text, out x))
            {
                double y = selectedStructure.interpolate(x);
                double dy = selectedStructure.interpolateDerivative(x);

                y = Math.Round(y, 6);
                dy = Math.Round(dy, 6);

                interpXoutputY.Text = y.ToString();
                interpXoutputdY.Text = dy.ToString();

            }
            else
            {
                interpXoutputY.Text = String.Empty;
                interpXoutputdY.Text = String.Empty;
            }

        }

        private void updateAUC()
        {
            double lower, upper;

            if(!double.TryParse(lowerBoundtxt.Text, out lower))
            {
                lowerBoundtxt.Text = String.Empty;
                interpXoutputAUC.Text = String.Empty;
                return;
            }
            if(!double.TryParse(upperBoundtxt.Text, out upper))
            {
                upperBoundtxt.Text = String.Empty;
                interpXoutputAUC.Text = String.Empty;
                return;
            }

            double AUC = selectedStructure.findArea(lower, upper);
            interpXoutputAUC.Text = AUC.ToString();

        }

        private void updateInterpY()
        {
            double y;
            if(double.TryParse(interpYtxt.Text, out y))
            {
                if(y < 0 || y > 100)
                {
                    System.Windows.MessageBox.Show("Invalid Y value");
                    interpYoutput.Text = String.Empty;
                    return;
                }
                double x = selectedStructure.invInterpolate(y);
                x = Math.Round(x, 3);
                interpYoutput.Text = x.ToString();
            }
            else
            {
                interpYoutput.Text = String.Empty;
            }
        }

        private void selectbtn_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < curvesDisplayed.Length; i++)
            {
                if(!curvesDisplayed[i])
                {
                    canvasGraph.addCurve(i);
                    curvesDisplayed[i] = true;
                }
            }

            foreach(StackPanel sp in structuresSP.Children)
            {
                CheckBox chk = sp.Children[0] as CheckBox;
                chk.IsChecked = true;
            }

        }
        private void deselectbtn_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < curvesDisplayed.Length; i++)
            {
                if(curvesDisplayed[i])
                {
                    canvasGraph.removeCurve(i);
                    curvesDisplayed[i] = false;
                }
            }


            foreach(StackPanel sp in structuresSP.Children)
            {
                CheckBox chk = sp.Children[0] as CheckBox;
                chk.IsChecked = false;
            }
        }

        private void exportBtn_Click(object sender, RoutedEventArgs e)
        {
            Canvas canvas = canvasGraph.generateExport();

            int width = (int)canvas.Width;
            int height = (int)canvas.Height;

            double dpi = 100;

            RenderTargetBitmap bmp = new RenderTargetBitmap(width, height, dpi, dpi, PixelFormats.Default);
            bmp.Render(canvas);

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            System.Windows.Forms.SaveFileDialog fdlg = new System.Windows.Forms.SaveFileDialog();
            fdlg.Filter = "png Files (*.png)|*.png";
            fdlg.DefaultExt = "png";
            fdlg.ShowDialog();


            if(fdlg.FileName != "")
            {
                using (var stream = System.IO.File.Create(fdlg.FileName))
                {
                    encoder.Save(stream);
                }
            }
        }

        private void setColor_btn_Click(object sender, RoutedEventArgs e)
        {
            Color newColor = (Color) colorPicker.SelectedColor;
            SolidColorBrush newColorBrush = new SolidColorBrush(newColor);


            selectedStructure.color = newColorBrush;

            int id = selectedStructure.id;

            canvasGraph.changeCurveColor(id, newColorBrush);

            // Update Color on legend
            StackPanel innerSP = structuresSP.Children[id] as StackPanel;
            Ellipse colorRect = innerSP.Children[1] as Ellipse;
            colorRect.Fill = newColorBrush;

        }

        private void xPosSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double range = xmax - xmin;
            double freeSpace = graph.maxX - range;
            double val = xPosSlider.Value;
        }
    }
}
