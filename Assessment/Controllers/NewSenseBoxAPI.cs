using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OAuth2NetCore.Model;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    /// <summary>
    /// Login
    /// </summary>
    /// <remarks>
    /// Create New sensebox
    /// </remarks>
    /// <response code="200">Create successful</response>
    /// <response code="400">Bad request</response>
    public class SenseBoxController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public SenseBoxController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // POST api/sensebox
        [HttpPost(Name = "Newsensebox")]
        public async Task<IActionResult> CreateSenseBox([FromBody] SenseBoxRequest request)
        {
            string bearerToken = "";
            string refreshbearerToken = "";
            if (UserService.logintoken.ContainsKey(request.email))
            { bearerToken = UserService.logintoken[request.email].ToString(); }
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {bearerToken}");

            HttpRequestMessage request3 = new HttpRequestMessage(HttpMethod.Post, "https://api.opensensemap.org/boxes");
            request3.Headers.Add("Authorization", $"Bearer "+ bearerToken);
            if (!User.Identity.IsAuthenticated)
            {
               // return Unauthorized("You need to be signed in to create a sense box.");
            }

            var jsonContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.opensensemap.org/boxes", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return Ok(responseContent); // Return the response from OpenSenseMap API
            }

            return BadRequest("Failed to create the sense box.");
        }
    }

    // Request model to map incoming JSON
    public class SenseBoxRequest
    {
        public string email { get; set; }
        public string name { get; set; }
        public string exposure { get; set; }
        public string model { get; set; }
        public Location location { get; set; }
    }

    // Location model
    public class Location
    {
        public double lat { get; set; }
        public double lng { get; set; }
        public double height { get; set; }
    }
}
