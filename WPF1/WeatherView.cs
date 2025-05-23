using API_testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPF1
{
    internal class WeatherView
    {
        public WeatherView(MainWindow Parent, Grid g, int row, int col)
        {
            Grid WV = new Grid() { ShowGridLines = true };
            for (int i = 0; i < 7; i++) { WV.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }); }
            for (int i = 0; i < 9; i++) { WV.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); }

            GridPlace(WV, g, row, col);
            Border b = new Border();
            b.BorderThickness = new Thickness(3);
            b.BorderBrush = new SolidColorBrush(Colors.DarkRed);
            GridPlace(b, WV, 0, 0, 7, 9);

            Task t = Display(Parent, WV);

        }

        private async Task Display(MainWindow Parent, Grid WV)
        {
            var JSONData = await Parent.APICall("weather?session_key=latest");
            Weather W = JsonConvert.DeserializeObject<List<Weather>>(JSONData).FirstOrDefault();
            MessageBox.Show("placing");
            Label Title = new Label() { Content = "Weather"};
            GridPlace(Title, WV, 0, 0, 1, 9);

            Label Temps = new Label() { Content = $"Air Temp: {W.air_temperature}°\nTrack Temp: {W.track_temperature}°" };
            GridPlace(Temps, WV, 1, 0, 3, 3);

            Label Humidity = new Label() { Content = $"Humidity: {W.humidity}%" };
            GridPlace(Humidity, WV, 1, 3, 3, 3);

            Label WS = new Label() { Content = $"Wind Speed: {W.wind_speed}m/s" };
            GridPlace(WS, WV, 4, 0, 3, 3);

            string dir = "error";
            int deg = W.wind_direction;
            if (deg < 22.5) { dir = "N"; }
            else if (deg < 67.5) { dir = "NE"; }
            else if (deg < 112.5) { dir = "E"; }
            else if (deg < 157.5) { dir = "SE"; }
            else if (deg < 202.5) { dir = "S"; }
            else if (deg < 247.5) { dir = "SW"; }
            else if (deg < 292.5) { dir = "W"; }
            else if (deg < 337.5) { dir = "NW"; }
            else { dir = "N"; }

            Label Dir = new Label() { Content = dir };
            GridPlace(Dir, WV, 4, 3, 3, 3);
            
            

            

            
            
        }

        private void GridPlace(UIElement e, Grid g, int row = 0, int col = 0, int rowspan = 1, int colspan = 1)
        {
            Grid.SetRow(e, row); Grid.SetColumn(e, col);
            Grid.SetRowSpan(e, rowspan); Grid.SetColumnSpan(e, colspan);
            g.Children.Add(e);
        }
    }
}
