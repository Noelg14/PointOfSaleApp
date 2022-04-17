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
        double toPay;
        Cart cart;

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
            this.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Utils.recPayment(button1.Text, toPay, cart);
            MessageBox.Show("Paid €" + toPay, "Paid");
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Utils.recPayment(button1.Text, toPay, cart);
            MessageBox.Show("Paid €" + toPay, "Paid");
            this.Dispose();
        }
    }
}
