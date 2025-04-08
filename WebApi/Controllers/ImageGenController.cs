using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using DataLayer;
using Mapster;
using System;
using DataLayer.Models;

namespace WebApi.Controllers
{

[ApiController]
[Route("api/[controller]")]
public class ImageGenController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public ImageGenController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateImage([FromBody] GenerateRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("http://localhost:5000/generate", request);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<GenerateResponse>();
            return Ok(result);
        }

        return StatusCode(500, "Failed to generate image");
    }
}

public class GenerateRequest
{
    public int Seed { get; set; }
}

public class GenerateResponse
{
    public string Image { get; set; } // base64-encoded PNG
}

}