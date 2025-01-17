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

    public async Task Authenticate(string username, string password)
    {
        using (HttpClient client = new HttpClient())
        {
            var payload = new
            {
                Username = username,
                Password = password
            };

            string jsonPayload = JsonSerializer.Serialize(payload);
            StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{base_url}/login", content);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<ResponseData>(responseContent);

                state.SessionId = responseData.SessionId;
                state.Score = responseData.Score;
            }
            else
            {
                // Handle error response
            }
        }
    }

    public async Task Verify(string sessionId, string username, string password)
    {
        using (HttpClient client = new HttpClient())
        {

            var payload = new
            {
                SessionId = sessionId,
                Sha256 = ComputeSha256Hash($"{username}{password}")
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

    private string ComputeSha256Hash(string rawData)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    private class ResponseData
    {
        public string SessionId { get; set; }
        public int Score { get; set; }
    }
}
