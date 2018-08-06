using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static CMSImportLibrary.Constants.FixedProperties;

namespace baseCMS.Models
{
    public class ChartData
    {
       public Dictionary<String,Object> KeyValues { get; set; }
       public String Title { get; set; }
       public String Style { get; set; }
        public ChartData()
        {
            KeyValues = new Dictionary<String, Object>();
            Title = "Unknown";
            Style = "pie";
        }
    }
}
