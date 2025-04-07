using BDGCodingTask.Controllers.Requests;
using BDGCodingTask.Controllers.Responses;
using BDGCodingTask.Domain.Entities;
using BDGCodingTask.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BDGCodingTask.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderMatchingController : ControllerBase
{
    private readonly ILogger<OrderMatchingController> _logger;
    private readonly IExchangeDataLoaderService _dataLoaderService;
    private readonly IUserInstructionsService _userInstructionsService;
    public OrderMatchingController(ILogger<OrderMatchingController> logger,IExchangeDataLoaderService dataLoaderService, IUserInstructionsService userInstructionsService)
    {
        _logger = logger;
        _dataLoaderService = dataLoaderService;
        _userInstructionsService = userInstructionsService;
    }

    [HttpGet(Name = "GetBestExecutionPlan")]
    public ActionResult<OrderResponse> Get([FromQuery]OrderRequest request)
    {

        try
        {
            var userInstructions = _userInstructionsService.GetUserInstructions(request.OrderType, request.Amount, _dataLoaderService.Exchanges);

            OrderResponse orderResponse = new OrderResponse
            {
                message = "Order " + userInstructions.Item2.ToString() + " Fullfilled.",
                amountFullfilled = userInstructions.Item1.Sum(x => x.Amount),
                userInstructions = userInstructions.Item1
                
            };

            return Ok(orderResponse);
        }
        catch (Exception ex) {
            
            _logger.LogError(ex.Message);
            _logger.LogTrace(ex.StackTrace);
            return BadRequest(ex.Message);
        }
        
    }
}
