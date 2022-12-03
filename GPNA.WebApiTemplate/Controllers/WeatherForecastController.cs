using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using GPNA.WebApiReceiver.Model;
using System.Net;
using Newtonsoft.Json;

namespace GPNA.WebApiReceiver.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public class WeatherForecastController : ControllerBase
    {
        #region Using
        private readonly IMapper _mapper;
        #endregion Using


        #region Constructors
        public WeatherForecastController(IMapper mapper,
            ILogger<WeatherForecastController> logger)
        {
            _mapper=mapper;
            _logger = logger;
        }
        #endregion Constructors

        private static readonly string[] Summaries = new[]
    {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        #region Methods
        /// <summary>
        /// Получить информацию
        /// </summary>
        /// <response code="200">Коллекция объектов топиков</response>
        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<SampleReport> Add()
        {
            using (WebClient wc = new())
            {
                var json = wc.DownloadString("http://localhost:5000/api/WeatherForecast/GetAll");
                var entity = JsonConvert.DeserializeObject<SampleReport>(json);
                return Ok(entity);
            }
        }
        #endregion Methods
    }
}
