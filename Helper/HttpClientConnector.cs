using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class HttpClientConnector
{
    private readonly HttpClient _httpClient;

    public HttpClientConnector(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T?> GetAsync<T>(string baseUrl, Dictionary<string, string>? queryParams = null)
    {
        string url = baseUrl;

        if (queryParams != null && queryParams.Count > 0)
        {
            var query = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
            url += "?" + query;
        }

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json);
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data)
    {
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TResponse>(responseJson);
    }
    public async Task<TResponse?> PostAsync<TResponse>(string baseUrl, Dictionary<string, string> queryParams)
    {
        string url = baseUrl;

        if (queryParams != null && queryParams.Count > 0)
        {
            var query = string.Join("&", queryParams
    .Where(kvp => kvp.Value != null) 
    .Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));

            url += "?" + query;
        }

        var response = await _httpClient.PostAsync(url, null); 
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TResponse>(responseJson);
    }

}