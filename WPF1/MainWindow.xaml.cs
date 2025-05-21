using ScottPlot;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using API_testing;
using Newtonsoft.Json;
using System.Windows.Controls.Primitives;
using Colors = System.Windows.Media.Colors;

namespace WPF1
{                  
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public static HttpClient Client = new HttpClient() { BaseAddress = new Uri("https://api.openf1.org/v1/")}; //The same instance of a HTTP client can be used across the program

        private Plotter Plotter = new Plotter();

        Style SBStyle = new Style(typeof(ScrollBar));
        Style LBStyle = new Style(typeof(ListBox));

        

        


        //The main UI is a grid, which handles all resizing and spacing.
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
            this.Background = new SolidColorBrush(Colors.Black);

            for (int i = 0; i < 5; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
            this.Content = grid;
            StyleSetup();
            Main();
        }

        private void StyleSetup()
        {
            
            SBStyle.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush(Colors.DarkSlateGray)));
            SBStyle.Setters.Add(new Setter(Control.ForegroundProperty, new SolidColorBrush(Colors.DarkRed)));
            SBStyle.Setters.Add(new Setter(Control.BorderBrushProperty, new SolidColorBrush(Colors.Black)));

            LBStyle.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush(Colors.Black)));
            LBStyle.Setters.Add(new Setter(Control.ForegroundProperty, new SolidColorBrush(Colors.WhiteSmoke)));
            LBStyle.Setters.Add(new Setter(Control.BorderBrushProperty, new SolidColorBrush(Colors.DarkSlateGray)));

            Resources.Add(typeof(ScrollBar), SBStyle);
            Resources.Add(typeof(ListBox), LBStyle);
        }

        private async void Main()
        {
            DataPlot d = new(this, Plotter, grid, 0, 0, 2, 5);
        }
        private void GridPlace( UIElement e, int c = 0, int r = 0, Grid g = null, int cs = 1, int rs = 1)
        {
            if (g == null) { g = this.grid; }
            Grid.SetRow(e, r); Grid.SetColumn(e, c);
            Grid.SetRowSpan(e, rs); Grid.SetColumnSpan(e, cs);
            g.Children.Add(e);
        }

        public async Task<string> APICall(string relativePath)
        {
            var request = new HttpRequestMessage(new HttpMethod("GET"), relativePath);
            var response = await Client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }
    }
}