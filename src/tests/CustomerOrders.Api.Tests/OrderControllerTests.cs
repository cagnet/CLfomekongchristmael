using System.Net;
using System.Net.Http.Json;
using CustomerOrders.Business.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CustomerOrders.Api.Tests;

public class OrderControllerTests(WebApplicationFactory<Program> webApplicationFactory)
    : IClassFixture<WebApplicationFactory<Program>>
{

    private HttpClient _httpClient = webApplicationFactory.CreateClient();


    [Fact]
    public async Task CreateOrder_WhenCustomerNotExists_Return400BadRequest()
    {
        var response = await _httpClient.PostAsJsonAsync("/customers/1/orders", new
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
        var createCustomerRes = await _httpClient.PostAsJsonAsync("/customers", new
        {
            Name = "Mael",
            IsActive = false
        });
     
        Assert.Equal(HttpStatusCode.Created, createCustomerRes.StatusCode);
        
        
        var response = await _httpClient.PostAsJsonAsync("/customers/1/orders", new
        {
            Amount = 10
        });
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<Dictionary<String, String>>();
        Assert.NotNull(body);
        
        Assert.Equal(BusinessRuleCodes.InactiveCustomer, body["code"]);
    }
}