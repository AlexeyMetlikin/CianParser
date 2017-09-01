using NUnit.Framework;
using SiteParser.Infrastructure.Abstract;
using SiteParser.Infrastructure.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SiteParserTests.Infrastructure
{
    [TestFixture]
    public class RequestToServerTests
    {
        [Test]
        public void Can_Get_Page_Content_With_Valid_Url()
        {
            //Arrange
            var url = "https://www.avito.ru/";
            IRequestToServer webRequest = new RequestToServer();

            //Act
            string content = webRequest.GetPageContent(url);

            //Assert
            Assert.IsNotNull(content);
            Assert.IsTrue(content.Contains("<html>"));
        }

        [Test]
        public void Can_Get_Page_Content_With_Proxy()
        {
            //Arrange
            var url = "https://www.avito.ru/";

            IRequestToServer webRequest = new RequestToServer();
            webRequest.SetWebProxy("213.204.37.254", 80);
            webRequest.IsRequestByProxy = true;

            //Act
            string content = webRequest.GetPageContent(url);

            //Assert
            Assert.IsNotNull(content);
            Assert.IsTrue(content.Contains("<html>"));
        }

        [Test]
        public void Cannot_Get_Page_Content_With_Invalid_Url()
        {
            //Arrange
            var url = "https://123123/";
            IRequestToServer webRequest = new RequestToServer();

            //Act and Assert
            Assert.That(() => webRequest.GetPageContent(url), Throws.TypeOf<HttpRequestException>());
        }
    }
}
