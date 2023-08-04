using Newtonsoft.Json;

namespace HOApi.Models
{

    [JsonObject]
    public class Data
    {
        public stock stockItem;
        public Data(stock s)
        {
            this.stockItem = s;
        }
    }

    public struct stock
    {
        public string PLU { get; }
        public string qty { get; }

        //public stock()
        //{

        //}        
        public stock(string PLU,string qty)
        {
            this.PLU = PLU;
            this.qty = qty;
        }

        public override string ToString()
        {
            return this.PLU + "," + this.qty;
        }

    }
}
