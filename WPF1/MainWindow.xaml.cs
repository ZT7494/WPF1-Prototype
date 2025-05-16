using ScottPlot;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using ScottPlot.Colormaps;
using API_testing;
using Newtonsoft.Json;
using System.Windows.Media.TextFormatting;
using System.Net;
using System.Runtime.Intrinsics.Arm;

namespace WPF1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static HttpClient Client = new HttpClient() { BaseAddress = new Uri("https://api.openf1.org/v1/")};
        private Plotter Plotter = new Plotter();
        private Grid grid = new Grid() {
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
        };

        public MainWindow()
        {
            InitializeComponent();
            //Main Window Setup:
            //this.Width = 900; this.Height = 900;
            this.WindowState = WindowState.Maximized;
            this.Top = 20; this.Left = 20;
            this.Background = new SolidColorBrush(System.Windows.Media.Colors.Black);

            for (int i = 0; i < 5; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
            this.Content = grid;

            Main();
        }

        private async void Main()
        {
            await ListBoxTest();
        }

        private void GridPlace( UIElement e, int c = 0, int r = 0, Grid g = null)
        {
            if (g == null) { g = this.grid; }
            Grid.SetRow(e, r); Grid.SetColumn(e, c);
            g.Children.Add(e);
        }
        
        private async Task ListBoxTest()
        {

            //try to make litbox for various items
            var JSONData = await APICall("drivers?session_key=latest");
            List<Driver> Drivers = JsonConvert.DeserializeObject<List<Driver>>(JSONData);
            ListBox DriverLB = new ListBox() { 
                SelectionMode = SelectionMode.Multiple,
                ItemsSource = Drivers,
                DisplayMemberPath = "full_name",
            };
            GridPlace(DriverLB);


            List<string>stats = new List<string>() { "Brake", "Gears","RPM", "Speed", "Throttle (%)" };
            ListBox StatLB = new()
            {
                SelectionMode = SelectionMode.Single,
                ItemsSource = stats,
            };
            GridPlace(StatLB, 1, 0);
            




            Button b = new Button();
            
            b.Click += (s, e) => { 
                
            };

            
        }

        
        

        async Task APItest()
        {
            var JSONData = await APICall("car_data?driver_number=81&session_key=latest&speed>310");
            List<CarStats> stats = JsonConvert.DeserializeObject<List<CarStats>>(JSONData);
            List<double> data = new();
            foreach (CarStats cs in stats)
            {
                data.Add(cs.getAtts()[3]);
            }
            System.Windows.Controls.Image img = Plotter.Plot(data.ToArray(), Generate.ConsecutiveSeconds(5332, new DateTime(2025, 5, 4, 21, 00, 00)), sizeX: (int)this.ActualWidth, sizeY: (int)(this.ActualHeight * 0.4));

            Grid.SetRow(img, 1);
            Grid.SetColumn(img, 0);
            Grid.SetRowSpan(img, 2);
            Grid.SetColumnSpan(img, 5);
            grid.Children.Add(img);
        }

        async Task<string> APICall(string relativePath)
        {
            var request = new HttpRequestMessage(new HttpMethod("GET"), relativePath);
            var response = await Client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        
    }
}