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
using ClosedXML.Excel;
using System.Diagnostics;
using System.Windows;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Demo
{
    public partial class frmManage : Form
    {

        public frmManage()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            if (Utils.getIndiviudalSetting("USE_CHARTS").Equals("Y"))
            {
                initChart(getData(), getDates());
            }
            else
            {
                cartesianChart1.Visible = false;
            }

            dropdown.Items.AddRange(Utils.getExports().ToArray());
            dropdown.SelectedIndex = 0;
            this.Text = "Reporting";
           
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
        private void initChart<T>(string yName,List<T> yCol,string xName, List<string> xCol)
        {
            cartesianChart1.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title= yName,
                    Values= new ChartValues<T>(yCol)
                }
            };
            cartesianChart1.AxisX.Add(new Axis
            {
                Title = xName,
                Labels = xCol
            });

            cartesianChart1.AxisY.Add(new Axis
            {
                Title = yName,
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
        private async void button2_Click(object sender, EventArgs e)
        {
            clearChart();
            Utils.GeneralExport("stock");
            //initChart<double>("Qtys", , "Products",);

        }
        private void Export_Click(object sender, EventArgs e)
        {
            //Utils.ExcelExport(getData());
            //Utils.ExcelExport(getDates(), getData());

            DialogResult result = MessageBox.Show("Run Selected Report?", "Run?", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                string file = ReportService.createDocument(dropdown.Text);
                if (file.Equals(""))
                {
                    MessageBox.Show("An error occurred generating the report");
                    return;
                }
                result = MessageBox.Show("Open report?", "Open report?", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    ProcessStartInfo psi = new ProcessStartInfo(file);
                    psi.UseShellExecute = true;
                    psi.WindowStyle = ProcessWindowStyle.Minimized;
                    Process.Start(psi);
                }
                if (result == DialogResult.No)
                {
                    //ReportService.createDocument();
                }
                //ReportService.createDocument();
            }

        }
        private void button3_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Excel Files (*.xlsx,*.xls)|*.xls;*.xlsx";
            openFile.Title = "Open File";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                filePath = openFile.FileName;
                XLWorkbook xl = new XLWorkbook(filePath);
                var Data = xl.Worksheet(1).Row(2).Cells("A2:B2");
                string data = "";
                foreach(var cell in Data)
                {
                    data+=cell.Value+",";
                }
                MessageBox.Show(data.ToString(),"First Row" );
            }
            else
            {

            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string selected = dropdown.Text;
            if (selected.Equals(""))
            {
                MessageBox.Show("Please select a report below");
            }
            else {
                Utils.GeneralExport(selected);
                
            }

        }
    }
}
