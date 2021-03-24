using KissLog;
using Microsoft.AspNetCore.Mvc;


namespace DSharp.KissLog.Controllers
{
    [ApiController]
    [Route("teste-de-logs")]
    public class LoggerTestController : ControllerBase
    {

        private readonly ILogger _logger;
        public LoggerTestController(ILogger logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _logger.Debug("Teste Debug");
            _logger.Info("Teste Info");
            _logger.Warn("Teste Warning");
            _logger.Error("Teste Error");
            _logger.Critical("Teste Critial");

            return BadRequest("Consulte os logs na plataforma do KissLog");
        }
    }
}
