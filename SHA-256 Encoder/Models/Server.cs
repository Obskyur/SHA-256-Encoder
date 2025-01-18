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

            await Dialog.ShowAsync("Authentication Request", $"Payload: {jsonPayload}");

            HttpResponseMessage response = await client.PostAsync($"{base_url}/login", content);

            int statusCode = (int)response.StatusCode;
            string status = response.StatusCode.ToString();
            string message = response.ReasonPhrase;

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<ResponseData>(responseContent);

                message = responseData.message;
                state.SessionId = responseData.session_id;
                state.Score = responseData.score;
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
                session_id = state.SessionId,
                sha256 = Encoder.ComputeSha256Hash($"{state.Username}{state.Password}")
            };

            await Dialog.ShowAsync("Verification Request", $"Payload: {JsonSerializer.Serialize(payload)}");

            string jsonPayload = JsonSerializer.Serialize(payload);
            StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{base_url}/verify", content);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<ResponseData>(responseContent);
                state.Score = responseData.score;
                state.IsVerified = true;
                await Dialog.ShowAsync("Verification", $"Status: {response.StatusCode}\nMessage: {response.ReasonPhrase}\nScore: {state.Score}");
            }
            else
            {
                await Dialog.ShowAsync($"ERROR: {(int)response.StatusCode}", $"Status: {response.StatusCode}\nMessage: {response.ReasonPhrase}\nFailed to verify. Please try again.");
            }
        }
    }

    private class ResponseData
    {
        public string session_id { get; set; }
        public int score { get; set; }

        public string message { get; set; }
    }
}
