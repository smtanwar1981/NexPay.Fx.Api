using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using NexPay.Fx.Api.Common;
using NexPay.Fx.Api.Core;
using NexPay.Fx.Api.Model;
using NexPay.Fx.Api.Service;

namespace NexPay.Fx.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FxController : ControllerBase
    {
        private readonly ILogger<FxController> _logger;
        private readonly IFxService _fxService;
        private readonly ILoginApiProxyService _loginApiProxyService;

        public FxController(ILogger<FxController> logger, IFxService fxService, ILoginApiProxyService loginApiProxyService)
        {
            _logger = logger;
            _fxService = fxService;
            _loginApiProxyService = loginApiProxyService;
            _logger.LogInformation("Intialized SecurityController class");
        }

        [HttpPost("getExchangeRate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetExchangeRateResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetExchangeRateResponse>> GetCurrencyExchangeRate([FromBody] GetExchangeRateRequest request)
        {
            _logger.LogInformation($"Begin executing GetCurrencyExchangeRate() of FxController class.");

            var _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            var isAuthenticated = await _loginApiProxyService.AuthenticateRequest(_bearer_token);

            if (isAuthenticated)
            {
                var response = new GetExchangeRateResponse();

                if (request == null || (string.IsNullOrEmpty(request.CurrencyCodeFrom) && string.IsNullOrEmpty(request.CurrencyCodeTo)))
                {
                    return BadRequest($"Request object is either empty or null - {request}");
                }
                if (string.IsNullOrEmpty(request.CurrencyCodeFrom) && !string.IsNullOrEmpty(request.CurrencyCodeTo))
                {
                    return BadRequest($"No FROM currency code found in the request - {nameof(request.CurrencyCodeFrom)}");
                }
                if (!string.IsNullOrEmpty(request.CurrencyCodeFrom) && string.IsNullOrEmpty(request.CurrencyCodeTo))
                {
                    return BadRequest($"No TO currency code found in the request - {nameof(request.CurrencyCodeTo)}");
                }
                response.ConversionRate = await _fxService.GetCurrencyExchangeRate(request);
                _logger.LogInformation($"Conversion rate of currency FROM {request.CurrencyCodeFrom} to {request.CurrencyCodeTo} is - {response}");

                _logger.LogInformation($"Finish executing GetCurrencyExchangeRate() of FxController class.");
                return Ok(response);
            }
            else
            {
                throw new Exception("Unauthorized access.");
            }
        }
    }
}
