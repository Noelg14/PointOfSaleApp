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
    public partial class VouchEntry : Form
    {
        public VouchEntry()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                double m = double.Parse(textBox1.Text.Replace("€", string.Empty));
                Form1.notifyVoucher(m);
            }catch(Exception ex) {
                Utils.log("error");
            }
            finally
            {
                this.Dispose();
            }

        }
    }
}
