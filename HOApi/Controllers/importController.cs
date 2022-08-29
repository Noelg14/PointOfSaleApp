using Microsoft.AspNetCore.Mvc; 
using Newtonsoft.Json;
using System.IO;


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
        
        
        [HttpGet]
        [Route("/api/hello")]
        public string getHello()
        {
            return "There";

        }

        [HttpPost]
        [Route("/api/stock")]
        public void PostStock([FromBody] Models.Data root)
        {
            //List<Models.stock> import = imp;

            try
            {
                _logger.LogInformation("Got request : PostStock");
                System.IO.File.WriteAllText("Web.txt",root.ToString());
                //Models.exportItem import = JsonConvert.DeserializeObject<Models.exportItem>(imp);
                //_logger.LogInformation(import.Count.ToString());
                //if (import.Count > 0)
                //{
                    _logger.LogInformation("Adding stock");
                    //Repository.dbWork.addStock(import);
                    _logger.LogInformation("Adding stock complete");
                //}
               // else { _logger.LogInformation("No stock to add"); }

            }
            catch (Exception e)
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
