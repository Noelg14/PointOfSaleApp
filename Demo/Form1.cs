using MySql.Data.MySqlClient;
using System.Timers;


namespace Demo
{
    public partial class Form1 : Form
    {
        double total=0;
        Cart c = new Cart();
        bool paid;
        static Form1 thisForm; // probably terrible :(

        public Form1()
        { 
            InitializeComponent();
            Rectangle r = Screen.FromControl(this).Bounds;
            button1.Left = (r.Width - (r.Width)/4);
            button1.Top= (r.Height - (r.Height)/4);
            button2.Left = (r.Width - (r.Width) / 4);
            button2.Top = (r.Height - (r.Height) / 4) -100;

            panel1.Height= (r.Height - 200);
            textBox1.Width=panel1.Width;
            label5.Text = "€ " + total;
            Utils.log("Init Form1");
            thisForm = this;

        }
        //public Form1(Cart c)
        //{
        //    InitializeComponent();
        //    Rectangle r = Screen.FromControl(this).Bounds;
        //    button1.Left = (r.Width - (r.Width) / 4);
        //    button1.Top = (r.Height - (r.Height) / 4);
        //    button2.Left = (r.Width - (r.Width) / 4);
        //    button2.Top = (r.Height - (r.Height) / 4) - 100;

        //    panel1.Height = (r.Height - 200);
        //    textBox1.Width = panel1.Width;
        //    label5.Text = "€ " + total;
        //    Utils.log("Init Form1 w/ Products");

        //    foreach(Product p in c.products)
        //    {

        //        label1.Text += "\n " + p.PLU;
        //        label3.Text += "\n " + p.desc;
        //        label2.Text += "\n €" + p.price;
        //        total = Math.Round(total += p.price, 2);
        //        label5.Text = "€ " + total;
        //    }

        //}

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (c.products.Count != 0)
            {
               
                Utils.recSale(c, total);
                Utils.log("Button Pay Now Pressed");

                bool paid = Payment.newPayment(total, c);
                //this.Hide();
                //if (paid)
                //{
                //    clear();
                //}
                this.textBox1.Enabled = false;
                //this.Show();
                //MessageBox.Show("Paid €" + total,"Paid");
                //clear();
                //this.Hide();
                //return;

            }
            if (c.products.Count == 0)
            {
                MessageBox.Show("Please add items before Paying", "No Items Added");
                return;
            }


        }
        /*
         * Need to refactor the below as currenlty does not allow voids. 
         */
        private void textBox1_KeyUp(object sender, KeyEventArgs e) 
        {
            
            if (e.KeyCode == Keys.Enter)
            {
                
                if (textBox1.Text == "ADMIN")
                {

                    new AddProd().Show();
                    //AddProd a = new AddProd();
                    //a.Show();
                    //textBox1.Enabled = false;
                    return;
                }
                Product p = Utils.search(textBox1.Text.ToString());
                if (p != null)
                {
                    c.AddProd(p);

                    //dataGridView1.DataSource = c.products;
                    //dataGridView1.Refresh();

                    label1.Text += "\n " + p.PLU;
                    label3.Text += "\n " + p.desc;
                    label2.Text += "\n €" + p.price;
                    total=Math.Round(total += p.price,2);
                    label5.Text = "€ " + total;

                    textBox1.Focus();
                    textBox1.SelectAll();

                }
                if (p is null)
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

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void clear()
        {
            total = 0;
            label2.Text = "Price";
            label1.Text = "PLU";
            label3.Text = "Desc";
            label5.Text = "€" + total ;
            c.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clear();
            textBox1.Focus();
        }

        public static void notify()
        {            

            Form1 f = Form1.thisForm;
            f.clear();
            f.textBox1.Enabled = true;
            f = null;
            Utils.log("Clear form");
        }
        public static void notifyBack()
        {

            Form1 f = Form1.thisForm;
            f.textBox1.Enabled = true;
            f = null;
            Utils.log("Back - enabling textbox");
        }
    }

    public class Product {
        public string PLU { get; }
        public string desc { get; }
        public float price { get; }
        public bool allowFra { get; }

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
        public Product()
        {

        }

    }

}