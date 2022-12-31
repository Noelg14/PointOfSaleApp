using Microsoft.AspNetCore.Mvc;
using HOApi.Models;
using HOApi.Repository;
using Newtonsoft.Json;
using System.Net.NetworkInformation;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HOApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        //// GET: api/<VoucherController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<VoucherController>/5
        [HttpGet("{id}")]
        public string GetBalance(string id)
        {
            Voucher v = VoucherRepo.getVoucher(id);
            return JsonConvert.SerializeObject(v);
        }

        // POST api/<VoucherController>
        [HttpPost("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UpdateBalance(string id,double newBal)
        {
            Voucher v = VoucherRepo.updateBalance(id,newBal);
            if (v.Id != id)
            {
                // if voucher does not exist, return bad request
                return StatusCode(400,"Voucher does not exist");
            }
            else
            {
                //all good return
                return StatusCode(200,v);
                //return JsonConvert.SerializeObject(v);
            }
        }

        //// PUT api/<VoucherController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<VoucherController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
