using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
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
        Dictionary<string, char> TypeKey = new Dictionary<string, char>();


        public AddProd()
        {
            InitializeComponent();
            Utils.log("Admin menu accessed");

            List<string> type = Utils.getTypes();
            //Dict to get the Key for sql query
            TypeKey = Utils.getTypeDict();
            comboBox1.Items.AddRange(type.ToArray());

            //enable initially for creating new items
            comboBox1.Enabled = false;
            comboBox1.SelectedIndex=0;

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        // below is messy, may look at tidying up but does for now
        //May look at moving to service down the line.
        // works fine for now 
        private void button1_Click(object sender, EventArgs e)
        {
            if (PLU.Text == "") // no text, return no need to do anything 
            {
                return;
            }
            if (changed) // if text or/anything has changed 
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
            ///if not changed 
            ///try recall existing item
            ///if p is null (does not exist)
            ///Lets create a new item
            Product p=Utils.search(PLU.Text);
            if (p == null)
            {
                if (!textChanged()) // check if price and desc have changed 
                {
                    this.PLU.Enabled = false;
                    comboBox1.Enabled = true;
                    return;
                }
                MySqlConnection cnn = new MySqlConnection();
                cnn.ConnectionString = connString;
                int allFra = 0;
                try
                {
                    cnn.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = cnn;

                    if (checkBox1.Checked)
                    {
                        allFra = 1;
                    }
                    // now lets get the Type Key
                    char typeID = 'N';
                    TypeKey.TryGetValue(comboBox1.SelectedItem.ToString(), out typeID);
                    //insert 

                    cmd.CommandText = "INSERT INTO product VALUES('" + PLU.Text + "','" + Desc.Text + "'," + Double.Parse(Price.Text) + "," + allFra + ",'"+typeID+"')";
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
                comboBox1.Enabled = false;
                comboBox1.SelectedItem = TypeKey.FirstOrDefault(x => x.Value == p.type).Key;
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
            comboBox1.Enabled = true;
            PLU.Enabled = true;
            comboBox1.Enabled = false;
            this.changed = false;
        }
        private void Form1_Closing(object sender, CancelEventArgs e)
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
