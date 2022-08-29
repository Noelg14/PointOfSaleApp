using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Demo;
using Newtonsoft.Json;
using System.Net;


namespace sendToMaster
{
    public partial class Form1 : Form
    {
        Models.exportItem exportItem = new Models.exportItem();
        Models.stock stock = new Models.stock();

        public static HttpClient _httpclient = new HttpClient();
        public Form1()
        {
            InitializeComponent();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            InitializeBackgroundWorker();

            backgroundWorker1.RunWorkerAsync();

            File.AppendAllText("Log.txt", "Init file\n");



        }


        private void InitializeBackgroundWorker()
        {
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);

            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);

            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);

        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // First, handle the case where an exception was thrown.
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else
            {
                Application.Exit();
            }

        }
        private void backgroundWorker1_ProgressChanged(object sender,ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
        }
        private async void backgroundWorker1_DoWork(object sender,DoWorkEventArgs e)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                BackgroundWorker worker = sender as BackgroundWorker;



                List<Models.sales> sale = dbFunctions.GetSales();
                worker.ReportProgress(25);

                File.AppendAllText("Log.txt", "Got sales\n");

                List<Models.salesline> saleline = dbFunctions.GetSaleslines();
                worker.ReportProgress(50);

                List<Models.stock> stock = dbFunctions.GetStock();

                File.AppendAllText("Log.txt", "Got lines\n");

                exportItem.sales = sale;
                exportItem.saleline = saleline;


                worker.ReportProgress(60);
                //Thread.Sleep(500);
                //send to Server
                worker.ReportProgress(75);
                string stockObj = JsonConvert.SerializeObject(stock);

                string jsonObj = JsonConvert.SerializeObject(exportItem);

                File.AppendAllText("Log.txt", "Conv to json\n");

                File.WriteAllText("JSON.txt", jsonObj);

                try
                {
                    #region Send Sales
                    string apiURL = Utils.getConfig("HO_SERVER");
                    HttpContent content = new StringContent(jsonObj, Encoding.UTF8, "application/json");
                    File.AppendAllText("Log.txt", "Sending \n");
                    Thread.Sleep(100);
                    HttpResponseMessage hrm = _httpclient.PostAsync(apiURL+"/sales", content).Result;
                    if (hrm.IsSuccessStatusCode)
                    {
                        //res.EnsureSuccessStatusCode();

                        File.AppendAllText("Log.txt", "Sent\n");


                        worker.ReportProgress(80);
                        //update to show posted 
                        string rows = dbFunctions.updateSales().ToString();
                        File.AppendAllText("Log.txt", $"Updated {rows} sale rows");

                        rows = dbFunctions.updateSaleLine().ToString();
                        File.AppendAllText("Log.txt", $"Updated {rows} saleline rows");
                    }
                    #endregion

                   /* #region Stock
                    content = new StringContent(stockObj, Encoding.UTF8, "application/json");
                    File.AppendAllText("Log.txt", "Sending Stock \n");
                    Thread.Sleep(150);
                    hrm = _httpclient.PostAsync(apiURL + "/stock", content).Result;
                    if (hrm.IsSuccessStatusCode)
                    {
                        //res.EnsureSuccessStatusCode();

                        File.AppendAllText("Log.txt", "Sent Stock\n");
                        worker.ReportProgress(90);
                    }
                    #endregion

                    else
                    {
                        throw new Exception("An error ocurred sending data \n "+hrm.Content);
                    }
                   */

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    File.AppendAllText("Log.txt", ex.Message);
                }

                worker.ReportProgress(100);

                //MessageBox.Show("Complete");
            }
            catch(Exception em)
            {
                MessageBox.Show(em.Message);

                File.AppendAllText("Log.txt", em.StackTrace);
            }
        }

        //  worker.ReportProgress(percentComplete);
    }
}