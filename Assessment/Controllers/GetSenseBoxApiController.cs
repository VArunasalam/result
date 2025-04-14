using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;

namespace Assessment.Controllers
{
    /// <summary>
    /// Get GetSenseBoxById
    /// </summary>
    /// <remarks>
    /// Get the SenseBox details by Id
    /// </remarks>
    /// <response code="200">GetSenseBoxById successful</response>
    /// <response code="400">Bad request</response>
    [Route("/[controller]")]
    [ApiController]
   
    public class GetSenseBoxApiController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public GetSenseBoxApiController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET api/sensebox/{id}
        [HttpGet(Name = "GetSenseBoxById/{senseBoxId}")]
        public async Task<IActionResult> GetSenseBoxById( string senseBoxId)
        {
            var url = $"https://api.opensensemap.org/boxes/{senseBoxId}";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return Ok(data);  // Returns the box data from OpenSenseMap
                }
                else
                {
                    return NotFound($"Sense box with ID {senseBoxId} not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
