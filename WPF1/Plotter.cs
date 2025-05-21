using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPF1
{
    class Plotter
    {
        //This is basically a nice wrapper for ScottPlot, allowing code in the main loop much more abstracted and easy to call repeatedly
        public System.Windows.Controls.Image Plot(double[] ys, DateTime[] xs, string path = "C:\\Users\\ZACKT\\Pictures\\demo.png", int sizeX = 500, int sizeY = 500, List<double[]>mys = null, bool time = true)
        {
            ScottPlot.Plot plot = new ScottPlot.Plot(); 

            
            
            if (mys != null) { MultiPlotter(plot, xs, mys); } //Multi-Plot (Have multiple lines on graph)
            else { plot.Add.Scatter(xs, ys); }

            if (time) { plot.Axes.DateTimeTicksBottom(); } //adds timing to bottom of graph

            plot.SavePng(path, sizeX, sizeY);

            BitmapImage p = new BitmapImage(new Uri(path));
            System.Windows.Controls.Image img = new()
            {
                Source = p,
                Stretch = Stretch.Fill,
            };
            return img;
        }

        public ScottPlot.WPF.WpfPlot WpfPlot(double[] ys, DateTime[] xs, List<double[]> mys = null, bool time = true)
        {
            ScottPlot.WPF.WpfPlot plot = new();
            plot.Plot.ScaleFactor = 0.75;
            plot.Plot.Axes.Hairline(true);
            
            if (mys != null) { MultiPlotter(plot.Plot, xs, mys); } //Multi-Plot (Have multiple lines on graph)
            else { var p = plot.Plot.Add.Scatter(xs, ys);}

            if (time) { plot.Plot.Axes.DateTimeTicksBottom(); } //adds timing to bottom of graph

            return plot;
        }

        private void MultiPlotter(Plot plot, DateTime[] xs, List<double[]>mys)
        {
            foreach (double[] ys in mys)
            {
                var tmp = plot.Add.Scatter(xs, ys);
                
            }
        }
    }
}
