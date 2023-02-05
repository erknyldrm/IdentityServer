using IdentityModel.Client;
using IdentityServer.Client1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace IdentityServer.Client1.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IConfiguration configuration, ILogger<ProductsController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            HttpClient httpClient = new();

            var discovery = await httpClient.GetDiscoveryDocumentAsync("https://localhost:7059");

            if (discovery.IsError)
            {
                _logger.LogError(discovery.Error);
            }


            ClientCredentialsTokenRequest tokenRequest = new()
            {
                ClientId = _configuration.GetValue<string>("Client:ClientId"),
                ClientSecret = _configuration.GetValue<string>("Client:ClientSecret"),
                Address = discovery.TokenEndpoint
            };

            var token = await httpClient.RequestClientCredentialsTokenAsync(tokenRequest);

            if (token.IsError)
            {
                _logger.LogError(token.Error);
            }

            httpClient.SetBearerToken(token.AccessToken);

            var response = await httpClient.GetAsync("https://localhost:7048/api/ProductGetProducts");

            List<Product> products= new();
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                products = JsonSerializer.Deserialize<List<Product>>(content);
            }
            else
            {
                _logger.LogError($"{response.StatusCode}"); 
            }
            return View(products);
        }
    }
}
