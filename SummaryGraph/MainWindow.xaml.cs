using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SummaryGraph
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        Thread timer;
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            BarCtr.DrawBackground();
            CurveCtr.DrawBackground();
            timer = new Thread(new ThreadStart(() =>
            {
                Random rd = new Random();
                while (true)
                {
                    try
                    {

                        Dispatcher.Invoke(new Action(() =>
                        {
                            double d = rd.Next(20, 30);
                            BarCtr.DrawLine(d);
                            CurveCtr.DrawLine(d);
                        }));

                    }
                    catch (Exception)
                    {
                    }
                    Thread.Sleep(700);
                }
            }));
            timer.Start();
        }
        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            timer.Abort();
            timer = null;
        }
    }
}
