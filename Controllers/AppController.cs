using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoronaApi.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoronaApi.Controllers
{
    [Route("app")]
    public class AppController : Controller
    {
        [Route("data")]
        [HttpGet]
        [Produces("application/json","application/xml")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DataAsync([FromQuery]bool? sorted = false)
        {
            var dict = new List<CountyData>();

            var doc = await (new HtmlWeb())
                .LoadFromWebAsync("https://health.ny.gov/diseases/communicable/coronavirus/");
            
            IEnumerable<HtmlNode> trs = doc.DocumentNode
                .SelectSingleNode("//*[@id=\"case_count_table\"]")
                .Descendants("tr")
                .Where(x => !x.HasClass("total_row"));

            foreach(var tr in trs)
            {
                var tds = tr.Descendants("td");
                if(tds.FirstOrDefault() !=null){
                    var tdsArr = tds.ToArray();
                    dict.Add(
                        new CountyData { 
                            County = tds.FirstOrDefault().InnerText, 
                            Count = int.Parse(tdsArr[1].InnerText)
                        }					
                    );
                }
            }

            var response = new NYData
            { 
                Counties =  sorted.Value ? dict.OrderByDescending(x => x.Count).ToList() : dict
            };

            var acceptHeader = Request.Headers["accept"].ToString();
            if(!string.IsNullOrEmpty(acceptHeader) && acceptHeader.Contains("xml"))
            {
                using(var stringwriter = new System.IO.StringWriter())
                { 
                    var serializer = new System.Xml.Serialization.XmlSerializer(response.GetType());
                    serializer.Serialize(stringwriter, response);
                    return Content(stringwriter.ToString(),"application/xml");
                }
            }

            return Json(response);
        }
    }
}