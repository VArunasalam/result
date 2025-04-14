using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using static SyncSoft.App.CONSTANTS;

namespace Assessment.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    /// <summary>
    /// New User Registration
    /// </summary>
    /// <remarks>
    /// Create New User
    /// </remarks>
    /// <response code="200">Create Usersuccessful</response>
    /// <response code="400">Bad request</response>
    public class RegistrationController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        public RegistrationController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        [HttpPost(Name = "Registration")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Name, Email, and Password are mandatory fields.");
            }

            // Create the request body for OpenSenseMap API
            var requestBody = new
            {
                name = request.Name,
                email = request.Email,
                password = request.Password,

            };

            // Convert the request body to JSON
            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            // Send the request to OpenSenseMap API
            var response = await _httpClient.PostAsync("https://api.opensensemap.org/users/register", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }

            var result = await response.Content.ReadAsStringAsync();
            var _token = JsonConvert.DeserializeObject<TokenResponse>(result);
            string _refreshtiken = Guid.NewGuid().ToString();
            var result1 = new
            {
                Code = "Created",
                message = "Successfullly registred new user",
                token =  _token,
                refreshtiken = _refreshtiken,
                data = "{}",
            };
            if (!UserService.logintoken.ContainsKey(request.Email))
            { UserService.logintoken.Add(request.Email, _token.Token.ToString()); }
            else
            {
                if (UserService.logintoken.ContainsKey(request.Email))
                { UserService.logintoken[request.Email] = _token.Token.ToString(); }
            }
            return Ok(result1);
        }
    }

    public class LoginRequest
    {
      
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterUserRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
