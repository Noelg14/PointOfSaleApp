using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Demo.Models;
using Demo.Services;

namespace Demo.Forms
{
    public partial class TestUtils : Form
    {
        public TestUtils()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Models.Voucher v = VoucherService.getVoucherDetails(enterVouch.Text);

            if(v is null)
            {
                MessageBox.Show("Voucher Not Found");
            }
            else
            {
                MessageBox.Show(v.ToString());
                enterVouch.Focus();
            }
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(VoucherService.CreateNewVoucher(createV.Text, Double.Parse(createB.Text)));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string id = textBox2.Text;
            double usage = Double.Parse(textBox1.Text);

            double newBal = VoucherService.UpdateVoucher(id, usage);
            if (newBal == -1 || newBal == -99)
            {
                MessageBox.Show("Could not use voucher. Usage greater than balance or Voucher does not exist");
                return;
            }
            else
            {
                MessageBox.Show($"New Voucher Balance for ID : {id} is {newBal}");
            }


        }
    }
}
