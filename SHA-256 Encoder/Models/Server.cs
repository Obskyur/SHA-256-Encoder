using SHA_256_Encoder.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SHA_256_Encoder.Models;
internal class Server
{
    private const string base_url = "http://54.186.80.40";
    private State state;

    public Server(State state)
    {
        this.state = state;
    }

    public async Task Authenticate()
    {
        using (HttpClient client = new HttpClient())
        {
            var payload = new
            {
                username = state.Username,
                password = state.Password
            };

            string jsonPayload = JsonSerializer.Serialize(payload);
            StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            await Dialog.ShowAsync("Post Request", $"Payload: {jsonPayload}");

            HttpResponseMessage response = await client.PostAsync($"{base_url}/login", content);

            int statusCode = (int)response.StatusCode;
            string status = response.StatusCode.ToString();
            string message = response.ReasonPhrase;

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<ResponseData>(responseContent);

                state.SessionId = responseData.SessionId;
                state.Score = responseData.Score;
                state.IsAuthenticated = true;

                await Dialog.ShowAsync("Authentication", $"Status: {status}\nMessage: {message}\nSessionID: {state.SessionId}\nScore: {state.Score}");
            }
            else
            {
                await Dialog.ShowAsync($"ERROR: {statusCode}", $"Status: {status}\nMessage: {message}\nFailed to authenticate. Please check your username and password.");
            }
        }
    }

    public async Task Verify()
    {
        using (HttpClient client = new HttpClient())
        {

            var payload = new
            {
                SessionId = state.SessionId,
                Sha256 = Encoder.ComputeSha256Hash($"{state.Username}{state.Password}")
            };

            string jsonPayload = JsonSerializer.Serialize(payload);
            StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{base_url}/verify", content);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<ResponseData>(responseContent);
                state.Score = responseData.Score;
            }
            else
            {
                // Handle error response
            }
        }
    }

    private class ResponseData
    {
        public string SessionId { get; set; }
        public int Score { get; set; }
    }
}
