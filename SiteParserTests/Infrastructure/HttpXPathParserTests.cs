using NUnit.Framework;
using SiteParser.Infrastructure.Abstract;
using SiteParser.Infrastructure.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteParserTests.Infrastructure
{
    [TestFixture]
    public class HttpXPathParserTests
    {
        [Test]
        public void Can_Get_Tag_Inner_Text_With_Valid_Content_And_Tag()
        {
            //Arrange
            IHtmlXPathParser htmlXPathParser = new HtmlXPathParser();

            var content = "<div>Test content</div>";

            //Act
            var innerText = htmlXPathParser.GetTagInnerText(content, "div");

            //Assert
            Assert.IsNotNull(innerText);
            Assert.AreEqual("Test content", innerText);
        }

        [Test]
        public void Can_Get_Null_Inner_Text_With_Non_Existent_Tag()
        {
            //Arrange
            IHtmlXPathParser htmlXPathParser = new HtmlXPathParser();

            var content = "<div>Test content</div>";

            //Act
            var innerText = htmlXPathParser.GetTagInnerText(content, "a");

            //Assert
            Assert.IsNotNull(string.IsNullOrEmpty(innerText));
        }

        [Test]
        public void Cannot_Get_Tag_Inner_Text_From_Null_Tag()
        {
            //Arrange
            IHtmlXPathParser htmlXPathParser = new HtmlXPathParser();

            var content = "<div>Test content</div>";

            //Act and Assert
            Assert.That(() => htmlXPathParser.GetTagInnerText(content, null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Cannot_Get_Tag_Inner_Text_From_Null_Content()
        {
            //Arrange
            IHtmlXPathParser htmlXPathParser = new HtmlXPathParser();

            //Act and Assert
            Assert.That(() => htmlXPathParser.GetTagInnerText(null, "div"), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Can_Get_Attribute_Value_With_Valid_Params()
        {
            //Arrange
            IHtmlXPathParser htmlXPathParser = new HtmlXPathParser();

            string href = "https://www.avito.ru/";

            var content = "<a href=\"" + href + "\"/>";

            //Act
            var attrValue = htmlXPathParser.GetAttributeValue(content, "a", "href");

            //Assert
            Assert.IsNotNull(attrValue);
            Assert.AreEqual(href, attrValue);
        }

        [Test]
        public void Cannot_Get_Attribute_Value_With_Nonexistent_Tag()
        {
            //Arrange
            IHtmlXPathParser htmlXPathParser = new HtmlXPathParser();

            string href = "https://www.avito.ru/";

            var content = "<a href=\"" + href + "\"/>";

            //Act
            var attrValue = htmlXPathParser.GetAttributeValue(content, "div", "href");

            //Assert
            Assert.IsTrue(string.IsNullOrEmpty(attrValue));
        }

        [Test]
        public void Cannot_Get_Attribute_Value_With_Nonexistent_Attr_Name()
        {
            //Arrange
            IHtmlXPathParser htmlXPathParser = new HtmlXPathParser();

            string href = "https://www.avito.ru/";

            var content = "<a href=\"" + href + "\"/>";

            //Act
            var attrValue = htmlXPathParser.GetAttributeValue(content, "a", "src");

            //Assert
            Assert.IsTrue(string.IsNullOrEmpty(attrValue));
        }

        [Test]
        public void Cannot_Get_Attribute_Value_From_Null_Content()
        {
            //Arrange
            IHtmlXPathParser htmlXPathParser = new HtmlXPathParser();

            //Act and Assert
            Assert.That(() => htmlXPathParser.GetAttributeValue(null, null, "href"), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Cannot_Get_Attribute_Value_With_Null_Tag()
        {
            //Arrange
            IHtmlXPathParser htmlXPathParser = new HtmlXPathParser();

            string href = "https://www.avito.ru/";

            var content = "<a href=\"" + href + "\"/>";

            //Act and Assert
            Assert.That(() => htmlXPathParser.GetAttributeValue(content, null, "href"), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Cannot_Get_Attribute_Value_With_Null_Attr_Name()
        {
            //Arrange
            IHtmlXPathParser htmlXPathParser = new HtmlXPathParser();

            string href = "https://www.avito.ru/";

            var content = "<a href=\"" + href + "\"/>";

            //Act and Assert
            Assert.That(() => htmlXPathParser.GetAttributeValue(content, "a", null), Throws.TypeOf<ArgumentNullException>());
        }


        // */*/*

        [Test]
        public void Can_Get_Attributes_Value_With_Few_Tags()
        {
            //Arrange
            IHtmlXPathParser htmlXPathParser = new HtmlXPathParser();

            string firstImage = "https://img.avito.st/640x480/1.jpg";
            string secondImage = "https://img.avito.st/640x480/2.jpg";

            var content = "<img src=\"" + firstImage + "\"/><img src=\"" + secondImage + "\"/><a href=\"123\"/>";

            //Act
            var attrValues = htmlXPathParser.GetAttributesValue(content, "img", "src");

            //Assert
            Assert.IsNotNull(attrValues);
            Assert.AreEqual(attrValues.GetType(), typeof(List<string>));

            Assert.AreEqual(attrValues.Count, 2);
            Assert.AreEqual(firstImage, attrValues[0]);
            Assert.AreEqual(secondImage, attrValues[1]);
        }

        [Test]
        public void Can_Get_Attributes_Value_With_One_Tag()
        {
            //Arrange
            IHtmlXPathParser htmlXPathParser = new HtmlXPathParser();

            string firstImage = "https://img.avito.st/640x480/1.jpg";

            var content = "<img src=\"" + firstImage + "\"/>";

            //Act
            var attrValues = htmlXPathParser.GetAttributesValue(content, "img", "src");

            //Assert
            Assert.IsNotNull(attrValues);
            Assert.AreEqual(attrValues.GetType(), typeof(List<string>));

            Assert.AreEqual(attrValues.Count, 1);
            Assert.AreEqual(firstImage, attrValues[0]);
        }
        
        [Test]
        public void Cannot_Get_Attributes_Value_With_Nonexistent_Tag()
        {
            //Arrange
            IHtmlXPathParser htmlXPathParser = new HtmlXPathParser();

            string firstImage = "https://img.avito.st/640x480/1.jpg";

            var content = "<img src=\"" + firstImage + "\"/>";

            //Act
            var attrValues = htmlXPathParser.GetAttributesValue(content, "a", "src");

            //Assert
            Assert.IsNotNull(attrValues);
            Assert.AreEqual(attrValues.Count, 0);
        }

        
        [Test]
        public void Cannot_Get_Attributes_Value_With_Nonexistent_Attr_Name()
        {
            //Arrange
            IHtmlXPathParser htmlXPathParser = new HtmlXPathParser();

            string firstImage = "https://img.avito.st/640x480/1.jpg";

            var content = "<img src=\"" + firstImage + "\"/>";

            //Act
            var attrValues = htmlXPathParser.GetAttributesValue(content, "img", "href");

            //Assert
            Assert.IsNotNull(attrValues);
            Assert.AreEqual(attrValues.Count, 0);
        }

        
        [Test]
        public void Cannot_Get_Attributes_Value_From_Null_Content()
        {
            //Arrange
            IHtmlXPathParser htmlXPathParser = new HtmlXPathParser();

            //Act and Assert
            Assert.That(() => htmlXPathParser.GetAttributesValue(null, null, "href"), Throws.TypeOf<ArgumentNullException>());
        }
        
        [Test]
        public void Cannot_Get_Attributes_Value_With_Null_Tag()
        {
            //Arrange
            IHtmlXPathParser htmlXPathParser = new HtmlXPathParser();

            string firstImage = "https://img.avito.st/640x480/1.jpg";

            var content = "<img src=\"" + firstImage + "\"/>";

            //Act and Assert
            Assert.That(() => htmlXPathParser.GetAttributeValue(content, null, "src"), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Cannot_Get_Attributes_Value_With_Null_Attr_Name()
        {
            //Arrange
            IHtmlXPathParser htmlXPathParser = new HtmlXPathParser();

            string firstImage = "https://img.avito.st/640x480/1.jpg";

            var content = "<img src=\"" + firstImage + "\"/>";

            //Act and Assert
            Assert.That(() => htmlXPathParser.GetAttributeValue(content, "img", null), Throws.TypeOf<ArgumentNullException>());
        }
    }
}
