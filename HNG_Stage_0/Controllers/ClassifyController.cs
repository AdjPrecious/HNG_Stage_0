using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HNG_Stage_0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassifyController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public ClassifyController(HttpClient httpClient)
        {

            _httpClient = httpClient;
        }


        [HttpGet]
        public async Task<IActionResult> Classify([FromQuery] string name)
        {
            if (name == null || string.IsNullOrWhiteSpace(name.ToString()))
            {
                return BadRequest(new
                {
                    status = "error",
                    message = "Name query parameter is required"
                });
            }


            if (!name.All(char.IsLetter))
            {
                return UnprocessableEntity(new
                {
                    status = "error",
                    message = "Name must be a string"
                });
            }

            var nameStr = name.ToString();

            try
            {
                var response = await _httpClient.GetAsync($"https://api.genderize.io/?name={nameStr}");

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode(502, new
                    {
                        status = "error",
                        message = "Error fetching data from Genderize API"
                    });
                }

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<GenderizeResponse>(json);

                if (apiResponse.gender == null || apiResponse.count == 0)
                {
                    return StatusCode(500, new
                    {
                        status = "error",
                        message = "No prediction available for the provided name"

                    });
                }

                var isConfident = apiResponse.probability >= 0.7 && apiResponse.count >= 100;

                var result = new
                {
                    status = "success",
                    data = new
                    {
                        name = nameStr,
                        gender = apiResponse.gender,
                        probability = apiResponse.probability,
                        sample_size = apiResponse.count,
                        is_confident = isConfident,
                        processed_at = DateTime.UtcNow.ToString("o")
                    }
                };
                return Ok(result);
            }
            catch
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = "Internal server error"
                });
            }
        }


    }
}
