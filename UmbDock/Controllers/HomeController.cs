using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using UmbDock.Models;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace UmbDock
{
    public class HomeController : RenderController
    {
        private readonly ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor) : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            this.logger = logger;
        }

        public override IActionResult Index()
        {

            var apiResp = GetWeatherForecasts("http://api-container:80/weatherforecast");
            TempData["ApiResponse"] = apiResp;

            return CurrentTemplate(CurrentPage);
        }

        private List<WeatherForecast> GetWeatherForecasts(string url)
        {
            var resp = new List<WeatherForecast>();

            logger.LogInformation("About to call API :" + url);

            try
            {
                // Doing this verbose so people can debug it if they want            
                // Todo : Make this a configuration option
                var client = new System.Net.Http.HttpClient();
                var response = client.GetAsync(url).Result;
                var result = response.Content.ReadAsStringAsync().Result;

                logger.LogInformation("API Response :" + result);

                var weatherForecast = Newtonsoft.Json.JsonConvert.DeserializeObject<WeatherForecast[]>(result);
                resp.AddRange(weatherForecast);
            }
            catch (Exception e)
            {
                logger.LogError("Exception : {0}", e.Message);
            }
            return resp;
        }
    }
}
