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
using System.Windows.Shapes;

namespace SummaryGraph
{
    /// <summary>
    /// Window2.xaml 的交互逻辑
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
        }

        Thread timer;
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            DrawBackground();

            timer = new Thread(new ThreadStart(() =>
            {
                Random rd = new Random();
                while (true)
                {
                    try
                    {

                        Dispatcher.Invoke(new Action(() =>
                        {
                            double d = rd.Next(MinReg, MaxReg);
                            DrawLine(d);
                            Console.WriteLine(d + "");
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
        #region 绘制折线
        int StepLength = 20;//X轴步长（两个顶点点X轴的距离）
        int MaxCount = 8;//顶点最多个数
        int MaxReg = 30;//最大量程‘
        int MinReg = 20;//最大量程
        //底部数字列表
        List<TextBlock> listBottom = new List<TextBlock>();
        //折线图
        Polyline pline = new Polyline();
        /// <summary>
        /// 画线
        /// </summary>
        /// <param name="Y2">线段终点Y轴坐标</param>
        private void DrawLine(double Y2)
        {
            //将值转换为图上坐标
            Y2 = grdMain.Height - (grdMain.Height / (MaxReg - MinReg)) * (Y2 - MinReg);
            //判断折线图顶点集合个数是否大于0
            if (pline.Points.Count > 0)
            {
                //向折线图顶点集合添加新线段终点坐标
                pline.Points.Add(new Point((pline.Points[pline.Points.Count - 1].X + StepLength), Y2));
                //判断顶点集合个数是否超过最大个数
                if (MaxCount + 1 < pline.Points.Count)
                {
                    //将曲线及下方数字往左移动

                    //删除第一个点
                    pline.Points.Remove(pline.Points[0]);
                    int ForLen = (pline.Points.Count > listBottom.Count) ? pline.Points.Count : listBottom.Count;
                    //将点击数字往左移动一位
                    for (int i = 0; i < ForLen; i++)
                    {
                        if (i < pline.Points.Count)
                        {
                            //将顶点的X轴坐标减去步长
                            pline.Points[i] = new Point(pline.Points[i].X - StepLength, pline.Points[i].Y);
                        }
                        if (i < listBottom.Count)
                        {
                            listBottom[i].Text = (int.Parse(listBottom[i].Text) + 1).ToString();
                        }
                    }
                }
            }
            else
            {//第一次添加顶点

                //设置线条颜色
                pline.Stroke = new SolidColorBrush(Colors.Red);
                //设置线条宽度
                pline.StrokeThickness = 1;

                //添加第一个点
                pline.Points.Add(new Point(0, Y2));
                //将折线控件作为子控件添加到界面
                this.grdMain.Children.Add(pline);
            }

            return;
            //=====================第二种写法===================
            Line line = new Line();
            double X1 = 0;
            double Y1 = grdMain.Height;
            if (grdMain.Children.Count > 0)
            {
                Line uPLine = (Line)grdMain.Children[grdMain.Children.Count - 1];
                X1 = uPLine.X2; Y1 = uPLine.Y2;
                if (X1 + StepLength > grdMain.Width)
                {
                    grdMain.Children.Remove(grdMain.Children[0]);
                    int ForLen = (grdMain.Children.Count > listBottom.Count) ? grdMain.Children.Count : listBottom.Count;
                    for (int i = 0; i < ForLen; i++)
                    {
                        if (i < grdMain.Children.Count)
                        {
                            ((Line)grdMain.Children[i]).X1 = ((Line)grdMain.Children[i]).X1 - StepLength;
                            ((Line)grdMain.Children[i]).X2 = ((Line)grdMain.Children[i]).X2 - StepLength;
                        }
                        if (i < listBottom.Count)
                        {
                            listBottom[i].Text = (int.Parse(listBottom[i].Text) + 1).ToString();
                        }
                    }
                    X1 = X1 - StepLength;
                }
            }



            line.X1 = X1;
            line.Y1 = Y1;

            X1 += StepLength;
            line.X2 = X1;
            line.Y2 = (grdMain.Height - (grdMain.Height / MaxReg) * Y2);
            Y1 = line.Y2;
            X1 = line.X2;

            line.Stroke = new SolidColorBrush(Colors.Red);
            line.StrokeThickness = 1;



            this.grdMain.Children.Add(line);
        }
        /// <summary>
        /// 绘制背景
        /// </summary>
        private void DrawBackground()
        {
            //==============绘制底部数字及绘制Y轴方向直线
            //计算顶点最大个数
            MaxCount = (int)(grdBackground.Width / StepLength);
            for (int i = 1; i <= MaxCount; i++)
            {
                //绘制Y轴直线
                Line line = new Line();
                //设置开始坐标及终点坐标
                line.X1 = StepLength * i;
                line.X2 = StepLength * i;
                line.Y1 = 0;
                line.Y2 = grdBackground.Height;

                Color color = new Color();
                color.R = 20;
                color.G = 80;
                color.B = 136;
                color.A = 100;
                //设置线条颜色
                line.Stroke = new SolidColorBrush(color);
                //设置线条宽度
                line.StrokeThickness = 2;

                //绘制下方数字
                TextBlock tb = new TextBlock();
                //设置字体颜色
                tb.Foreground = new SolidColorBrush(Colors.Red);
                //数字
                tb.Text = "" + i;
                //显示界面
                grdBackground.Children.Add(tb);
                grdBackground.Children.Add(line);
                //添加到全局变量
                listBottom.Add(tb);
                //设置坐标
                Canvas.SetBottom(tb, 0);
                Canvas.SetLeft(tb, StepLength * i);
            }

            //==============绘制左侧数字及绘制X轴方向直线（大概原理同上）
            for (int i = MinReg; i <= MaxReg; i++)
            {
                Line line = new Line();
                line.X1 = 0;
                line.X2 = grdBackground.Width;
                //grdMain.Height - (grdMain.Height / (MaxReg-MinReg)) * Y2
                line.Y1 = grdMain.Height - (grdMain.Height / (MaxReg - MinReg)) * (i - MinReg);
                line.Y2 = grdMain.Height - (grdMain.Height / (MaxReg - MinReg)) * (i - MinReg);
                Color color = new Color();
                color.R = 20;
                color.G = 80;
                color.B = 136;
                color.A = 100;
                line.Stroke = new SolidColorBrush(color);
                line.StrokeThickness = 2;

                TextBlock tb = new TextBlock();
                tb.Foreground = new SolidColorBrush(Colors.Red);
                tb.Text = "" + i;
                grdBackground.Children.Add(tb);
                grdBackground.Children.Add(line);
                //  listBottom.Add(tb);
                Canvas.SetTop(tb, grdMain.Height - (grdMain.Height / (MaxReg - MinReg)) * (i - MinReg));
                Canvas.SetLeft(tb, 0);
            }
            //设置线条颜色
            pline.Stroke = new SolidColorBrush(Colors.Red);
            //设置线条宽度
            pline.StrokeThickness = 1;

            //添加第一个点
            pline.Points.Add(new Point(0, this.grdMain.Height));
            //将折线控件作为子控件添加到界面
            this.grdMain.Children.Add(pline);
        }

        #endregion

    }
}
