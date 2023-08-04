using HOApi.Models;

namespace HOApi.Interfaces
{
    public interface ISalesRepo
    {
        public Task<IReadOnlyList<sales>> getAllSales();
    }
}
