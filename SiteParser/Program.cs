using SiteParser.Infrastructure.Abstract;
using SiteParser.Infrastructure.Implements;
using System;
using System.IO;
using System.Reflection;

namespace SiteParser
{
    class Program
    {
        static void Main(string[] args)
        {
            IExcelExport excelExport = new ExcelExport(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Cian's advertisements.csv");
            ILogger logger = new Logger();
            IHtmlXPathParser htmlXPathParser = new HtmlXPathParser();

            IRequestToServer webRequest = new RequestToServer();
            webRequest.SetWebProxy("195.64.212.66", 8080);
            webRequest.IsRequestByProxy = false;

            AdvertisementsInfo advertisementInfo = new AdvertisementsInfo("https://www.cian.ru/kupit-ofis/");

            advertisementInfo.SetExcelExport(excelExport);
            advertisementInfo.SetLogger(logger);
            advertisementInfo.SetWebRequest(webRequest);
            advertisementInfo.SetHtmlXPathParser(htmlXPathParser);

            advertisementInfo.GetAllAdvertisements();

            Console.ReadKey();
        }
    }
}
