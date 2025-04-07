using BDGCodingTask.Controllers.Requests;
using BDGCodingTask.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BDGCodingTask.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderMatchingController : ControllerBase
{
    private readonly ILogger<OrderMatchingController> _logger;
    private readonly IExchangeDataLoaderService _dataLoaderService;
    public OrderMatchingController(ILogger<OrderMatchingController> logger,IExchangeDataLoaderService dataLoaderService)
    {
        _logger = logger;
        _dataLoaderService = dataLoaderService;
    }

    [HttpGet(Name = "GetBestExecutionPlan")]
    public IActionResult Get([FromQuery]OrderRequest request)
    {

        
       return Ok(_dataLoaderService.Exchanges.Count);
    }
}
