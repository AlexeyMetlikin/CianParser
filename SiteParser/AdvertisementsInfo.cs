using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using SiteParser.Infrastructure.Abstract;
using SiteParser.Model;
using SiteParser.Infrastructure.Implements;
using System.Net.Http;

namespace SiteParser
{
    public class AdvertisementsInfo
    {
        private readonly string xPathNextPage = "//ul[contains(@class,'list')]/li[contains(@class,'list-item--active')]/following-sibling::li/a";

        private readonly string xPathPageAdsHeader = "//div[contains(@class,'header')]/h3[contains(@class,'header-title--')]/a[contains(@class,'header-link--')]";
        private readonly string xPathPageAdsFooter = "//div[contains(@class,'content')]/div[contains(@class,'content-footer--')]/a[contains(@class,'cardLink--')]";

        private readonly string xPathAdvTitle = "//h1[@class='object_descr_addr']";
        private readonly string xPathAdvPrice = "//div[@class='cf-object-descr-add']";
        private readonly string xPathAdvPlacingDate = "//ul[@class='offerStatuses']/following-sibling::span";
        private readonly string xPathAdvSquare = "//div[contains(@class,'offer_container')]/table/tr/td[1]/article/section[1]/dl[dt='Площадь:']/dd";
        private readonly string xPathAdvPhotos = "//div[@class='fotorama']/img";

        private int _adsProcessed;
        private string _startURL;

        public IExcelExport ExcelExport { get; private set; }
        public ILogger Logger { get; private set; }
        public IRequestToServer WebRequest { get; private set; }
        public IHtmlXPathParser HtmlXPathParser { get; private set; }

        public List<Advertisement> Advertisements { get; set; }

        public void SetExcelExport(IExcelExport excelExport)
        {
            ExcelExport = excelExport;
        }

        public void SetLogger(ILogger logger)
        {
            Logger = logger;
        }

        public void SetWebRequest(IRequestToServer webRequest)
        {
            WebRequest = webRequest;
        }

        public void SetHtmlXPathParser(IHtmlXPathParser htmlXPathParser)
        {
            HtmlXPathParser = htmlXPathParser;
        }

        public AdvertisementsInfo(string url)
        {
            _startURL = url;
            _adsProcessed = 0;

            ExcelExport = new ExcelExport("", "temp.csv");
            Logger = new Logger();
            WebRequest = new RequestToServer();
            HtmlXPathParser = new HtmlXPathParser();            

            Advertisements = new List<Advertisement>();
        }

        public void GetAllAdvertisements()
        {
            string currentURL = _startURL;
            string content;

            Logger.Write("Начата обработка объявлений");

            while(!string.IsNullOrEmpty(currentURL))
            {
                content = GetHtmlContent(currentURL);

                if (string.IsNullOrEmpty(content))
                {
                    break;
                }

                GetAdvertisementsFromPage(content);

                currentURL = GetNextPage(content);

                ExportAdvertisementsToExcel();

                Logger.Write(string.Format("Количество обработанных объявлений: {0}", _adsProcessed));
            }

            SendProcessResultMessage();            
        }

        private string GetHtmlContent(string currentURL)
        {
            var htmlContent = string.Empty;
            try
            {
                htmlContent = WebRequest.GetPageContent(currentURL);
            }
            catch (HttpRequestException httpRequestException)
            {
                Logger.Write(httpRequestException.Message);
            }
            return htmlContent;
        }

        private void GetAdvertisementsFromPage(string content)
        {
            var attrName = "href";

            var advertisements = HtmlXPathParser.GetAttributesValue(content, xPathPageAdsHeader, attrName);

            if (advertisements.Count == 0)
            {
                advertisements = HtmlXPathParser.GetAttributesValue(content, xPathPageAdsFooter, attrName);
            }

            foreach (var advertisement in advertisements)
            {
                GetAdvertisementInfo(advertisement);
            }
        }

