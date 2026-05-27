using BooksApi.Models;
using BooksApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Text;
using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Linq;

namespace BooksApi.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet("LoggingLevels")]
        public ActionResult<string> LoggingLevels()
        {
            _logger.LogInformation("LogInformation => Controller: TestController");
            _logger.LogCritical("LogCritical => Controller: TestController");
            _logger.LogError("LogError => Controller: TestController");
            _logger.LogDebug("LogDebug => Controller: TestController");
            _logger.LogWarning("LogDebug => Controller: TestController");

            return "api-books logging-levels";
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

        [HttpGet("QueryString")]
        public IActionResult QueryString([FromQuery(Name = "querystring")] string querystring)
        {
            return Ok("query testing: " + querystring);
        }


        [HttpGet("SumNumbers")]
        public IActionResult SumNumbers(int num1, int num2)
        {
            return Ok(num1 + num2);
        }

        [HttpGet("PrintHostname")]
        public ActionResult<string> PrintHostname()
        {
            return Dns.GetHostName();
        }

        [HttpGet("IP")]
        public ActionResult<string> IP()
        {
            var addresses = NetworkInterface.GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up
                              && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .SelectMany(nic => nic.GetIPProperties().UnicastAddresses)
                .Where(addr => addr.Address.AddressFamily == AddressFamily.InterNetwork)
                .Select(addr => addr.Address.ToString());

            return string.Join(' ', addresses);
        }

        [HttpGet("GenerateException")]
        public ActionResult<string> GenerateException()
        {
            throw new InvalidOperationException("testing GenerateException ");
        }

        [HttpGet("Status")]
        public IActionResult Status([FromQuery(Name = "response")] int statusCode)
        {
            string message = GetStatusCodeMessage(statusCode);
            return StatusCode(statusCode, new { code = statusCode, message = message });
        }

        [HttpGet("time/{milliseconds}")]
        public async Task<IActionResult> Delay(int milliseconds)
        {
            if (milliseconds < 0)
            {
                return BadRequest("O tempo deve ser maior ou igual a zero");
            }

            if (milliseconds > 30000) // Limite de 30 segundos
            {
                return BadRequest("O tempo máximo permitido é de 30 segundos (30000 ms)");
            }

            await Task.Delay(milliseconds);
            return Ok(new { message = $"Resposta após aguardar {milliseconds} milissegundos", timestamp = DateTime.UtcNow });
        }

        private string GetStatusCodeMessage(int statusCode)
        {
            return statusCode switch
            {
                200 => "OK - Requisição bem sucedida",
                201 => "Created - Recurso criado com sucesso",
                202 => "Accepted - Requisição aceita para processamento",
                204 => "No Content - Requisição bem sucedida, sem conteúdo para retornar",

                300 => "Multiple Choices - Múltiplas opções disponíveis",
                301 => "Moved Permanently - Recurso movido permanentemente",
                302 => "Found - Recurso encontrado em outra URL",
                304 => "Not Modified - Recurso não foi modificado",

                400 => "Bad Request - Requisição inválida ou malformada",
                401 => "Unauthorized - Autenticação é necessária",
                403 => "Forbidden - Sem permissão para acessar o recurso",
                404 => "Not Found - Recurso não encontrado",
                405 => "Method Not Allowed - Método HTTP não permitido",
                406 => "Not Acceptable - Não foi possível encontrar conteúdo de acordo com os critérios aceitos",
                408 => "Request Timeout - Tempo limite da requisição excedido",
                409 => "Conflict - Conflito no estado atual do recurso",
                410 => "Gone - Recurso não está mais disponível",
                413 => "Payload Too Large - Tamanho da requisição excede o limite",
                415 => "Unsupported Media Type - Formato de mídia não suportado",
                429 => "Too Many Requests - Muitas requisições em um período de tempo",

                500 => "Internal Server Error - Erro interno do servidor",
                501 => "Not Implemented - Funcionalidade não implementada",
                502 => "Bad Gateway - Resposta inválida do servidor upstream",
                503 => "Service Unavailable - Serviço temporariamente indisponível",
                504 => "Gateway Timeout - Tempo limite excedido no gateway",

                _ => $"Status code {statusCode} - Código personalizado"
            };
        }
    }
}
