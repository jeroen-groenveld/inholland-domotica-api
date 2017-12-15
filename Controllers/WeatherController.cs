using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Domotica_API.Controllers;
using Domotica_API.Middleware;
using Domotica_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Domotica_API.Controllers
{
	[Route(Config.App.API_ROOT_PATH + "/weather")]
	[MiddlewareFilter(typeof(TokenAuthorize))]
	public class WeatherController : ApiController
	{
		//Constructor
		public WeatherController(DatabaseContext db) : base(db) { }

		[HttpGet("lat/{latitude}/lon/{longitude}/cnt/{days}/lang/{lang}/units/{units}")]
		public IActionResult GetWeather(string latitude, string longitude, int days, string language, string units)
		{
			string appid = env.WEATHER_API_KEY;
			string body = string.Empty;

			string url = Config.Weather.ENDPOINT + String.Format($"?lat={latitude}&lon={longitude}&cnt={days}&lang={language}&units={units}&appid={appid}");

			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.AutomaticDecompression = DecompressionMethods.GZip;

				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
				using (Stream stream = response.GetResponseStream())
				using (StreamReader reader = new StreamReader(stream))
				{
					body = reader.ReadToEnd();
				}
				return Ok(Newtonsoft.Json.JsonConvert.DeserializeObject(body));
			}
			catch (Exception e)
			{
				return BadRequest(e.ToString());
			}
		}
	}
}