        private void ExportAdvertisementsToExcel()
        {
            if (Advertisements.Count > 0)
            {
                DataTable advertisementsTable = ExcelExport.ListToDataTable(Advertisements);
                advertisementsTable = ExcelExport
                    .SetHeadersCaption(
                        advertisementsTable,
                        new string[]
                        {
                        "Адрес объекта",
                        "Дата размещения объявления",
                        "Цена за кв.м",
                        "Площадь объекта",
                        "Фото объекта"
                        });

                _adsProcessed += Advertisements.Count;
                Advertisements.Clear();

                ExcelExport.ExportExcel(advertisementsTable);
            }
        }

        private void GetAdvertisementInfo(string linkToAdvertisement)
        {
            Advertisement advertisement = new Advertisement();

            string content = GetHtmlContent(linkToAdvertisement);

            advertisement.Address = GetTagText(content, xPathAdvTitle);
            advertisement.Price = GetPriceGetPricePerSquareMetr(content, xPathAdvPrice);
            advertisement.PlacingDate = GetDateFromString(content, xPathAdvPlacingDate);
            advertisement.Square = GetSquareFromString(content, xPathAdvSquare);
            advertisement.Photos = GetPhotosString(content, xPathAdvPhotos);

            Advertisements.Add(advertisement);
        }

        private string GetPriceGetPricePerSquareMetr(string content, string xPath)
        {
            var price = GetTagText(content, xPath);
            if (price != null)
            {
                price = Regex.Replace(price.Replace("&sup", "").Replace("&#36;", "$"), " за м.+", "");
            }
            return price;
        }

        private DateTime? GetDateFromString(string content, string xPath)
        {
            var stringDate = GetTagText(content, xPath);
            return ParseStringToDate(stringDate);
        }

        private double? GetSquareFromString(string content, string xPath)
        {
            var stringNum = GetTagText(content, xPath);
            return ParseStringToDouble(stringNum);
        }

        private string GetTagText(string content, string xPath)
        {
            var innerText = HtmlXPathParser.GetTagInnerText(content, xPath);
            if(innerText != null)
            {
                innerText = Regex.Replace(innerText.Replace("\n", "").Replace("&nbsp;", " ").Replace("&ndash;", "–").Trim(), "[ ]+|&nbsp;", " ");
            }
            return innerText;
        }

        private string GetNextPage(string content)
        {
            var attrName = "href";

            return HtmlXPathParser.GetAttributeValue(content, xPathNextPage, attrName);
        }

        private DateTime? ParseStringToDate(string stringDate)
        {
            try
            {
                string[] dateTime = stringDate.Split(' ');

                string[] time = dateTime[1].Split(':');
                string[] date = dateTime[0].Split('.');

                DateTime resultDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, int.Parse(time[0]), int.Parse(time[1]), 0);

                switch (dateTime[0].Replace(",", "").ToLower())
                {
                    case "сегодня":
                        break;

                    case "вчера":
                        resultDate = resultDate.AddDays(-1);
                        break;

                    case "позавчера":
                        resultDate = resultDate.AddDays(-2);
                        break;

                    default:
                        resultDate = new DateTime(int.Parse(date[2]), int.Parse(date[1]), int.Parse(date[0]), int.Parse(time[0]), int.Parse(time[1]), 0);
                        break;
                }

                return resultDate;
            }
            catch
            {
                return null;
            }
        }

        private double? ParseStringToDouble(string stringNum)
        {
            try
            {
                string[] allNumParts = stringNum.Split(' ');

                return double.Parse(allNumParts[0].ToString());
            }
            catch
            {
                return null;
            }
        }

        private string GetPhotosString(string content, string xPath)
        {
            StringBuilder photoSources = new StringBuilder();
            var attrName = "src";

            var images = HtmlXPathParser.GetAttributesValue(content, xPath, attrName);

            foreach (var image in images)
            {
                photoSources.Append(image == null ? null : image + ';');
            }

            return photoSources.ToString();
        }

        private void SendProcessResultMessage()
        {
            if (_adsProcessed > 0)
            {
                Logger.Write(string.Format("Информация об объявлениях доступна по пути {0}", ExcelExport.FullPath));
            }
            else
            {
                Logger.Write("Сервер недоступен, либо превышен запрос обращений на сервер");
            }
        }
    }
}
