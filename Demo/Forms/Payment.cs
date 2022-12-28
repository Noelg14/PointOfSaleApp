using Demo.Services;
using DocumentFormat.OpenXml.Vml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    public partial class Payment : Form
    {
        double toPay { get; set; }
        Cart cart { get; set; }
        bool paid { get; set; } = false;
        bool useQR = false;
        bool sendSales = false;
        List<PayItem> payments = new List<PayItem>();

        public Payment(double toPay, Cart c)
        {
            InitializeComponent();
            label1.Text += "€ " + Math.Round(toPay, 2, MidpointRounding.ToEven);
            //label1.Text += " € " + toPay;
            this.toPay = toPay;
            this.cart = c;
            string qr = Utils.getConfig("USEQR");
            if (qr.Equals("Y"))
            {
                useQR = true;
            }
            if (Utils.getConfig("SENDSALES").Equals("Y"))
            {
                sendSales = true;


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pay(button1.Text);

        }

        private void button3_Click(object sender, EventArgs e) //Vouchers
        {
            if(textBox1.Visible == false)
            {
                textBox1.Visible = true;
                MessageBox.Show("Please enter voucher number ", "Enter Voucher");
            }
            if (textBox1.Visible == true && textBox1.Text != "")
            {

                Models.Voucher v = VoucherService.getVoucherDetails(textBox1.Text);
                if(v is null)
                {
                    MessageBox.Show("voucher does not exist");
                    return;
                }

                if (v.Balance < toPay)
                {
                    MessageBox.Show("Cannot use this voucher, please try another");
                    return;
                }
                string msg = $"Voucher has €{v.Balance} on it, are you sure you want to use €{toPay}?";
                if (toPay < 0)
                {
                    msg = $"Voucher will be topped up by €{Math.Abs(toPay)} & will have a balance of €{v.Balance + Math.Abs(toPay)}, Continue?";
                }

                DialogResult res =MessageBox.Show(msg,"Check",MessageBoxButtons.OKCancel);
                if(res != DialogResult.OK)
                {
                    textBox1.Text = "";
                    textBox1.Visible = false ;
                    return;
                }
                else
                {

                    VoucherService.UpdateVoucher(v.Id, toPay, cart);
                    pay(button3.Text);
                }

                //pay(button3.Text);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            pay(button2.Text);
        }

        private void pay(string text) // should update this to be an EventHandler, then add to button.click and read text via cast of sender.
        {
            payments.Add(new PayItem(text, toPay));
            Utils.recPayment(text, toPay, cart);
            Utils.recSale(this.cart, this.toPay);
            //MessageBox.Show("Paid €" + Math.Round(toPay,2), "Paid");
            this.paid = true;
            if (useQR)
            {
                showQR(cart);
            }
            DialogResult result = MessageBox.Show("Create pdf receipt?", "Create & Open?", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                
                ProcessStartInfo psi = new ProcessStartInfo(ReportService.createRecDocument("",cart,text));
                psi.UseShellExecute = true;
                psi.WindowStyle = ProcessWindowStyle.Minimized;
                Process.Start(psi);
            }
            if (Utils.getConfig("SENDSALES").Equals("Y"))
            {
                Process.Start("sendtomaster.exe");
            }
            sendNotif();
            this.Dispose();
        }
        public bool isPaid()
        {
            return paid;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Form1.notifyBack();
            //Form1.notify();
            this.Dispose();
         
        }

        public static bool newPayment(Double toPay,Cart c)
        {
            Payment p = new Payment(toPay, c);
            p.Show();
            return p.paid;
        }

        private static void sendNotif()
        {
            Form1.notify();
        }

        private void Payment_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1.notifyBack();
        }

        private void showQR(Cart c)
        {
            new qr(Utils.genQR(c.id.ToString())).Show();
        }

    }
}
