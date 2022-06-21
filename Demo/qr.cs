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
    public partial class qr : Form
    {
        public qr(Bitmap qr)
        {
            InitializeComponent();
            pictureBox1.Size = qr.Size;
            pictureBox1.Image = qr;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
