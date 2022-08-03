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
    }
}
