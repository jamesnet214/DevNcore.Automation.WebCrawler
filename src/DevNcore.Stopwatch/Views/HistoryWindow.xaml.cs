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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DevNcore.Stopwatch.Views
{
    /// <summary>
    /// SettingWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class HistoryWindow : Window
    {
        HistoryViewModel vm { get; set; }
        public HistoryWindow()
        {
            InitializeComponent();
            vm = new HistoryViewModel(this);
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

        private void 클릭이벤트_이동(object sender, RoutedEventArgs e)
        {

        }
    }
}
