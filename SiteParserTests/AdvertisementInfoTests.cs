using Moq;
using NUnit.Framework;
using SiteParser;
using SiteParser.Infrastructure.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteParserTests
{
    [TestFixture]
    public class AdvertisementInfoTests
    {
        [Test]
        public void Can_Set_Excel_Export()
        {
            //Arrange
            var mockExcelExport = new Mock<IExcelExport>();

            var advertisementInfo = new AdvertisementsInfo("new_url");

            //Act
            advertisementInfo.SetExcelExport(mockExcelExport.Object);

            //Assert
            Assert.IsNotNull(advertisementInfo.ExcelExport);
            Assert.AreEqual(advertisementInfo.ExcelExport, mockExcelExport.Object);
        }

        [Test]
        public void Cannot_Set_Null_Excel_Export()
        {
            //Arrange
            var mockExcelExport = new Mock<IExcelExport>();
            var advertisementInfo = new AdvertisementsInfo("new_url");

            //Act
            advertisementInfo.SetExcelExport(null);

            //Assert
            Assert.IsNull(advertisementInfo.ExcelExport);
        }

        [Test]
        public void Can_Set_Html_XPath_Parser()
        {
            //Arrange
            var mockHtmlXPathParser = new Mock<IHtmlXPathParser>();
            var advertisementInfo = new AdvertisementsInfo("new_url");

            //Act
            advertisementInfo.SetHtmlXPathParser(mockHtmlXPathParser.Object);

            //Assert
            Assert.IsNotNull(advertisementInfo.HtmlXPathParser);
            Assert.AreEqual(advertisementInfo.HtmlXPathParser, mockHtmlXPathParser.Object);
        }

        [Test]
        public void Cannot_Set_Null_Html_XPath_Parser()
        {
            //Arrange
            var mockHtmlXPathParser = new Mock<IHtmlXPathParser>();
            var advertisementInfo = new AdvertisementsInfo("new_url");

            //Act
            advertisementInfo.SetHtmlXPathParser(null);

            //Assert
            Assert.IsNull(advertisementInfo.HtmlXPathParser);
        }

        [Test]
        public void Can_Set_Logger()
        {
            //Arrange
            var mockLogger = new Mock<ILogger>();
            var advertisementInfo = new AdvertisementsInfo("new_url");

            //Act
            advertisementInfo.SetLogger(mockLogger.Object);

            //Assert
            Assert.IsNotNull(advertisementInfo.Logger);
            Assert.AreEqual(advertisementInfo.Logger, mockLogger.Object);
        }

        [Test]
        public void Cannot_Set_Null_Logger()
        {
            //Arrange
            var mockLogger = new Mock<IHtmlXPathParser>();
            var advertisementInfo = new AdvertisementsInfo("new_url");

            //Act
            advertisementInfo.SetLogger(null);

            //Assert
            Assert.IsNull(advertisementInfo.Logger);
        }

        [Test]
        public void Can_Set_WebRequest()
        {
            //Arrange
            var mockWebRequest = new Mock<IRequestToServer>();
            var advertisementInfo = new AdvertisementsInfo("new_url");

            //Act
            advertisementInfo.SetWebRequest(mockWebRequest.Object);

            //Assert
            Assert.IsNotNull(advertisementInfo.WebRequest);
            Assert.AreEqual(advertisementInfo.WebRequest, mockWebRequest.Object);
        }

        [Test]
        public void Cannot_Set_Null_WebRequest()
        {
            //Arrange
            var mockWebRequest = new Mock<IRequestToServer>();
            var advertisementInfo = new AdvertisementsInfo("new_url");

            //Act
            advertisementInfo.SetWebRequest(null);

            //Assert
            Assert.IsNull(advertisementInfo.WebRequest);
        }

        [Test]
        public void Can_Get_Title_Advertisement_With_Valid_Title()
        {
            //Arrange
            string url = "test_url";

            string output = null;
            string excelResult = null;
            string excelPath = "test_file.csv";
            DataTable adsTable = null;

            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(x => x.Write(It.IsAny<string>()))
                .Callback<string>(x => output = x);

            var mockExcelExport = new Mock<IExcelExport>();
            mockExcelExport.Setup(x => x.ExportExcel(It.IsAny<DataTable>(), It.IsAny<string>()))
                .Callback<DataTable, string>(
                (table, s) =>
                {
                    adsTable = table;
                    excelResult = "exportComplete";
                });
            mockExcelExport.Object.SetFullPath("", "test_file.csv");

            var mockHtmlXPathParser = new Mock<IHtmlXPathParser>();
            mockHtmlXPathParser.Setup(x => x.GetAttributesValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<string> { "attributesValue" });
            mockHtmlXPathParser.Setup(x => x.GetAttributeValue(It.IsNotIn("pageContent"), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("attributeValue");
            mockHtmlXPathParser.Setup(x => x.GetAttributeValue(It.IsIn("pageContent"), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("");
            mockHtmlXPathParser.Setup(x => x.GetTagInnerText(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("Address");
            mockExcelExport.Object.SetFullPath("", "test_file.csv");

            var mockWebRequest = new Mock<IRequestToServer>();
            mockWebRequest.Setup(x => x.GetPageContent(It.IsAny<string>()))
                .Returns("pageContent");

            var advertisementInfo = new AdvertisementsInfo(url);
            advertisementInfo.SetLogger(mockLogger.Object);
            advertisementInfo.SetExcelExport(mockExcelExport.Object);
            advertisementInfo.SetHtmlXPathParser(mockHtmlXPathParser.Object);
            advertisementInfo.SetWebRequest(mockWebRequest.Object);

            //Act
            advertisementInfo.GetAllAdvertisements();

            //Assert
            Assert.IsNotNull(output);
            Assert.AreEqual(output, "Информация об объявлениях доступна по пути " + excelPath);

            Assert.IsNotNull(excelResult);
            Assert.AreEqual(excelResult, "exportComplete");

            Assert.AreEqual(adsTable.Rows.Count, 1);
            Assert.AreEqual(adsTable.Rows[0]["Address"], "Address");
        }

        [Test]
        public void Cannot_Get_Title_Advertisement_With_Null_Title()
        {
            //Arrange
            string url = "test_url";
            string null_value = null;

            string output = null;
            string excelResult = null;
            string excelPath = "test_file.csv";
            DataTable adsTable = null;

            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(x => x.Write(It.IsAny<string>()))
                .Callback<string>(x => output = x);

            var mockExcelExport = new Mock<IExcelExport>();
            mockExcelExport.Setup(x => x.ExportExcel(It.IsAny<DataTable>(), It.IsAny<string>()))
                .Callback<DataTable, string>(
                (table, s) =>
                {
                    adsTable = table;
                    excelResult = "exportComplete";
                });
            mockExcelExport.Object.SetFullPath("", "test_file.csv");

            var mockHtmlXPathParser = new Mock<IHtmlXPathParser>();
            mockHtmlXPathParser.Setup(x => x.GetAttributesValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<string> { "attributesValue" });
            mockHtmlXPathParser.Setup(x => x.GetAttributeValue(It.IsNotIn("pageContent"), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("attributeValue");
            mockHtmlXPathParser.Setup(x => x.GetAttributeValue(It.IsIn("pageContent"), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("");
            mockHtmlXPathParser.Setup(x => x.GetTagInnerText(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(null_value);
            mockExcelExport.Object.SetFullPath("", "test_file.csv");

            var mockWebRequest = new Mock<IRequestToServer>();
            mockWebRequest.Setup(x => x.GetPageContent(It.IsAny<string>()))
                .Returns("pageContent");

            var advertisementInfo = new AdvertisementsInfo(url);
            advertisementInfo.SetLogger(mockLogger.Object);
            advertisementInfo.SetExcelExport(mockExcelExport.Object);
            advertisementInfo.SetHtmlXPathParser(mockHtmlXPathParser.Object);
            advertisementInfo.SetWebRequest(mockWebRequest.Object);

            //Act
            advertisementInfo.GetAllAdvertisements();

            //Assert
            Assert.IsNotNull(output);
            Assert.AreEqual(output, "Информация об объявлениях доступна по пути " + excelPath);

            Assert.IsNotNull(excelResult);
            Assert.AreEqual(excelResult, "exportComplete");

            Assert.AreEqual(adsTable.Rows.Count, 1);
            Assert.IsTrue(string.IsNullOrEmpty(adsTable.Rows[0]["Address"].ToString()));
        }

        [Test]
        public void Can_Get_Price_Advertisement_With_Valid_Price()
        {
            //Arrange
            string url = "test_url";

            string output = null;
            string excelResult = null;
            string excelPath = "test_file.csv";
            DataTable adsTable = null;

            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(x => x.Write(It.IsAny<string>()))
                .Callback<string>(x => output = x);

            var mockExcelExport = new Mock<IExcelExport>();
            mockExcelExport.Setup(x => x.ExportExcel(It.IsAny<DataTable>(), It.IsAny<string>()))
                .Callback<DataTable, string>(
                (table, s) =>
                {
                    adsTable = table;
                    excelResult = "exportComplete";
                });
            mockExcelExport.Object.SetFullPath("", "test_file.csv");

            var mockHtmlXPathParser = new Mock<IHtmlXPathParser>();
            mockHtmlXPathParser.Setup(x => x.GetAttributesValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<string> { "attributesValue" });
            mockHtmlXPathParser.Setup(x => x.GetAttributeValue(It.IsNotIn("pageContent"), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("attributeValue");
            mockHtmlXPathParser.Setup(x => x.GetAttributeValue(It.IsIn("pageContent"), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("");
            mockHtmlXPathParser.Setup(x => x.GetTagInnerText(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("1&nbsp;018&nbsp;500&nbsp;$");
            mockExcelExport.Object.SetFullPath("", "test_file.csv");

            var mockWebRequest = new Mock<IRequestToServer>();
            mockWebRequest.Setup(x => x.GetPageContent(It.IsAny<string>()))
                .Returns("pageContent");

            var advertisementInfo = new AdvertisementsInfo(url);
            advertisementInfo.SetLogger(mockLogger.Object);
            advertisementInfo.SetExcelExport(mockExcelExport.Object);
            advertisementInfo.SetHtmlXPathParser(mockHtmlXPathParser.Object);
            advertisementInfo.SetWebRequest(mockWebRequest.Object);

            //Act
            advertisementInfo.GetAllAdvertisements();

            //Assert
            Assert.IsNotNull(output);
            Assert.AreEqual(output, "Информация об объявлениях доступна по пути " + excelPath);

            Assert.IsNotNull(excelResult);
            Assert.AreEqual(excelResult, "exportComplete");

            Assert.AreEqual(adsTable.Rows.Count, 1);
            Assert.AreEqual(adsTable.Rows[0]["Price"], "1 018 500 $");
        }

        [Test]
        public void Cannot_Get_Price_Advertisement_With_Null_Price()
        {
            //Arrange
            string url = "test_url";
            string null_value = null;

            string output = null;
            string excelResult = null;
            string excelPath = "test_file.csv";
            DataTable adsTable = null;

            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(x => x.Write(It.IsAny<string>()))
                .Callback<string>(x => output = x);

            var mockExcelExport = new Mock<IExcelExport>();
            mockExcelExport.Setup(x => x.ExportExcel(It.IsAny<DataTable>(), It.IsAny<string>()))
                .Callback<DataTable, string>(
                (table, s) =>
                {
                    adsTable = table;
                    excelResult = "exportComplete";
                });
            mockExcelExport.Object.SetFullPath("", "test_file.csv");

            var mockHtmlXPathParser = new Mock<IHtmlXPathParser>();
            mockHtmlXPathParser.Setup(x => x.GetAttributesValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<string> { "attributesValue" });
            mockHtmlXPathParser.Setup(x => x.GetAttributeValue(It.IsNotIn("pageContent"), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("attributeValue");
            mockHtmlXPathParser.Setup(x => x.GetAttributeValue(It.IsIn("pageContent"), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("");
            mockHtmlXPathParser.Setup(x => x.GetTagInnerText(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(null_value);
            mockExcelExport.Object.SetFullPath("", "test_file.csv");

            var mockWebRequest = new Mock<IRequestToServer>();
            mockWebRequest.Setup(x => x.GetPageContent(It.IsAny<string>()))
                .Returns("pageContent");

            var advertisementInfo = new AdvertisementsInfo(url);
            advertisementInfo.SetLogger(mockLogger.Object);
            advertisementInfo.SetExcelExport(mockExcelExport.Object);
            advertisementInfo.SetHtmlXPathParser(mockHtmlXPathParser.Object);
            advertisementInfo.SetWebRequest(mockWebRequest.Object);

            //Act
            advertisementInfo.GetAllAdvertisements();

            //Assert
            Assert.IsNotNull(output);
            Assert.AreEqual(output, "Информация об объявлениях доступна по пути " + excelPath);

            Assert.IsNotNull(excelResult);
            Assert.AreEqual(excelResult, "exportComplete");

            Assert.AreEqual(adsTable.Rows.Count, 1);
            Assert.IsTrue(string.IsNullOrEmpty(adsTable.Rows[0]["Price"].ToString()));
        }

        [Test]
        public void Can_Get_PlacingDate_Advertisements_Info_From_Valid_PlacingDate()
        {
            //Arrange
            string url = "test_url";

            string output = null;
            string excelResult = null;
            string excelPath = "test_file.csv";
            DataTable adsTable = null;

            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(x => x.Write(It.IsAny<string>()))
                .Callback<string>(x => output = x);

            var mockExcelExport = new Mock<IExcelExport>();
            mockExcelExport.Setup(x => x.ExportExcel(It.IsAny<DataTable>(), It.IsAny<string>()))
                .Callback<DataTable, string>(
                (table, s) =>
                {
                    adsTable = table;
                    excelResult = "exportComplete";
                });
            mockExcelExport.Object.SetFullPath("", "test_file.csv");

            var mockHtmlXPathParser = new Mock<IHtmlXPathParser>();
            mockHtmlXPathParser.Setup(x => x.GetAttributesValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<string> { "attributesValue" });
            mockHtmlXPathParser.Setup(x => x.GetAttributeValue(It.IsNotIn("pageContent"), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("attributeValue");
            mockHtmlXPathParser.Setup(x => x.GetAttributeValue(It.IsIn("pageContent"), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("");
            mockHtmlXPathParser.Setup(x => x.GetTagInnerText(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("30.01.2017 18:50");
            mockExcelExport.Object.SetFullPath("", "test_file.csv");

            var mockWebRequest = new Mock<IRequestToServer>();
            mockWebRequest.Setup(x => x.GetPageContent(It.IsAny<string>()))
                .Returns("pageContent");

            var advertisementInfo = new AdvertisementsInfo(url);
            advertisementInfo.SetLogger(mockLogger.Object);
            advertisementInfo.SetExcelExport(mockExcelExport.Object);
            advertisementInfo.SetHtmlXPathParser(mockHtmlXPathParser.Object);
            advertisementInfo.SetWebRequest(mockWebRequest.Object);

            //Act
            advertisementInfo.GetAllAdvertisements();

            //Assert
            Assert.IsNotNull(output);
            Assert.AreEqual(output, "Информация об объявлениях доступна по пути " + excelPath);

            Assert.IsNotNull(excelResult);
            Assert.AreEqual(excelResult, "exportComplete");

            Assert.AreEqual(adsTable.Rows.Count, 1);
            Assert.AreEqual(adsTable.Rows[0]["PlacingDate"], new DateTime(2017, 01, 30, 18, 50, 00));
        }

        [Test]
        public void Cannot_Get_PlacingDate_Advertisement_With_Null_PlacingDate()
        {
            //Arrange
            string url = "test_url";
            string null_value = null;

            string output = null;
            string excelResult = null;
            string excelPath = "test_file.csv";
            DataTable adsTable = null;

            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(x => x.Write(It.IsAny<string>()))
                .Callback<string>(x => output = x);

            var mockExcelExport = new Mock<IExcelExport>();
            mockExcelExport.Setup(x => x.ExportExcel(It.IsAny<DataTable>(), It.IsAny<string>()))
                .Callback<DataTable, string>(
                (table, s) =>
                {
                    adsTable = table;
                    excelResult = "exportComplete";
                });
            mockExcelExport.Object.SetFullPath("", "test_file.csv");

            var mockHtmlXPathParser = new Mock<IHtmlXPathParser>();
            mockHtmlXPathParser.Setup(x => x.GetAttributesValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<string> { "attributesValue" });
            mockHtmlXPathParser.Setup(x => x.GetAttributeValue(It.IsNotIn("pageContent"), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("attributeValue");
            mockHtmlXPathParser.Setup(x => x.GetAttributeValue(It.IsIn("pageContent"), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("");
            mockHtmlXPathParser.Setup(x => x.GetTagInnerText(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(null_value);
            mockExcelExport.Object.SetFullPath("", "test_file.csv");

            var mockWebRequest = new Mock<IRequestToServer>();
            mockWebRequest.Setup(x => x.GetPageContent(It.IsAny<string>()))
                .Returns("pageContent");

            var advertisementInfo = new AdvertisementsInfo(url);
            advertisementInfo.SetLogger(mockLogger.Object);
            advertisementInfo.SetExcelExport(mockExcelExport.Object);
            advertisementInfo.SetHtmlXPathParser(mockHtmlXPathParser.Object);
            advertisementInfo.SetWebRequest(mockWebRequest.Object);

            //Act
            advertisementInfo.GetAllAdvertisements();

            //Assert
            Assert.IsNotNull(output);
            Assert.AreEqual(output, "Информация об объявлениях доступна по пути " + excelPath);

            Assert.IsNotNull(excelResult);
            Assert.AreEqual(excelResult, "exportComplete");

            Assert.AreEqual(adsTable.Rows.Count, 1);
            Assert.IsTrue(string.IsNullOrEmpty(adsTable.Rows[0]["PlacingDate"].ToString()));
        }

        [Test]
        public void Can_Get_Square_Advertisement_With_Valid_Square()
        {
            //Arrange
            string url = "test_url";

            string output = null;
            string excelResult = null;
            string excelPath = "test_file.csv";
            DataTable adsTable = null;

            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(x => x.Write(It.IsAny<string>()))
                .Callback<string>(x => output = x);

            var mockExcelExport = new Mock<IExcelExport>();
            mockExcelExport.Setup(x => x.ExportExcel(It.IsAny<DataTable>(), It.IsAny<string>()))
                .Callback<DataTable, string>(
                (table, s) =>
                {
                    adsTable = table;
                    excelResult = "exportComplete";
                });
            mockExcelExport.Object.SetFullPath("", "test_file.csv");

            var mockHtmlXPathParser = new Mock<IHtmlXPathParser>();
            mockHtmlXPathParser.Setup(x => x.GetAttributesValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<string> { "attributesValue" });
            mockHtmlXPathParser.Setup(x => x.GetAttributeValue(It.IsNotIn("pageContent"), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("attributeValue");
            mockHtmlXPathParser.Setup(x => x.GetAttributeValue(It.IsIn("pageContent"), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("");
            mockHtmlXPathParser.Setup(x => x.GetTagInnerText(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("18,7 м2");
            mockExcelExport.Object.SetFullPath("", "test_file.csv");

            var mockWebRequest = new Mock<IRequestToServer>();
            mockWebRequest.Setup(x => x.GetPageContent(It.IsAny<string>()))
                .Returns("pageContent");

            var advertisementInfo = new AdvertisementsInfo(url);
            advertisementInfo.SetLogger(mockLogger.Object);
            advertisementInfo.SetExcelExport(mockExcelExport.Object);
            advertisementInfo.SetHtmlXPathParser(mockHtmlXPathParser.Object);
            advertisementInfo.SetWebRequest(mockWebRequest.Object);

            //Act
            advertisementInfo.GetAllAdvertisements();

            //Assert
            Assert.IsNotNull(output);
            Assert.AreEqual(output, "Информация об объявлениях доступна по пути " + excelPath);

            Assert.IsNotNull(excelResult);
            Assert.AreEqual(excelResult, "exportComplete");

            Assert.AreEqual(adsTable.Rows.Count, 1);
            Assert.AreEqual(adsTable.Rows[0]["Square"].ToString(), "18,7");
        }

        [Test]
        public void Cannot_Get_Square_Advertisement_With_Null_Square()
        {
            //Arrange
            string url = "test_url";
            string null_value = null;

            string output = null;
            string excelResult = null;
            string excelPath = "test_file.csv";
            DataTable adsTable = null;

            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(x => x.Write(It.IsAny<string>()))
                .Callback<string>(x => output = x);

            var mockExcelExport = new Mock<IExcelExport>();
            mockExcelExport.Setup(x => x.ExportExcel(It.IsAny<DataTable>(), It.IsAny<string>()))
                .Callback<DataTable, string>(
                (table, s) =>
                {
                    adsTable = table;
                    excelResult = "exportComplete";
                });
            mockExcelExport.Object.SetFullPath("", "test_file.csv");

            var mockHtmlXPathParser = new Mock<IHtmlXPathParser>();
            mockHtmlXPathParser.Setup(x => x.GetAttributesValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<string> { "attributesValue" });
            mockHtmlXPathParser.Setup(x => x.GetAttributeValue(It.IsNotIn("pageContent"), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("attributeValue");
            mockHtmlXPathParser.Setup(x => x.GetAttributeValue(It.IsIn("pageContent"), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("");
            mockHtmlXPathParser.Setup(x => x.GetTagInnerText(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(null_value);
            mockExcelExport.Object.SetFullPath("", "test_file.csv");

            var mockWebRequest = new Mock<IRequestToServer>();
            mockWebRequest.Setup(x => x.GetPageContent(It.IsAny<string>()))
                .Returns("pageContent");

            var advertisementInfo = new AdvertisementsInfo(url);
            advertisementInfo.SetLogger(mockLogger.Object);
            advertisementInfo.SetExcelExport(mockExcelExport.Object);
            advertisementInfo.SetHtmlXPathParser(mockHtmlXPathParser.Object);
            advertisementInfo.SetWebRequest(mockWebRequest.Object);

            //Act
            advertisementInfo.GetAllAdvertisements();

            //Assert
            Assert.IsNotNull(output);
            Assert.AreEqual(output, "Информация об объявлениях доступна по пути " + excelPath);

            Assert.IsNotNull(excelResult);
            Assert.AreEqual(excelResult, "exportComplete");

            Assert.AreEqual(adsTable.Rows.Count, 1);
            Assert.IsTrue(string.IsNullOrEmpty(adsTable.Rows[0]["Square"].ToString()));
        }

        [Test]
        public void Can_Get_Photos_Advertisement_With_Valid_Photos()
        {
            //Arrange
            string url = "test_url";

            string output = null;
            string excelResult = null;
            string excelPath = "test_file.csv";
            DataTable adsTable = null;

            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(x => x.Write(It.IsAny<string>()))
                .Callback<string>(x => output = x);

            var mockExcelExport = new Mock<IExcelExport>();
            mockExcelExport.Setup(x => x.ExportExcel(It.IsAny<DataTable>(), It.IsAny<string>()))
                .Callback<DataTable, string>(
                (table, s) =>
                {
                    adsTable = table;
                    excelResult = "exportComplete";
                });
            mockExcelExport.Object.SetFullPath("", "test_file.csv");

            var mockHtmlXPathParser = new Mock<IHtmlXPathParser>();
            mockHtmlXPathParser.Setup(x => x.GetAttributesValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<string> { "photos" });
            mockHtmlXPathParser.Setup(x => x.GetAttributeValue(It.IsNotIn("pageContent"), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("attributeValue");
            mockHtmlXPathParser.Setup(x => x.GetAttributeValue(It.IsIn("pageContent"), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("");
            mockHtmlXPathParser.Setup(x => x.GetTagInnerText(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("");
            mockExcelExport.Object.SetFullPath("", "test_file.csv");

            var mockWebRequest = new Mock<IRequestToServer>();
            mockWebRequest.Setup(x => x.GetPageContent(It.IsAny<string>()))
                .Returns("pageContent");

            var advertisementInfo = new AdvertisementsInfo(url);
            advertisementInfo.SetLogger(mockLogger.Object);
            advertisementInfo.SetExcelExport(mockExcelExport.Object);
            advertisementInfo.SetHtmlXPathParser(mockHtmlXPathParser.Object);
            advertisementInfo.SetWebRequest(mockWebRequest.Object);

            //Act
            advertisementInfo.GetAllAdvertisements();

            //Assert
            Assert.IsNotNull(output);
            Assert.AreEqual(output, "Информация об объявлениях доступна по пути " + excelPath);

            Assert.IsNotNull(excelResult);
            Assert.AreEqual(excelResult, "exportComplete");

            Assert.AreEqual(adsTable.Rows.Count, 1);
            Assert.AreEqual(adsTable.Rows[0]["Photos"], "photos;");
        }

        [Test]
        public void Cannot_Get_Photos_Advertisement_With_Null_Photos()
        {
            //Arrange
            string url = "test_url";
            string null_value = null;

            string output = null;
            string excelResult = null;
            string excelPath = "test_file.csv";
            DataTable adsTable = null;

            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(x => x.Write(It.IsAny<string>()))
                .Callback<string>(x => output = x);

            var mockExcelExport = new Mock<IExcelExport>();
            mockExcelExport.Setup(x => x.ExportExcel(It.IsAny<DataTable>(), It.IsAny<string>()))
                .Callback<DataTable, string>(
                (table, s) =>
                {
                    adsTable = table;
                    excelResult = "exportComplete";
                });
            mockExcelExport.Object.SetFullPath("", "test_file.csv");

            var mockHtmlXPathParser = new Mock<IHtmlXPathParser>();
            mockHtmlXPathParser.Setup(x => x.GetAttributesValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<string> { null_value });
            mockHtmlXPathParser.Setup(x => x.GetAttributeValue(It.IsNotIn("pageContent"), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("attributeValue");
            mockHtmlXPathParser.Setup(x => x.GetAttributeValue(It.IsIn("pageContent"), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("");
            mockHtmlXPathParser.Setup(x => x.GetTagInnerText(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(null_value);
            mockExcelExport.Object.SetFullPath("", "test_file.csv");

            var mockWebRequest = new Mock<IRequestToServer>();
            mockWebRequest.Setup(x => x.GetPageContent(It.IsAny<string>()))
                .Returns("pageContent");

            var advertisementInfo = new AdvertisementsInfo(url);
            advertisementInfo.SetLogger(mockLogger.Object);
            advertisementInfo.SetExcelExport(mockExcelExport.Object);
            advertisementInfo.SetHtmlXPathParser(mockHtmlXPathParser.Object);
            advertisementInfo.SetWebRequest(mockWebRequest.Object);

            //Act
            advertisementInfo.GetAllAdvertisements();

            //Assert
            Assert.IsNotNull(output);
            Assert.AreEqual(output, "Информация об объявлениях доступна по пути " + excelPath);

            Assert.IsNotNull(excelResult);
            Assert.AreEqual(excelResult, "exportComplete");

            Assert.AreEqual(adsTable.Rows.Count, 1);
            Assert.IsTrue(string.IsNullOrEmpty(adsTable.Rows[0]["Photos"].ToString()));
        }

       /* [Test]
        public void Can_Get_Advertisements_Info_From_Valid_Few_Page()
        {
            //Arrange
            string url = "test_url";

            string output = null;
            string excelResult = null;
            string excelPath = "test_file.csv";
            DataTable adsTable = null;

            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(x => x.Write(It.IsAny<string>()))
                .Callback<string>(x => output = x);

            var mockExcelExport = new Mock<IExcelExport>();
            mockExcelExport.Setup(x => x.ExportExcel(It.IsAny<DataTable>(), It.IsAny<string>()))
                .Callback<DataTable, string>(
                (table, s) =>
                {
                    adsTable = table;
                    excelResult = "exportComplete";
                });
            mockExcelExport.Object.SetFullPath("", "test_file.csv");

            var mockHtmlXPathParser = new Mock<IHtmlXPathParser>();
            mockHtmlXPathParser.Setup(x => x.GetAttributesValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<string> { "attributesValue" });
            mockHtmlXPathParser.Setup(x => x.GetAttributeValue(It.IsNotIn("pageContent"), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("attributeValue");
            mockHtmlXPathParser.Setup(x => x.GetAttributeValue(It.IsIn("pageContent"), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("");
            mockHtmlXPathParser.Setup(x => x.GetTagInnerText(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("30.01.2017 18:50");
            mockExcelExport.Object.SetFullPath("", "test_file.csv");

            var mockWebRequest = new Mock<IRequestToServer>();
            mockWebRequest.Setup(x => x.GetPageContent(It.IsAny<string>()))
                .Returns("pageContent");

            var advertisementInfo = new AdvertisementsInfo(url);
            advertisementInfo.SetLogger(mockLogger.Object);
            advertisementInfo.SetExcelExport(mockExcelExport.Object);
            advertisementInfo.SetHtmlXPathParser(mockHtmlXPathParser.Object);
            advertisementInfo.SetWebRequest(mockWebRequest.Object);

            //Act
            advertisementInfo.GetAllAdvertisements();

            //Assert
            Assert.IsNotNull(output);
            Assert.AreEqual(output, "Информация об объявлениях доступна по пути " + excelPath);

            Assert.IsNotNull(excelResult);
            Assert.AreEqual(excelResult, "exportComplete");

            Assert.AreEqual(adsTable.Rows.Count, 1);
            Assert.AreEqual(adsTable.Rows[0]["Address"], "30.01.2017 18:50");
            Assert.AreEqual(adsTable.Rows[0]["Price"], "30.01.2017 18:50");
            Assert.AreEqual(adsTable.Rows[0]["PlacingDate"], new DateTime(2017, 01, 30, 18, 50, 00));
            Assert.IsTrue(string.IsNullOrEmpty(adsTable.Rows[0]["Square"].ToString()));
            Assert.AreEqual(adsTable.Rows[0]["Photos"], "attributesValue;");
        }*/

        [Test]
        public void Cannot_Get_Advertisements_Info_From_Null_Url()
        {
            //Arrange
            string output = null;
                      
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(l => l.Write(It.IsAny<string>()))
                .Callback<string>(l => output = l);

            var advertisementInfo = new AdvertisementsInfo(null);
            advertisementInfo.SetLogger(mockLogger.Object);

            //Act
            advertisementInfo.GetAllAdvertisements();

            //Assert
            Assert.IsNotNull(output);
            Assert.AreEqual("Сервер недоступен, либо превышен запрос обращений на сервер", output);
        }

        [Test]
        public void Cannot_Get_Advertisements_Info_From_Nonexistent_Url()
        {
            //Arrange
            string output = null;

            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(l => l.Write(It.IsAny<string>()))
                .Callback<string>(l => output = l);

            var advertisementInfo = new AdvertisementsInfo("nonexistent_url");
            advertisementInfo.SetLogger(mockLogger.Object);

            //Act
            advertisementInfo.GetAllAdvertisements();

            //Assert
            Assert.IsNotNull(output);
            Assert.AreEqual("Сервер недоступен, либо превышен запрос обращений на сервер", output);
        }
    }
}
