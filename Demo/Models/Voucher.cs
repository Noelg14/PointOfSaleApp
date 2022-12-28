using Demo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Models
{
    public class Voucher
    {
        public string Id { get; } // string for leading Zeros and other bits
        public double Balance { get; }

        public  Voucher(string id, double balance) // private. Cannot be created outside this class
        {
            Id = id;
            Balance = balance;
        }

        private Voucher() // private. Cannot be created outside this class
        {
        }

        public static Voucher GetNewVoucher()
        {
            return new Voucher();
        }



        //public static Voucher GetVoucher(string ID)
        //{
        //    DO some stuff to get a voucher
        //    string[] details = VoucherService.getVoucherDetails(ID);
        //    return new Voucher(details[0], Double.Parse(details[1]));

        //}
        //public static Voucher CreateVoucher(double balance)
        //{
        //    // DO some stuff to create a voucher

        //}
        //public static Voucher UseVoucher(string ID)
        //{

        //}

        public override string ToString()
        {
            return $"Voucher : {Id} ; Balance : €{Balance}";
        }
    }
}
