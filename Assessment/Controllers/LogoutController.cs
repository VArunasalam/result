using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SyncSoft.App.Messaging;
using System.Net.Http.Headers;
using System.Text;

namespace Assessment.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class LogoutController : ControllerBase
    {
       
        /// <summary>
        /// Logout user by clearing authentication token from cache
        /// </summary>
        /// <remarks>
        /// This endpoint removes the authentication token from the cache, effectively logging out the user.
        /// </remarks>
        /// <response code="200">Logout successful</response>
        /// <response code="400">Bad request</response>

        [HttpPost(Name = "Sign Out")]
        public async Task<IActionResult> SignOut()
        {
            var apiUrl = "https://api.opensensemap.org/users/sign-out";
            string token = UserService.logouttoken;
            var data = new
            {
                userId = "user123"
            };
            // Create an HttpClient instance
            using (var client = new HttpClient())
            {
                // Set Authorization header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                // Convert data to JSON
                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                try
                {
                    // Sending POST request with body
                    var response = await client.PostAsync(apiUrl, content);

                    // Check response status
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Successfully signed out.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to sign out: {response.StatusCode}");
                        return Ok("Failed to sign out");
                    }
                    return Ok("Successfully signed out");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                return BadRequest("Token not found.");
            }

        }
    }
}
