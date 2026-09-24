using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace CustomerOrders.Api.Tests.Helpers;

public static class HttpHelpers
{
    public static async Task<int> CreateCustomer(HttpClient client, String name, bool isActive, string email = null)
    {
        var createCustomerRes = await client.PostAsJsonAsync("/customers", new CreateCustomer(
            Name: name,
            IsActive: isActive,
            FirstName: GenerateRandomString(6),
            Email: email ?? $"{GenerateRandomString(10)}@gmail.com",
            Address: GenerateRandomString(9)
        ));
        Assert.Equal(HttpStatusCode.Created, createCustomerRes.StatusCode);
        var body = await createCustomerRes.Content.ReadAsStringAsync();
        var rootElement = JsonDocument.Parse(body).RootElement;
        return rootElement.GetProperty("id").GetInt32();

    }
    
    public static String GenerateRandomString(int length)
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
    
    public static async Task<HttpResponseMessage> PatchAsync<T>(HttpClient client, String url, T content)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, url)
        {
            Content = JsonContent.Create(content)
        };
        return await client.SendAsync(request);
    }
}