using HOApi.Interfaces;
using HOApi.Models;
using HOApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HOApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISalesRepo _salesRepo;

        public SalesController(ISalesRepo salesRepo)
        {
            this._salesRepo = salesRepo;
        }

        [HttpGet]
        public async Task<List<sales>> GetSales()
        {
            return await _salesRepo.getAllSales();
        }
    }
}
