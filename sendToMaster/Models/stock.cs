using DocumentFormat.OpenXml.Drawing.Charts;
using Newtonsoft.Json;
//using System.Text.Json.Serialization;

namespace sendToMaster.Models
{
    [JsonObject]
    public class stock
    {

        public string PLU;

        public string qty;

        public stock()
        {

        }        
        //public stock(string PLU,string qty)
        //{
        //    this.PLU = PLU;
        //    this.qty = qty;
        //}
        //[JsonConstructor]
        public stock( string PLU, string qty)
        {
            this.PLU = PLU;
            this.qty = qty;
        }




    }
}
