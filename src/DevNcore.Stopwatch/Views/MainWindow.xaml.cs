using DevNcore.Stopwatch.ViewModels;
using System;
using System.Collections.Generic;
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

namespace DevNcore.Stopwatch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel vm { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            MouseLeftButtonUp += OnMouseLeftButtonUp;

            vm = new MainViewModel(this);
            DataContext = vm;
        }

        private void MainWindowDrag(object sender, MouseButtonEventArgs e)
        {
            try
            {                
                this.DragMove();
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e) 
        {
            int offset = 25; 
            foreach (Screen scr in Screen.AllScreens)
            { 
                if (Math.Abs(scr.WorkingArea.Left - this.Left) < offset)
                    this.Left = scr.WorkingArea.Left; 
                else if (Math.Abs(this.Left + this.ActualWidth - scr.WorkingArea.Left - scr.WorkingArea.Width) < offset) 
                    this.Left = scr.WorkingArea.Left + scr.WorkingArea.Width - this.ActualWidth; if (Math.Abs(scr.WorkingArea.Top - this.Top) < offset) this.Top = scr.WorkingArea.Top; 
                else if (Math.Abs(this.Top + this.ActualHeight - scr.WorkingArea.Top - scr.WorkingArea.Height) < offset) 
                    this.Top = scr.WorkingArea.Top + scr.WorkingArea.Height - this.ActualHeight; 
            } 
        }
    }
}
