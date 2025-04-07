using BDGCodingTask.Controllers.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BDGCodingTask.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderMatchingController : ControllerBase
{
    private readonly ILogger<OrderMatchingController> _logger;
    public OrderMatchingController(ILogger<OrderMatchingController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetBestExecutionPlan")]
    public IActionResult Get([FromQuery]OrderRequest request)
    {
       return Ok("Test");
    }
}
