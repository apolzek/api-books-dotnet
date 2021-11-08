using BooksApi.Models;
using BooksApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace BooksApi.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        // private readonly BookService _bookService;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            _logger.LogInformation("LogInformation => Controller: TestController");
            _logger.LogCritical("LogCritical => Controller: TestController");
            _logger.LogError("LogError => Controller: TestController");
            _logger.LogDebug("LogDebug => Controller: TestController");
            _logger.LogWarning("LogDebug => Controller: TestController");

            return "api-books";
        }

        [HttpGet("name")]
        public ActionResult<string> GetResult(string name)
        {
            return "Hello " + name;
        }

        [HttpGet("GetAllHeaders")]
        public ActionResult<Dictionary<string, string>> GetAllHeaders()
        {
            Dictionary<string, string> requestHeaders =
                new Dictionary<string, string>();
            foreach (var header in Request.Headers)
            {
                requestHeaders.Add(header.Key, header.Value);
            }
            return requestHeaders;
        }

        [HttpGet("testing")]
        public IActionResult Get([FromQuery(Name = "querystring")] string querystring)
        {
            return Ok("query testing: " + querystring);
        }


        [HttpGet("sum")]
        public IActionResult Details(int num1, int num2)
        {
            return Ok(num1 + num2);
        }

    }
}
