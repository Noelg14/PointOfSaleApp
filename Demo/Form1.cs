using MySql.Data.MySqlClient;


namespace Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Rectangle r = Screen.FromControl(this).Bounds;
            button1.Left = (r.Width - (r.Width)/4);
            button1.Top= (r.Height - (r.Height)/4);
            panel1.Height= (r.Height - 200);
            textBox1.Width=panel1.Width;
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            Console.WriteLine("Button Pay Now Pressed");
            label1.Text += "\n PLU          DESC            €PRICE";
        }


        private Product searchProduct(String PLU)
        {

            /*
             * search PLU IN DB
             * return Product
             * 
             */

            return null;


        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if(textBox1.Text == "ADMIN")
                {
                    AddProd a = new AddProd(this);
                    a.Show();
                    textBox1.Enabled = false;
                    return;
                }
                Product p = searchProduct(textBox1.Text.ToString());
                if(p is null)
                {
                    MessageBox.Show(null,"No product found. \nPlease ensure PLU is correct","No product Found");
                    textBox1.Focus();
                    textBox1.SelectAll();
                }
            }
            else
            {
                
            }
        }
    }


    public class Product {
        string PLU;
        string desc;
        float price;
        bool allowFra;

        public Product(string PLU, string desc, float price, bool allowFra)
        {
            this.PLU = PLU;
            this.desc = desc;
            this.price = price;
            this.allowFra = allowFra;

        }
        public Product(string PLU, string desc, float price)
        {
            this.PLU = PLU;
            this.desc = desc;
            this.price = price;
            this.allowFra = false; // false by default

        }

    }

}