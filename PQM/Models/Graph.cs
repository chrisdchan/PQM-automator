using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PQM.Models
{
    public class Graph
    {
        public string metric { get; set; }
        public List<Structure> structures { get; set; }

        public SeriesCollection allSeries { get; set; }

        public Dictionary<string, int> structureIDTable { get; }

        public double maxX { get; set; }

        public Graph(string metric, List<Structure> structures)
        {
            this.metric = metric;
            this.structures = structures;

            setColors();
            structureIDTable = setDict();
            setAllSeries();

            maxX = 0;
            foreach(Structure s in structures)
            {
                if(s.maxX > maxX) maxX = s.maxX;
            }
            
        }

        public Graph(string[] filePaths)
        {
            structures = new List<Structure>();

            maxX = 0;

            int i = 0;

            foreach(string filePath in filePaths)
            {
                Structure newStructure = new Structure(filePath);
                
                if (metric == null) metric = newStructure.metric;
                if (newStructure.metric != metric) throw new Exception("Metric is not the same");

                if (newStructure.maxX > maxX) maxX = newStructure.maxX;
                newStructure.id = i;
                structures.Add(newStructure);
                i++;

            }

            setColors();
            structureIDTable = setDict();
            setAllSeries();
        }

        private void setAllSeries()
        {
            allSeries = new SeriesCollection();
            foreach(Structure structure in structures)
            {
                allSeries.Add(new LineSeries
                {
                    Configuration = new CartesianMapper<Point>()
                        .X(point => point.X)
                        .Y(point => point.Y),
                    Title = structure.name,
                    Values = structure.getCurve(0, 100),
                    PointGeometrySize = 1,
                    LineSmoothness = 0,
                    Stroke = structure.color,
                    Fill = System.Windows.Media.Brushes.Transparent
                });
            }
        }


        private Dictionary<string, int> setDict()
        {
            Dictionary<string, int> M = new Dictionary<string, int>();

            for(int i = 0; i < structures.Count; i++)
            {
                M[structures[i].name] = i;
            }

            return M;
        }

        private void setColors()
        {
            int n = structures.Count;
            int dc = 1530 / n;
            int colorId = 0;

            for(int i = 0; i < n; i++)
            {
                structures[i].color = getColorfromId(colorId);
                colorId += dc;
            }

        }

        private static SolidColorBrush getColorfromId(int id)
        {
            int group = id / 255; // integer division

            int offset = id % 255;

            int R = 0, G = 0, B = 0;

            switch(group)
            {
                case 0:
                    R = 255;
                    G = offset;
                    break;
                case 1:
                    R = 255 - offset;
                    G = 255;
                    break;
                case 2:
                    G = 255;
                    B = offset;
                    break;
                case 3:
                    G = 255 - offset;
                    B = 255;
                    break;
                case 4:
                    R = offset;
                    B = 255;
                    break;
                case 5:
                    R = 255;
                    B = offset;
                    break;
                default:
                    throw new Exception("Invalid group");
            }

            return new SolidColorBrush(Color.FromArgb(255, (byte)R, (byte)G, (byte)B));

        }


    }
}
