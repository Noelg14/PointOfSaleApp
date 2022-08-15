using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sendToMaster.Models
{
    internal class exportItem
    {
        public List<sales> sales;
        public List<salesline> saleline;

        public exportItem(List<sales> s, List<salesline> sl)
        {
            sales = s;
            saleline = sl;
        }
        public exportItem()
        {
        }

    }
}
