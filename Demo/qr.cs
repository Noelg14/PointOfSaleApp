using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

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

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "JPeg Image|*.jpg";
                saveFileDialog.Title = "Save an Image File";
                saveFileDialog.ShowDialog();

                if (saveFileDialog.FileName != "")
                {
                  
                    Stream fs = (System.IO.FileStream)saveFileDialog.OpenFile();
                    this.pictureBox1.Image.Save(fs,System.Drawing.Imaging.ImageFormat.Jpeg);
                    fs.Close();
                    MessageBox.Show("Saved");
                }
                this.Dispose();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An error ocurred");
            }
        }
    }
}
