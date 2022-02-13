using System.Net.Http;
using System.Threading.Tasks;
using Insurance.DataProvider.Contract;
using Insurance.DataProvider.Model;
using Insurance.Utilities.APIRestClient;
using Microsoft.Extensions.Logging;

namespace Insurance.DataProvider.Service
{
    public class ProductDataApiClient : ApiRestClient<ProductDataApiClient>, IProductDataApiClient
    {
        public ProductDataApiClient(HttpClient httpClient, ILogger<ProductDataApiClient> logger) :base(httpClient, logger)
        {
        }

        public async Task<Product> GetProductById(int productId) =>
            await SendGetRequest<Product>($"products/{productId}");

        public async Task<ProductType> GetProductTypeById(int productTypeId) =>
            await SendGetRequest<ProductType>($"product_types/{productTypeId}");
        
    }
}
