using System.Collections.Generic;
using System.Xml.Serialization;

namespace CoronaApi.Models
{
    public class CountyData
    {
        public string County {get;set;}
        public int Count { get; set; }
    }

    public class NYData
    {
        [XmlArray]
        public List<CountyData> Counties {get;set;}
    }
}