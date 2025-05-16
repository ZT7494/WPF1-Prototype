using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPF1
{
    class Plotter
    {
        public System.Windows.Controls.Image Plot(double[] ys, DateTime[] xs, string path = "C:\\Users\\ZACKT\\Pictures\\demo.png", int sizeX = 500, int sizeY = 500, List<double[]>mys = null, bool time = true)
        {
            ScottPlot.Plot plot = new ScottPlot.Plot();

            if (mys != null) { MultiPlotter(plot, xs, mys); }
            else { plot.Add.Scatter(xs, ys); }

            if (time) { plot.Axes.DateTimeTicksBottom(); }

            plot.SavePng(path, sizeX, sizeY);

            BitmapImage p = new BitmapImage(new Uri(path));
            System.Windows.Controls.Image img = new()
            {
                Source = p,
                Stretch = Stretch.Fill,
            };
            return img;
        }

        private void MultiPlotter(Plot plot, DateTime[] xs, List<double[]>mys)
        {
            foreach (double[] ys in mys)
            {
                plot.Add.Scatter(xs, ys);
            }
        }
    }
}
