using API_testing;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPF1
{
    internal class DataPlot
    {
        public DataPlot(MainWindow Parent, Plotter Plotter, Grid masterGrid,
            int x, int y, int rowspan, int colspan) {
            if (rowspan < 2 || colspan < 3) { MessageBox.Show("Not enough space to be placed"); return; }

            Grid subGrid = new Grid();
            for (int i = 0; i < rowspan; i++) { subGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }); }
            for (int i = 0; i < colspan; i++) { subGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); }

            GridPlace(subGrid, masterGrid, x, y, colspan, rowspan);
            
            subGrid.Background = new SolidColorBrush(Colors.DarkSlateGray); 

            Border b = new Border();
            b.BorderThickness = new Thickness(3);
            b.BorderBrush = new SolidColorBrush(Colors.DarkRed);

            GridPlace(b, subGrid, x, y, colspan, rowspan);
            
            
            UIDisplay(Parent, subGrid, Plotter, rowspan, colspan);
        }

        private async Task UIDisplay(MainWindow Parent, Grid subGrid, Plotter Plotter, int imgX, int imgY)
        {
            var JSONData = await Parent.APICall("drivers?session_key=latest");
            List<Driver> Drivers = JsonConvert.DeserializeObject<List<Driver>>(JSONData);
            ListBox DriverLB = new ListBox()
            {
                SelectionMode = SelectionMode.Multiple,
                ItemsSource = Drivers,
                DisplayMemberPath = "full_name",
            };

            GridPlace(DriverLB, subGrid);

            List<string> stats = new List<string>() { "Brake", "Gears", "RPM", "Speed", "Throttle (%)" };
            ListBox StatLB = new()
            {
                SelectionMode = SelectionMode.Single,
                ItemsSource = stats,
            };

            GridPlace(StatLB, subGrid, 1);


            Button b = new Button()
            {
                Content = "Plot",
            };
            GridPlace(b, subGrid, 2);

            b.Click += async (s, e) => {
                if (DriverLB.SelectedIndex == -1 || StatLB.SelectedIndex == -1) { MessageBox.Show("One or more items unselected"); return; }
                int ind = StatLB.SelectedIndex;
                List<DateTime> xs = new List<DateTime>();
                List<double[]> ys = new();

                
                foreach (Driver D in DriverLB.SelectedItems)
                {
                    var JSONData = await Parent.APICall($"car_data?driver_number={D.driver_number}&session_key=latest");
                    List<CarStats> stats = JsonConvert.DeserializeObject<List<CarStats>>(JSONData);
                    List<double> tmp = new List<double>();
                    foreach (CarStats cs in stats)
                    {
                        tmp.Add(((double)cs.getAtts()[ind]));
                        if (xs.Count < tmp.Count) { xs.Add(cs.date); }
                    }
                    ys.Add(tmp.ToArray());
                }
                //System.Windows.Controls.Image img = Plotter.Plot(null, xs.ToArray(), sizeX: (int)subGrid.RenderSize.Width, sizeY: (int)subGrid.RenderSize.Height, mys: ys);
                MessageBox.Show("Plotter Initialized");
                ScottPlot.WPF.WpfPlot p = Plotter.WpfPlot(null, xs.ToArray(), ys);
                GridPlace(p, subGrid, 0, 0, imgY, imgX);
            };
        }

        private void GridPlace(UIElement e, Grid g, int c = 0, int r = 0,  int cs = 1, int rs = 1)
        {
            Grid.SetRow(e, r); Grid.SetColumn(e, c);
            Grid.SetRowSpan(e, rs); Grid.SetColumnSpan(e, cs);
            g.Children.Add(e);
        }
    }
}
