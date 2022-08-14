using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo.Forms
{
    public partial class frmStock : Form
    {
        public frmStock()
        {
            InitializeComponent();
            panel1.Visible = false;
        }

        private void save_Click(object sender, EventArgs e)
        {
            try
            {
                double qty = double.Parse(textBox5.Text);

                Utils.updateProductQty(textBox1.Text, qty);

                MessageBox.Show("Updated");
                button1.PerformClick();

            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            
            
            if (e.KeyCode == Keys.Enter)
            {
                Product p = Utils.search(textBox1.Text);
                if(p == null)
                {
                    MessageBox.Show("No product found");
                    return;
                }
                panel1.Visible = true;
                textBox2.Text = p.PLU;
                textBox3.Text = p.desc;
                textBox4.Text = p.price.ToString();

                textBox5.Text = Utils.getProductQty(p.PLU).ToString() ;


            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text ="";
            textBox3.Text ="";
            textBox4.Text = "";
            panel1.Visible = false;
            textBox1.Focus();

        }
    }
}
