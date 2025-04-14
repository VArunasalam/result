using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace Assessment.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    /// <summary>
    /// Login
    /// </summary>
    /// <remarks>
    /// Get the Login details
    /// </remarks>
    /// <response code="200">Login successful</response>
    /// <response code="400">Bad request</response>
    public class LoginController : ControllerBase
    {
        // POST api/login
        [HttpPost(Name = "Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Email and Password are required.");
            }

            var loginPayload = new
            {

                email = request.Email,
                password = request.Password
            };

            using (var client = new HttpClient())
            {
                var loginUri = "https://api.opensensemap.org/users/sign-in";
                var content = new StringContent(JsonConvert.SerializeObject(loginPayload), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(loginUri, content);
                string tokenvalue = "";
                if (!response.IsSuccessStatusCode)
                {
                    if (UserService.logintoken.ContainsKey(request.Email))
                    { tokenvalue = UserService.logintoken[request.Email].ToString(); }
                    else { return Unauthorized("Invalid credentials or error logging in."); }
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<TokenResponse>(responseContent);
                if (token.Token != null && token.Token != "")
                {
                    tokenvalue = token.Token.ToString();
                    if (!UserService.logintoken.ContainsKey(request.Email))
                    { UserService.logintoken.Add(request.Email, tokenvalue); }
                    else
                    {
                        if (UserService.logintoken.ContainsKey(request.Email))
                        { UserService.logintoken[request.Email] = tokenvalue; }
                    }
                }

                UserService.logouttoken = tokenvalue;
                return Ok(responseContent);
            }
        }
    }



    // Request Model

    // Response Model for Token
    public class TokenResponse
    {
        public string Token { get; set; }

    }

    
}
