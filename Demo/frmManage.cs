using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using LiveCharts;
using LiveCharts.Wpf;


namespace Demo
{
    public partial class frmManage : Form
    {
        public frmManage()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            initChart(getData(), getDates());
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clearChart();
            initChart(getData(), getDates());
        }
        private void initChart(List<double> sales,List<string> dates)
        {
            cartesianChart1.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title="Sales",
                    Values= new ChartValues<double>(sales)
                }
            };
            cartesianChart1.AxisX.Add(new Axis
            {
                Title = "Dates",
                Labels = dates.ToArray()
            });

            cartesianChart1.AxisY.Add(new Axis
            {
                Title = "Sold",
                LabelFormatter = value => value.ToString("N")
            });
        }
    
    
        private void demoChart()
        {
            cartesianChart1.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "2015",
                    Values = new ChartValues<double> { 10, 50, 39, 50 }
                }
            };

            //adding series will update and animate the chart automatically
            cartesianChart1.Series.Add(new ColumnSeries
            {
                Title = "2016",
                Values = new ChartValues<double> { 11, 56, 42 }
            });

            //also adding values updates and animates the chart automatically
            //cartesianChart1.Series[1].Values.Add(48d);

            cartesianChart1.AxisX.Add(new Axis
            {
                Title = "Sales Man",
                Labels = new[] { "Maria", "Susan", "Charles", "Frida" }
            });

            cartesianChart1.AxisY.Add(new Axis
            {
                Title = "Sold Apps",
                LabelFormatter = value => value.ToString("N")
            });

        }
        private List<string> getDates()
        {
            return Utils.getSalesDates();
        }
        private List<double> getData()
        {
            return Utils.getSalesDouble();
        }

        private void clearChart()
        {
            this.cartesianChart1.Series.Clear();
            this.cartesianChart1.AxisX.Clear();
            this.cartesianChart1.AxisY.Clear();
            
        }
    }
}
