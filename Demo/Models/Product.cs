using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class Product
    {
        public string PLU { get; }
        public string desc { get; }
        public float price { get; set; }
        public bool allowFra { get; }
        public double qty { get; set; } = 0;
        public char type { get; } = 'N';
        public string sID { get; set; } = "";

        public Product(string PLU, string desc, float price, bool allowFra, char type)
        {
            this.PLU = PLU;
            this.desc = desc;
            this.price = price;
            this.allowFra = allowFra;
            this.type = type;

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

        public override bool Equals(object comp)
        {
            if (comp == null) { return false; }

            Product compare = (Product)comp;

            if (this.price == compare.price && this.PLU == compare.PLU && this.qty == compare.qty && this.desc == compare.desc)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return Tuple.Create(this.PLU, this.desc).GetHashCode();
        }

    }

}
