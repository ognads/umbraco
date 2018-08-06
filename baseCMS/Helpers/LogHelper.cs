using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace baseCMS.Helpers
{
    public class CustomLogHelper
    {
        private static CustomLogHelper Instance;
        public static CustomLogHelper logHelper
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new CustomLogHelper();
                }
                return Instance;
            }
        }

        public void Log(string Message)
        {
            try
            {
                System.IO.File.AppendAllText(HttpRuntime.AppDomainAppPath+"App_Data\\Logs\\CustomLog.txt", DateTime.Now.ToString("dd/MM/yy H:mm:ss") +"\t" +Message+System.Environment.NewLine);
            }
            catch(Exception ex) { }
        }
        public void Log(string Message,Exception ex)
        {
            try
            {
                System.IO.File.AppendAllText(HttpRuntime.AppDomainAppPath+"App_Data\\Logs\\CustomLog.txt", DateTime.Now.ToString("dd/MM/yy H:mm:ss") +"\t" +Message+System.Environment.NewLine
                +ex.Message +System.Environment.NewLine);
            }
            catch(Exception exs) {

            }
        }
    }
}
