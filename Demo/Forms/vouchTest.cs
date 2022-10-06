using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;

namespace Demo.Forms
{
    public partial class vouchTest : Form
    {
        public vouchTest()
        {
            InitializeComponent();
            textBox2.Enabled = false;
            this.Text = "Get Voucher";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                Voucher vouch = Utils.GetVoucher(textBox1.Text);
                if(vouch.number is not null)
                {
                    textBox1.Text = vouch.number;
                    textBox2.Text = vouch.balance.ToString() ;

                    textBox2.Enabled = true;
                    textBox1.Enabled = false ;
                }
                else
                {
                    MessageBox.Show("Not found");
                    textBox1.SelectAll();
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Utils.UpdateVoucher(textBox1.Text, Double.Parse(textBox2.Text));
            clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            clear();


        }
        private void clear()
        {
            textBox1.Enabled = true;
            textBox2.Enabled = false;

            textBox1.Text = "";
            textBox2.Text = "";
        }
    }
}
