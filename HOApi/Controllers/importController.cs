﻿using Microsoft.AspNetCore.Mvc; 
using Newtonsoft.Json;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HOApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class importController : ControllerBase
    {
        private readonly ILogger<importController> _logger;

        public importController(ILogger<importController> logger) =>
            _logger = logger;
        //// GET: api/<importController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<importController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<importController>
        [HttpPost]
        [Route("/api/sales")]
        public void PostSales([FromBody] Models.exportItem value)
        {
            try
            {
                _logger.LogInformation("Got request");
                //Models.exportItem import = JsonConvert.DeserializeObject<Models.exportItem>(value);
                _logger.LogInformation(value.sales.Count.ToString());
                if(value.sales.Count > 0)
                {
                    _logger.LogInformation("Adding sales");
                    Repository.dbWork.addSales(value.sales);
                    _logger.LogInformation("Adding sales complete");
                }
                else { _logger.LogInformation("No sales to add"); }
                if (value.saleline.Count > 0)
                {

                    _logger.LogInformation("Adding salelines");
                    Repository.dbWork.addSaleLines(value.saleline);

                    _logger.LogInformation("Added all new lines");
                }
                else { _logger.LogInformation("No lines to add"); }

            }
            catch(Exception e)
            {
              
                _logger.LogError("Failed " + e.Message);
            }


        }

        //// PUT api/<importController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<importController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
