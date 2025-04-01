using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;

namespace azure_webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IConfiguration _configuration; 
 
    public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration config)
    {
        _logger = logger;
        _configuration = config;
    }

    [HttpGet(Name = "SayHello")]
    public string SayHello()
    {
        return "Hellow azure web api!";
    }

    [HttpGet]
    [Route("FetchKey/{key}")]
    public IActionResult FetchKeyvaultData(string key)
    {
        if (string.IsNullOrEmpty(key)) return BadRequest();
        var data = string.Empty;

        try
        {
            data = _configuration[key];
        }catch(Exception ex)
        {
            _logger.LogError("Keyvault connection failed");
                return Ok("keyvault connection failed");
        }
        return Ok(data);
    }

    [HttpGet]
    [Route("HitLogs/{cutomstring}")]
    public IActionResult HitLogs(string cutomstring)
    {
        if (string.IsNullOrEmpty(cutomstring)) return BadRequest();


        _logger.LogInformation("Learn : LogInformation" + cutomstring);
        _logger.LogError("Learn : LogError" + cutomstring);
        _logger.LogCritical("Learn : LogCritical" + cutomstring);
        _logger.LogWarning("Learn : LogWarning" + cutomstring);

        return Ok();
    }
}
