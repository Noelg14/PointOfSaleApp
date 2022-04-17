using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Demo
{
    public partial class AddProd : Form
    {
        Form1 f;
        public AddProd(Form1 form1)
        {
            f = form1;
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection cnn;
            cnn = new MySqlConnection(Utils.getConfig());
            try
            {
                cnn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = ("INSERT INTO product values(@plu,@desc,@price,@allfra)");
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@plu", PLU.Text);
                cmd.Parameters.AddWithValue("@desc", Desc.Text);
                cmd.Parameters.AddWithValue("@price", Price.Text);
                if (checkBox1.Checked)
                {
                    cmd.Parameters.AddWithValue("@allfra", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@allfra", 0);
                }

                cmd.ExecuteNonQuery();
                MessageBox.Show("Products Added");
                clear();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                cnn.Close();
            }

        }

        private void PLU_TextChanged(object sender, EventArgs e)
        {

        }

        private void clear()
        {
            PLU.Text = "";
            Desc.Text = "";
            Price.Text = "";
            checkBox1.Checked = false;
        }
        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            f.textBox1.Enabled = true;
        }
    }
}
