using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class Cart
    {
        public List<Product> products = new List<Product>();
        //public Dictionary<Product,double> prod = new Dic<>();  Product / QTY?

        public long id;
        public Cart(List<Product> prods)
        {
            this.products = prods;
            this.id = DateTime.Now.Ticks;
        }
        public Cart()
        {
            this.id = DateTime.Now.Ticks;
        }

        public void AddProd(Product p)
        {
            products.Add(p);
        }
        public void removeProd(Product p)
        {
            products.Remove(p);
        }
        public void Clear()
        {
            products.Clear();
        }

    }
}
