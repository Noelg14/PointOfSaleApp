using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOApi.Models
{
    public class exportItem
    {
        public List<sales> sales;
        public List<salesline> saleline;

        public exportItem(List<sales> s, List<salesline> sl)
        {
            this.sales = s;
            this.saleline = sl;
        }
        public exportItem()
        {
        }

    }
    public class StockExp
    {
        public List<stock> stock;

        public StockExp(List<stock> s)
        {
            stock = stock;
        }
        public StockExp()
        {
        }

    }
}
