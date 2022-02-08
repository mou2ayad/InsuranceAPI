using System.Threading.Tasks;
using Insurance.DataProvider.Model;

namespace Insurance.DataProvider.Contract
{
    public interface IProductDataApiClient
    {
        Task<Product> GetProductById(int productId);

        Task<ProductType> GetProductTypeById(int productTypeId);
    }
}
