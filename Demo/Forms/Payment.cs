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
        List<PayItem> payments = new List<PayItem>();

        public Payment(double toPay,Cart c)
        {
            InitializeComponent();
            label1.Text += "€ " + Math.Round(toPay, 2, MidpointRounding.ToEven);
            //label1.Text += " € " + toPay;
            this.toPay= toPay;
            this.cart = c;
            string qr = Utils.getConfig("USEQR");
            if (qr.Equals("Y"))
            {
                useQR = true;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            pay(button1.Text);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            pay(button3.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pay(button2.Text);
        }

        private void pay(string text)
        {
            payments.Add(new PayItem(text, toPay));
            Utils.recPayment(text, toPay, cart);
            Utils.recSale(this.cart, this.toPay);
            MessageBox.Show("Paid €" + toPay, "Paid");
            this.paid = true;
            sendNotif();
            if (useQR)
            {
                showQR(cart);
            }
            if (Utils.getConfig("SENDSALES").Equals("Y"))
            {
                Process.Start("sendtomaster.exe");
            }
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
