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

                // Get the BackgroundWorker that raised this event.
                BackgroundWorker worker = sender as BackgroundWorker;

                // Assign the result of the computation
                // to the Result property of the DoWorkEventArgs
                // object. This is will be available to the 
                // RunWorkerCompleted eventhandler.
                /////#
                // Add senidng here.

                //List<Product> prod = Utils.getButtons();
                //worker.ReportProgress(10);
                //int prog =(int) (100 - 90 / prod.Count);
                //int current = 10;
                //foreach (Product p in prod)
                //{
                //    current += prog;
                //    if(current > 100)
                //    {
                //        current = 100;
                //    }
                //    worker.ReportProgress(current);
                //}

                List<Models.sales> sale = dbFunctions.GetSales();
                worker.ReportProgress(25);

                File.AppendAllText("Log.txt", "Got sales\n");

                List<Models.salesline> saleline = dbFunctions.GetSaleslines();
                worker.ReportProgress(50);


                File.AppendAllText("Log.txt", "Got lines\n");

                exportItem.sales = sale;
                exportItem.saleline = saleline;


                worker.ReportProgress(60);
                //Thread.Sleep(500);
                //send to Server
                worker.ReportProgress(75);

                string jsonObj = JsonConvert.SerializeObject(exportItem);

                File.AppendAllText("Log.txt", "Conv to json\n");

                File.WriteAllText("JSON.txt", jsonObj);

                try
                {
                    string apiURL = Utils.getConfig("HO_SERVER");
                    HttpContent content = new StringContent(jsonObj, Encoding.UTF8, "application/json");
                    File.AppendAllText("Log.txt", "Sending \n");
                    _httpclient.PostAsync(apiURL, content);
                    Thread.Sleep(100);
                    //res.EnsureSuccessStatusCode();
                    
                    File.AppendAllText("Log.txt", "Sent\n");


                    worker.ReportProgress(80);
                    //update to show posted 
                    string rows = dbFunctions.updateSales().ToString();
                    File.AppendAllText("Log.txt", $"Updated {rows} sale rows");                
                    
                    rows = dbFunctions.updateSaleLine().ToString();
                    File.AppendAllText("Log.txt", $"Updated {rows} saleline rows");  
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    File.AppendAllText("Log.txt", "Err\n");
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