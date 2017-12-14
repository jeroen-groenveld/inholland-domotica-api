using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Domotica_API.Middleware;
using Microsoft.AspNetCore.Mvc;
using System.Xml;

namespace Domotica_API.Controllers
{
    [Route(Config.App.API_ROOT_PATH + "/news")]
    [MiddlewareFilter(typeof(TokenAuthorize))]
    public class NewsController : Controller
    {
        [HttpGet]
        public IActionResult GetNews(string latitude, string longitude, int days, string language)
        {
            List<object> result = new List<object>();

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Config.News.ENDPOINT);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(reader.ReadToEnd());

                    foreach (XmlNode item in doc.DocumentElement.SelectNodes(@"/rss/channel/item"))
                    {
                        List<string> categories = new List<string>();
                        foreach (XmlNode category in item.SelectNodes("category"))
                        {
                            categories.Add(category.FirstChild.Value);
                        }

                        result.Add(new
                        {
                            title = item.SelectSingleNode("title").FirstChild.Value,
                            link = item.SelectSingleNode("link").FirstChild.Value,
                            description = item.SelectSingleNode("description").FirstChild.Value,
                            date = item.SelectSingleNode("pubDate").FirstChild.Value,
                            image = item.SelectSingleNode("enclosure").Attributes["url"].Value,
                            categories = categories
                        });
                    }
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }
    }
}