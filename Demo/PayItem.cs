using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    internal class PayItem
    {
        string type { get; }
        double value { get; }

        public PayItem(string type, double value)
        {
            this.type = type;
            this.value = value;
        }


    }
}
