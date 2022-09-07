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
    public partial class settings : Form
    {
        public settings()
        {
            InitializeComponent();
            getInitData();
            //comboBox1.Items.AddRange();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void getInitData()
        {
            Dictionary<string,string> kv = Utils.getSettings();
            if (kv.Equals(null))
            {
                return;
            }
            textBox1.Text = kv.GetValueOrDefault("name");
            textBox2.Text = kv.GetValueOrDefault("address");
            textBox3.Text = kv.GetValueOrDefault("message");
        }

        private void updateData()
        {
            Utils.updateSettings("name", textBox1.Text);
            Utils.updateSettings("address", textBox2.Text);
            Utils.updateSettings("message", textBox3.Text);
            MessageBox.Show("Update Sucessful");
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            updateData();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            DialogResult res = MessageBox.Show($"Are you sure you want to save {colorDialog1.Color.Name} as your new background?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (res == DialogResult.OK)
            {
                Utils.updateSettings("color", colorDialog1.Color.ToArgb().ToString()) ;
                MessageBox.Show("Change will apply on next start up");
            }
            if (res == DialogResult.Cancel)
            {
            }
        }

        private void settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Form1.notifyColor();
        }
    }
}
