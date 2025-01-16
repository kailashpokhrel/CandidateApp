using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

public class ApiTests
{
    private readonly HttpClient _client;

    public ApiTests()
    {
        _client = new HttpClient
        {
            BaseAddress = new System.Uri("http://localhost:8080")
        };
    }

    [Fact]
    public async Task Test_GetCandidates_ReturnsOk()
    {
        // Arrange
        var request = "/api/candidates"; // Update with your actual API endpoint
        
        // Act
        var response = await _client.GetAsync(request);
        
        // Assert
        response.EnsureSuccessStatusCode();
    }
}
