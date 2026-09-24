using System.Net;
using System.Net.Http.Json;
using CustomerOrders.Api.Tests.Helpers;
using CustomerOrders.Business.Entities;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CustomerOrders.Api.Tests;

public class CustomerControllerTests(
    WebApplicationFactory<Program> webApplicationFactory
    ): IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient = webApplicationFactory.CreateClient();
    
    [Fact]
    public async Task GetOneCustomer_WhenCustomerNotExists_ReturnNotFound()
    {
        const int id = 40000;
        var response = await _httpClient.GetAsync($"/customers/{id}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task GetOneCustomer_WhenCustomerExists_ReturnOk()
    {
        int customerId = await HttpHelpers.CreateCustomer(_httpClient, HttpHelpers.GenerateRandomString(8), true);
        var response = await _httpClient.GetAsync($"customers/{customerId}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteCustomer_WhenCustomerExists_ReturnNoContent()
    {
        int customerId = await HttpHelpers.CreateCustomer(_httpClient, HttpHelpers.GenerateRandomString(8), true);

        var response = await _httpClient.DeleteAsync($"/customers/{customerId}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        
    }
    
    [Fact]
    public async Task DeleteCustomer_WhenCustomerNotExists_ReturnNotFound()
    {
        int customerId = await HttpHelpers.CreateCustomer(_httpClient, HttpHelpers.GenerateRandomString(8), true);

        var response = await _httpClient.DeleteAsync($"/Customers/{customerId + 10000}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        
    }
    
    [Fact]
    public async Task UpdateCustomer_WhenCustomerNotExists_ReturnNotFound()
    {
        int customerId = await HttpHelpers.CreateCustomer(_httpClient, HttpHelpers.GenerateRandomString(8), true);

        var response = await HttpHelpers.PatchAsync(_httpClient, $"/customers/{customerId + 10000}", new UpdateCustomer("Welcome", false));
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        
    }
    
    [Fact]
    public async Task UpdateCustomer_WhenCustomerExists_ReturnOk()
    {
        int customerId = await HttpHelpers.CreateCustomer(_httpClient, HttpHelpers.GenerateRandomString(8), true);

        const string newName = "Christ Mael";
        var response = await HttpHelpers.PatchAsync(_httpClient, $"/customers/{customerId}", new UpdateCustomer(newName, false));
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<Customer>();
        Assert.NotNull(body);
        Assert.Equal(newName, body.Name);
        Assert.False(body.IsActive);

    }
    
    

}