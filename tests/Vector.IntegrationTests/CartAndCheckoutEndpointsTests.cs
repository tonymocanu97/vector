using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Vector.Application.DTOs;

namespace Vector.IntegrationTests
{
    [Collection(ApiTestCollection.Name)]
    public class CartAndCheckoutEndpointsTests(ApiWebApplicationFactory factory)
    {
        private readonly HttpClient _client = factory.CreateClient();

        private async Task AuthenticateAsync()
        {
            var token = await TestDataHelper.RegisterAndLoginAsync(_client);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        [Fact]
        public async Task GetCart_WithoutAuth_ReturnsUnauthorized()
        {
            var response = await _client.GetAsync("/api/cart");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task AddItem_ThenGetCart_ReturnsItemInCart()
        {
            await AuthenticateAsync();

            var addResponse = await _client.PostAsJsonAsync("/api/cart/items", new AddCartItemRequest(1, 2));
            addResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var cart = await _client.GetFromJsonAsync<CartDto>("/api/cart");

            cart!.Items.Should().ContainSingle(i => i.ProductId == 1 && i.Quantity == 2);
        }

        [Fact]
        public async Task AddItem_ExceedsAvailableStock_ReturnsBadRequest()
        {
            await AuthenticateAsync();

            var response = await _client.PostAsJsonAsync("/api/cart/items", new AddCartItemRequest(1, 1_000_000));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RemoveItem_AfterAdding_ClearsCart()
        {
            await AuthenticateAsync();
            await _client.PostAsJsonAsync("/api/cart/items", new AddCartItemRequest(2, 1));

            var response = await _client.DeleteAsync("/api/cart/items/2");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var cart = await response.Content.ReadFromJsonAsync<CartDto>();
            cart!.Items.Should().BeEmpty();
        }

        [Fact]
        public async Task Checkout_EmptyCart_ReturnsBadRequest()
        {
            await AuthenticateAsync();
            var request = new CheckoutRequest("Jane Doe", "123 Main St", "Metropolis", "12345", "USA");

            var response = await _client.PostAsJsonAsync("/api/orders/checkout", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Checkout_ValidCart_CreatesOrderAndClearsCart()
        {
            await AuthenticateAsync();
            await _client.PostAsJsonAsync("/api/cart/items", new AddCartItemRequest(3, 2));
            var checkoutRequest = new CheckoutRequest("Jane Doe", "123 Main St", "Metropolis", "12345", "USA");

            var checkoutResponse = await _client.PostAsJsonAsync("/api/orders/checkout", checkoutRequest);

            checkoutResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            var order = await checkoutResponse.Content.ReadFromJsonAsync<OrderDto>();
            order!.Status.Should().Be("Paid");
            order.Items.Should().ContainSingle(i => i.ProductId == 3 && i.Quantity == 2);

            var cart = await _client.GetFromJsonAsync<CartDto>("/api/cart");
            cart!.Items.Should().BeEmpty();

            var orderHistory = await _client.GetFromJsonAsync<List<OrderDto>>("/api/orders");
            orderHistory.Should().ContainSingle(o => o.Id == order.Id);
        }
    }
}
