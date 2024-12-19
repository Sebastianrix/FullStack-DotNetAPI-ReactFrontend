using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace cit13_portfolioproject2.Tests
{
    public static class HelperTest
    {
        private static readonly HttpClient client;

        static HelperTest()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5002")
            };
        }

        // Set the Authorization header with the token
        public static void SetAuthorizationHeader(string? token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                client.DefaultRequestHeaders.Authorization = null;
            }
        }


        /// Deserializes the response content to the specified type.
        public static async Task<T?> DeserializeData<T>(HttpResponseMessage response) where T : class
        {
            var data = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(data))
            {
                return null;
            }

            return JsonSerializer.Deserialize<T>(data);

        }

        /// Creates a JSON StringContent for HTTP requests.
        public static StringContent CreateJsonContent(object content) =>
            new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");

        /// Sends a GET request and returns a JSON array.
        public static async Task<(JsonArray?, HttpStatusCode)> GetArray(string url)
        {
            var response = await client.GetAsync(url);
            var statusCode = response.StatusCode;

            var jsonString = await response.Content.ReadAsStringAsync();
            try
            {
                var jsonNode = JsonNode.Parse(jsonString);
                if (jsonNode is JsonObject jsonObject && jsonObject["items"] is JsonArray jsonArray)
                {
                    return (jsonArray, statusCode);
                }
            }
            catch (JsonException)
            {
                // Handle non-JSON response
            }

            return (null, statusCode);
        }

        /// Sends a GET request and returns a JSON object.
        public static async Task<(JsonObject?, HttpStatusCode)> GetObject(string url)
        {
            var response = await client.GetAsync(url);
            return (await DeserializeData<JsonObject>(response), response.StatusCode);
        }

        /// Sends a POST request with JSON content and returns a JSON object.
        public static async Task<(JsonObject?, HttpStatusCode)> PostData(string url, object content)
        {
            var response = await client.PostAsync(url, CreateJsonContent(content));
            return (await DeserializeData<JsonObject>(response), response.StatusCode);
        }

        /// Sends a PUT request with JSON content and returns the status code.
        public static async Task<(JsonNode?, HttpStatusCode)> PutData(string url, object content)
        {
            var response = await client.PutAsync(url, CreateJsonContent(content));
            JsonNode? responseContent = null;

            if (response.Content.Headers.ContentType?.MediaType == "application/json")
            {
                var contentString = await response.Content.ReadAsStringAsync();
                responseContent = JsonNode.Parse(contentString);
            }

            return (responseContent, response.StatusCode);

        }
        public static async Task<HttpStatusCode> PutDataStatusOnly(string url, object content)
        {
            var response = await client.PutAsync(url, CreateJsonContent(content));
            return response.StatusCode;
        }


        /// Sends a DELETE request and returns the status code.
        public static async Task<HttpStatusCode> DeleteData(string url)
        {
            var response = await client.DeleteAsync(url);
            return response.StatusCode;
        }

        /// Safely extracts a string value from a JsonNode.
        public static string? Value(this JsonNode node, string name) =>
            node[name]?.ToString();

        /// Safely extracts an integer value from a JsonNode.
        public static int? ValueInt(this JsonNode node, string name) =>
            int.TryParse(node[name]?.ToString(), out var result) ? result : null;
    }
}