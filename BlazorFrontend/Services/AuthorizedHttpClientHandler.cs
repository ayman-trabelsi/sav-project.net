using System.Net.Http.Headers;
using Microsoft.JSInterop;

public class AuthorizedHttpClientHandler : DelegatingHandler
{
    private readonly IJSRuntime _jsRuntime;
    private const string TokenKey = "authToken";

    public AuthorizedHttpClientHandler(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TokenKey);
            if (!string.IsNullOrEmpty(token))
            {
                // Log token presence
                Console.WriteLine($"Token found in localStorage: {token.Substring(0, 20)}...");

                // Ensure the token is properly formatted
                if (!token.StartsWith("Bearer "))
                {
                    token = $"Bearer {token}";
                }
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));
                
                // Log the request details
                Console.WriteLine($"Sending request to: {request.RequestUri}");
                Console.WriteLine($"Authorization header: {request.Headers.Authorization}");
            }
            else
            {
                Console.WriteLine("No token found in localStorage");
            }

            var response = await base.SendAsync(request, cancellationToken);

            // Log response details
            Console.WriteLine($"Response status: {response.StatusCode}");
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("Received 401 Unauthorized - clearing token");
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
            }

            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in AuthorizedHttpClientHandler: {ex.Message}");
            return await base.SendAsync(request, cancellationToken);
        }
    }
}