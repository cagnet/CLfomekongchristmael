using Microsoft.AspNetCore.Mvc.Testing;

namespace CustomerOrders.Api.Tests;

public class CustomerControllerTests(
    WebApplicationFactory<Program> webApplicationFactory
    ): IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient = webApplicationFactory.CreateClient();

}