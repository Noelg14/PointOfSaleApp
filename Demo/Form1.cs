using MySql.Data.MySqlClient;


namespace Demo
{
    public partial class Form1 : Form
    {
        double total=0;
        Cart c = new Cart();
        bool paid;
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
            
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            if(c.products.Count!=0)
            {
                bool paid = false;
                Utils.recSale(c, total);
                Console.WriteLine("Button Pay Now Pressed");
                Payment p = new Payment(total, c);
                p.Show();
                //MessageBox.Show("Paid €" + total,"Paid");
                if (p.IsDisposed)
                {
                    clear();
                    return;
                }
            }
            if(total ==0 && c.products.Count ==0)
            {
                MessageBox.Show("Please add items before Paying", "No Items Added");
                return;
            }


        }
        public void isPaid(bool p)
        {
            this.paid = p;
        }


        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if(textBox1.Text == "ADMIN")
                {
                    AddProd a = new AddProd();
                    a.Show();
                    //textBox1.Enabled = false;
                    return;
                }
                Product p = Utils.search(textBox1.Text.ToString());
                if (p != null)
                {
                    c.AddProd(p);
                    label1.Text += "\n " + p.PLU;
                    label3.Text += "\n " + p.desc;
                    label2.Text += "\n €" + p.price;
                    total += p.price;
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
            label2.Text = "PLU";
            label1.Text = "Price";
            label3.Text = "Desc";
            label5.Text = "€" + total ;
            c.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clear();
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