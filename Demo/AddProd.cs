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
        string connString = Utils.getConfig();
        public AddProd()
        {
            InitializeComponent();
            Utils.log("Admin menu accessed");
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            MySqlConnection cnn = new MySqlConnection();
            cnn.ConnectionString= connString;
            int allFra = 0;
            try
            {
                cnn.Open();
                MySqlCommand cmd = new MySqlCommand();
                //cmd.CommandText = "INSERT INTO product VALUES(" + PLU.Text + "," + Desc.Text + ","+ Price.Text + ","++")";
                cmd.Connection = cnn;

                //cmd.Prepare();
                //cmd.Parameters.AddWithValue("@plu", PLU.Text);
                //cmd.Parameters.AddWithValue("@desc", Desc.Text);
                //cmd.Parameters.AddWithValue("@price", Price.Text);
                if (checkBox1.Checked)
                {
                     allFra = 1;
                }



                cmd.CommandText = "INSERT INTO product VALUES('" + PLU.Text + "','" + Desc.Text + "'," + Double.Parse(Price.Text) + ","+allFra+")";
                Console.WriteLine(cmd.CommandText);
                
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
        }
    }
}
