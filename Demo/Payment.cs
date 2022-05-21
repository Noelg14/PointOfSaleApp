using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        public Payment(double toPay,Cart c)
        {
            InitializeComponent();
            label1.Text += " € " + toPay;
            this.toPay= toPay;
            this.cart = c;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Utils.recPayment(button1.Text, toPay, cart);
            MessageBox.Show("Paid €" + toPay,"Paid");
            this.paid = true;
            sendNotif();
            this.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Utils.recPayment(button3.Text, toPay, cart);
            MessageBox.Show("Paid €" + toPay, "Paid");
            this.paid = true;
            sendNotif();
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Utils.recPayment(button2.Text, toPay, cart);
            MessageBox.Show("Paid €" + toPay, "Paid");
            this.paid = true;
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
    }
}
