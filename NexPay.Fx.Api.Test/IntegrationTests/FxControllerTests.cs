using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Moq;
using NexPay.Fx.Api.Common;
using NexPay.Fx.Api.Controllers;
using NexPay.Fx.Api.Model;
using NexPay.Fx.Api.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NexPay.Fx.Api.Test.IntegrationTests
{
    public class FxControllerTests
    {
        private readonly Mock<IFxService> _fxService;
        private readonly Mock<ILogger<FxController>> _logger;
        private readonly Mock<ILoginApiProxyService> _loginApiProxyService;

        public FxControllerTests()
        {
            _fxService = new Mock<IFxService>();
            _logger = new Mock<ILogger<FxController>>(); 
            _loginApiProxyService = new Mock<ILoginApiProxyService>();  
        }

        [Fact]
        public async Task GetCurrencyExchangeRate_should_return_an_unauthorised_access_exception()
        {
            //arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[HeaderNames.Authorization] = "Bearer 12321";
            GetExchangeRateRequest request = new GetExchangeRateRequest { CurrencyCodeFrom = Constants.AUDCurrencyCode, CurrencyCodeTo = Constants.CADCurrencyCode };
            
            decimal fxRate = Convert.ToDecimal(1.55);
            _fxService.Setup(x => x.GetCurrencyExchangeRate(request))
                    .ReturnsAsync(await Task.FromResult(fxRate));
            _loginApiProxyService.Setup(x => x.AuthenticateRequest("abcd")).Returns(Task.FromResult(false));
            var controller = new FxController(_logger.Object, _fxService.Object, _loginApiProxyService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            // act
            var ex = await Assert.ThrowsAsync<Exception>(async () => await controller.GetCurrencyExchangeRate(request));
            Assert.True(ex.Message.Contains("Unauthorized access"));
        }

        //[Fact]
        //public async Task GetCurrencyExchangeRate_should_return_400_bad_request_when_CurrencyCodeFrom_is_empty_or_null()
        //{
        //    //arrange
        //    string? token = "Bearer 12312";
        //    var httpContext = new DefaultHttpContext();
        //    httpContext.Request.Headers[HeaderNames.Authorization] = token;
        //    GetExchangeRateRequest request = new GetExchangeRateRequest { CurrencyCodeFrom = String.Empty, CurrencyCodeTo = Constants.CADCurrencyCode };

        //    decimal fxRate = Convert.ToDecimal(1.55);
        //    _fxService.Setup(x => x.GetCurrencyExchangeRate(request))
        //            .ReturnsAsync(await Task.FromResult(fxRate));
        //    _loginApiProxyService.Setup(x => x.AuthenticateRequest(token)).ReturnsAsync(await Task.FromResult(true));
        //    var controller = new FxController(_logger.Object, _fxService.Object, _loginApiProxyService.Object)
        //    {
        //        ControllerContext = new ControllerContext()
        //        {
        //            HttpContext = httpContext
        //        }
        //    };

        //    // act
        //    var result = await controller.GetCurrencyExchangeRate(request);
        //    //var statusCodeResult = (IStatusCodeActionResult)result;

        //    // assert
        //    //Assert.True(result?.Exception?.Message.Contains("Unauthorized access"));
        //    Assert.NotNull(result);
        //}
    }
}
