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
    public struct Voucher {
        public string number { get; }
        public double balance { get; }
        public Voucher(string n, double b)
        {
            this.number = n;
            this.balance = b;
        }
        public override string ToString()
        {
            return $"Voucher No: {this.number} . Balance : {this.balance}";
        }
    }


}
