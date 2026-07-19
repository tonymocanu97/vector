using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Vector.Application.DTOs;

namespace Vector.IntegrationTests
{
    [Collection(ApiTestCollection.Name)]
    public class ProductsEndpointsTests(ApiWebApplicationFactory factory)
    {
        private readonly ApiWebApplicationFactory _factory = factory;
        private readonly HttpClient _client = factory.CreateClient();

        [Fact]
        public async Task GetAll_ReturnsSeededProducts()
        {
            var products = await _client.GetFromJsonAsync<List<ProductDto>>("/api/products");

            products.Should().NotBeNull();
            products!.Should().Contain(p => p.Id == 1 && p.Name == "Pro Kit Jersey 2026");
        }

        [Fact]
        public async Task GetAll_FilteredByCategory_ReturnsOnlyMatchingProducts()
        {
            var products = await _client.GetFromJsonAsync<List<ProductDto>>("/api/products?category=apparel");

            products.Should().NotBeNull();
            products!.Should().NotBeEmpty();
            products.Should().OnlyContain(p => p.CategorySlug == "apparel");
        }

        [Fact]
        public async Task GetById_NotFound_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/products/99999");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetById_Found_ReturnsProduct()
        {
            var response = await _client.GetAsync("/api/products/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var product = await response.Content.ReadFromJsonAsync<ProductDto>();
            product!.Id.Should().Be(1);
        }

        [Fact]
        public async Task Create_WithoutAuth_ReturnsUnauthorized()
        {
            var request = new CreateProductRequest("Test Product", "Description", 9.99m, "/images/test.jpg", 10, null, 1);

            var response = await _client.PostAsJsonAsync("/api/products", request);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Create_AsCustomer_ReturnsForbidden()
        {
            var token = await TestDataHelper.RegisterAndLoginAsync(_client);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new CreateProductRequest("Test Product", "Description", 9.99m, "/images/test.jpg", 10, null, 1);

            var response = await _client.PostAsJsonAsync("/api/products", request);

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Create_AsAdmin_ReturnsCreatedProduct()
        {
            var token = await TestDataHelper.CreateAdminAndLoginAsync(_client, _factory);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new CreateProductRequest("New Test Product", "Description", 19.99m, "/images/test.jpg", 5, "New", 1);

            var response = await _client.PostAsJsonAsync("/api/products", request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var created = await response.Content.ReadFromJsonAsync<ProductDto>();
            created!.Name.Should().Be("New Test Product");
            created.Tag.Should().Be("New");
        }

        [Fact]
        public async Task Create_AsAdminWithInvalidCategory_ReturnsBadRequest()
        {
            var token = await TestDataHelper.CreateAdminAndLoginAsync(_client, _factory);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var request = new CreateProductRequest("Test Product", "Description", 9.99m, "/images/test.jpg", 10, null, 99999);

            var response = await _client.PostAsJsonAsync("/api/products", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Delete_AsAdmin_RemovesProduct()
        {
            var token = await TestDataHelper.CreateAdminAndLoginAsync(_client, _factory);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var createRequest = new CreateProductRequest("Disposable Product", "Description", 9.99m, "/images/test.jpg", 10, null, 1);
            var createResponse = await _client.PostAsJsonAsync("/api/products", createRequest);
            var created = await createResponse.Content.ReadFromJsonAsync<ProductDto>();

            var deleteResponse = await _client.DeleteAsync($"/api/products/{created!.Id}");
            var getResponse = await _client.GetAsync($"/api/products/{created.Id}");

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
