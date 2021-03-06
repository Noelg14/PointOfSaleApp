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
        bool changed = false;
        public AddProd()
        {
            InitializeComponent();
            Utils.log("Admin menu accessed");
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        // below is messy, may look at tidying up but does for now
        private void button1_Click(object sender, EventArgs e)
        {
            if (PLU.Text == "")
            {
                return;
            }
            if (changed)
            {
                MySqlConnection cnn = new MySqlConnection();
                cnn.ConnectionString = connString;
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = cnn;

                if (Desc.Text.Equals("") || Price.Text.Equals(""))
                {
                    return;
                }
                try
                {
                    /*
                     * Desc seems to be ambiguous so needs to be product.
                     * 
                     */
                    cmd.CommandText = "Update product SET product.DESC='" + Desc.Text + "',price=" + Double.Parse(Price.Text) + ",allfra=" + checkBox1.Checked + "" +
                    " where PLU='" + PLU.Text + "';";

                    cnn.Open();
                    cmd.ExecuteNonQuery();
                    clear();
                    MessageBox.Show("Updated Product");
                    Utils.log("updated product " + PLU.Text);
                    changed = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An Error occurred \n " + ex.Message, "Oops");
                }
                finally
                {
                    cnn.Close();
                }
                return;
            }
            Product p=Utils.search(PLU.Text);
            if (p == null)
            {
                if (!textChanged())
                {
                    this.PLU.Enabled = false;
                    return;
                }
                MySqlConnection cnn = new MySqlConnection();
                cnn.ConnectionString = connString;
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
                    cmd.CommandText = "INSERT INTO product VALUES('" + PLU.Text + "','" + Desc.Text + "'," + Double.Parse(Price.Text) + "," + allFra + ")";
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Products Added");
                    Utils.log("Product added : " + PLU.Text);
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
            else
            {
                Utils.log("Product exists, recalling");
                PLU.Text = p.PLU;
                Desc.Text = p.desc;
                Price.Text = p.price.ToString();
                checkBox1.Checked= p.allowFra ? true : false;
                changed = true;
                PLU.Enabled = false;
            }
        }

        private void PLU_TextChanged(object sender, EventArgs e)
        {
            button2.Text = "Clear";
        }

        private void clear()
        {
            PLU.Text = "";
            Desc.Text = "";
            Price.Text = "";
            checkBox1.Checked = false;
            PLU.Enabled = true;
            this.changed = false;
        }
        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void PLU_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)13)
            {
                button1.PerformClick();
            }
        }
        private bool textChanged()
        {
            if(this.Price.Text == "" || this.Desc.Text == "")
            {
                return false;
            }
            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        { 
            if (!textChanged() && PLU.Text=="")
            {
                this.Dispose();
                return;
            }
            clear();
            button2.Text = "Back";

        }
    }
}
