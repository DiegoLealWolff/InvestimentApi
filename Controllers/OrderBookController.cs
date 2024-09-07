using Azure.Core;
using InvestimentApi.Models;
using InvestimentApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderBookController : ControllerBase
    {
        private readonly IOrderBookService _orderBookService;

        public OrderBookController(IOrderBookService orderBookService)
        {
            _orderBookService = orderBookService;
        }

        [HttpGet("CalculateBestOrder")]
        public async Task<IActionResult> GetCalculateBestOrder([FromQuery] string asset, [FromQuery] OrderType orderType, [FromQuery] decimal quantity)
        {
            if (string.IsNullOrEmpty(asset) || quantity <= 0)
            {
                return NoContent();
            }

            try
            {                          
                var result = await _orderBookService.GetBestOrderAsync(asset, orderType, quantity);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }
    }
}
