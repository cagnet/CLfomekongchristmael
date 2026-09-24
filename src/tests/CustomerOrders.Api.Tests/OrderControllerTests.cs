using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CustomerOrders.Business.Entities;
using CustomerOrders.Business.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CustomerOrders.Api.Tests;

public class OrderControllerTests(WebApplicationFactory<Program> webApplicationFactory)
    : IClassFixture<WebApplicationFactory<Program>>
{

    private HttpClient _httpClient = webApplicationFactory.CreateClient();


    [Fact]
    public async Task GetOneOrder_WhenOrderNotExists_ReturnNotFound()
    {
        const int id = 40000;
        var response = await _httpClient.GetAsync($"orders/{id}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task GetOneOrder_WhenOrderExists_ReturnOk()
    {
        int customerId = await CreateCustomer(GenerateRandomString(8), true);
        Order order = await CreateOrder(customerId, 6000);
        var response = await _httpClient.GetAsync($"orders/{order.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task CreateOrder_WhenCustomerNotExists_Return400BadRequest()
    {
        var response = await _httpClient.PostAsJsonAsync("/customers/1000/orders", new
        {
            Amount = 10
        });
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<Dictionary<String, String>>();
        Assert.NotNull(body);
        
        Assert.Equal(BusinessRuleCodes.CustomerNotFound, body["code"]);
    }

    [Fact]
    public async Task CreateOrder_WhenAmountIsNegative_Return400BadRequest()
    {
        var response = await _httpClient.PostAsJsonAsync("/customers/1/orders", new
        {
            Amount = -4
        });
     
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.NotNull(body);
        Assert.Contains("\"errors\"", body);
        Assert.Contains("\"Amount\":", body);
    }
    
    [Fact]
    public async Task CreateOrder_WhenCustomerIsInative_Return400BadRequest()
    {
        // Let avoid mocking data for the moment
        int id = await CreateCustomer("Mael", false);
        
        
        var response = await _httpClient.PostAsJsonAsync($"/customers/{id}/orders", new
        {
            Amount = 10
        });
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<Dictionary<String, String>>();
        Assert.NotNull(body);
        
        Assert.Equal(BusinessRuleCodes.InactiveCustomer, body["code"]);
    }

    [Fact]
    public async Task CreateOrder_WhenCustomerIsActiveAndRequestIsValid_Return201Created()
    {
        int id = await CreateCustomer("Christ", true);
        var response = await _httpClient.PostAsJsonAsync($"/customers/{id}/orders", new
        {
            Amount = 10
        });
        
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task DeleteOrder_WhenOrderExists_ReturnNoContent()
    {
        int customerId = await CreateCustomer(GenerateRandomString(8), true);
        Order order = await CreateOrder(customerId, 100);

        var response = await _httpClient.DeleteAsync($"/orders/{order.Id}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        
    }
    
    [Fact]
    public async Task DeleteOrder_WhenOrderNotExists_ReturnNotFound()
    {
        int customerId = await CreateCustomer(GenerateRandomString(8), true);
        Order order = await CreateOrder(customerId, 100);

        var response = await _httpClient.DeleteAsync($"/orders/{order.Id + 10000}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        
    }
    
    [Fact]
    public async Task UpdateOrder_WhenOrderNotExists_ReturnNotFound()
    {
        int customerId = await CreateCustomer(GenerateRandomString(8), true);
        Order order = await CreateOrder(customerId, 100);

        var response = await PatchAsync(_httpClient, $"/orders/{order.Id + 10000}", new UpdateOrder(Amount: 200));
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        
    }
    
    [Fact]
    public async Task UpdateOrder_WhenOrderExists_ReturnOk()
    {
        int customerId = await CreateCustomer(GenerateRandomString(8), true);
        Order order = await CreateOrder(customerId, 100);

        const int newAmount = 200;
        var response = await PatchAsync(_httpClient, $"/orders/{order.Id}", new UpdateOrder(Amount: newAmount));
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<Order>();
        Assert.NotNull(body);
        Assert.Equal(newAmount, body.Amount);

    }


    private async Task<int> CreateCustomer(String name, bool isActive)
    {
        var createCustomerRes = await _httpClient.PostAsJsonAsync("/customers", new CreateCustomer(
                Name: name,
                IsActive: isActive,
                FirstName: GenerateRandomString(6),
                Email: $"{GenerateRandomString(10)}@gmail.com",
                Address: GenerateRandomString(9)
            ));
        Assert.Equal(HttpStatusCode.Created, createCustomerRes.StatusCode);
        var body = await createCustomerRes.Content.ReadAsStringAsync();
        var rootElement = JsonDocument.Parse(body).RootElement;
        return rootElement.GetProperty("id").GetInt32();

    }

    private async Task<Order> CreateOrder(int customerId, decimal amount)
    {
        var response = await _httpClient.PostAsJsonAsync($"/customers/{customerId}/orders", new
        {
            Amount = 10
        });
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Order? order =  await response.Content.ReadFromJsonAsync<Order>();
        Assert.NotNull(order);
        return order;
    }

    private String GenerateRandomString(int length)
    {
        string alphabet = "qwertyuiopasdfghjklzxcvbnm";
        var words = new char[length];
        var random = new Random();

        for (int i = 0; i < length; i++)
        {
            words[i] = alphabet[random.Next(alphabet.Length)];
        }

        return new string(words);
    }

    private async Task<HttpResponseMessage> PatchAsync<T>(HttpClient client, String url, T content)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, url)
        {
            Content = JsonContent.Create(content)
        };
        return await client.SendAsync(request);
    }
}