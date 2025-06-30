
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using API_testing;
using Newtonsoft.Json;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OpenTK.Graphics.ES20;


namespace WPF1
{

    internal class DriverView
    { 
        public DriverView(MainWindow Parent, Grid g, int row, int col)
        {
            //A sub-grid will control layout of smaller text and images in this widget
            Grid DV = new Grid()
            {
                //ShowGridLines = true,
                Background = new SolidColorBrush(Colors.Black)
            };
            for (int i = 0; i < 5; i++) { DV.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }); }
            for (int i = 0; i < 8; i++) { DV.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); }

            GridPlace(DV, g, row, col);

            Border b = new Border();
            b.BorderThickness = new Thickness(3);
            b.BorderBrush = new SolidColorBrush(Colors.DarkRed);

            GridPlace(b, DV, 0, 0, 5, 8);
            Task t = Display(Parent, DV);
        }

        private async Task Display(MainWindow Parent, Grid DV)
        {
            var JSONData = await Parent.APICall("drivers?session_key=latest");
            List<Driver> Drivers = JsonConvert.DeserializeObject<List<Driver>>(JSONData);
            ListBox DriverLB = new ListBox()
            {
                SelectionMode = SelectionMode.Single,
                ItemsSource = Drivers,
                DisplayMemberPath = "full_name",
            };
            GridPlace(DriverLB, DV, 0, 0, 5, 8);

            DriverLB.SelectionChanged += (sender, args) =>
            {
                ShowData(Parent, DV, DriverLB.SelectedItem as Driver);
                DV.Children.Remove(DriverLB);
            };

        }

        private void ShowData(MainWindow Parent, Grid DV, Driver D)
        {
            //Function to display data from the Driver class
            System.Windows.Media.Color col = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#" + D.team_colour);
            SolidColorBrush MainColor = new SolidColorBrush(col);
            Thickness Default = new Thickness(2);
            SolidColorBrush Border = new SolidColorBrush(Colors.Black);

            DV.Background = MainColor;

            Label Title = new() { Content = $"{D.first_name} {D.last_name} - {D.driver_number}", Background = MainColor, BorderBrush = Border, BorderThickness = Default };
            GridPlace(Title, DV, 0, 0, 1, 8);

            Label Acronym = new() {Content = D.name_acronym, Background = MainColor, BorderBrush = Border, BorderThickness = Default, HorizontalContentAlignment = HorizontalAlignment.Center }; 
            GridPlace(Acronym, DV, 5, 0, 1, 3);

            Label TeamName = new() {Content = D.team_name, Background = MainColor, BorderBrush = Border, BorderThickness = Default, HorizontalContentAlignment = HorizontalAlignment.Center }; 
            GridPlace(TeamName, DV, 5, 3, 1, 5);

            BitmapImage b = new(); //Get image into a WPF image element 
            b.BeginInit();
            b.UriSource = new Uri(D.headshot_url, UriKind.Absolute);
            b.CacheOption = BitmapCacheOption.OnLoad;
            b.EndInit();
            var img = new System.Windows.Controls.Image() { Source = b };
            GridPlace(img, DV, 1, 0, 3, 3);

            
            Task t = ShowPastSessions(Parent, D, DV, MainColor);
        }

        private async Task ShowPastSessions(MainWindow Parent, Driver D, Grid DV, SolidColorBrush BG)
        {
            //ListBox to display drivers recent results
            ListBox Results = new() { Background = BG };
            ScrollViewer.SetVerticalScrollBarVisibility(Results, ScrollBarVisibility.Hidden);
            var JSONData = await Parent.APICall($"sessions?session_type=Race&year=2025");
            List<Session> Sessions = JsonConvert.DeserializeObject<List<Session>>(JSONData);

            List<string> DisplayData = new();
            foreach(Session S in Sessions)
            {
                
                var posJSON = await Parent.APICall($"position?session_key={S.session_key}&driver_number={D.driver_number}");
                Position p = JsonConvert.DeserializeObject<List<Position>>(posJSON).FirstOrDefault();

                DisplayData.Add($"{S.country_name} {S.session_name}: P{p.position}");
            }
            Results.ItemsSource = DisplayData;
            GridPlace(Results, DV, 1, 3, 3, 5);
        }
        private void GridPlace(UIElement e, Grid g, int row = 0, int col = 0, int rowspan = 1, int colspan = 1)
        {
            Grid.SetRow(e, row); Grid.SetColumn(e, col);
            Grid.SetRowSpan(e, rowspan); Grid.SetColumnSpan(e, colspan);
            g.Children.Add(e);
        }
    }
}
