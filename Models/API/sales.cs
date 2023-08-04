using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOApi.Models
{
    public struct sales
    {
        public string cartID;
        public string value;
        public string date;
        public sales(string cartID,double value,string date)
        {
            this.cartID = cartID;
            this.value = value.ToString();
            this.date = date;
        }

    }

    public struct salesline {
        public string PLU;
        public string value;
        public string cartID;

        public salesline(string plu,double value,string cart)
        {
            this.PLU = plu;
            this.value = value.ToString();
            this.cartID = cart;

        }
    
    }

}
