using HOApi.Interfaces;
using HOApi.Models;

namespace HOApi.Repository
{
    public class SalesRepo : ISalesRepo
    {

        public async Task<IReadOnlyList<sales>> getAllSales()
        {
            return dbWork.GetSales();
        }
    }
}
