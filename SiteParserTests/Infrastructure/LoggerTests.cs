using NUnit.Framework;
using SiteParser.Infrastructure.Abstract;
using SiteParser.Infrastructure.Implements;
using System;
using System.IO;

namespace SiteParserTests.Infrastructure
{
    [TestFixture]
    public class LoggerTests
    {
        [Test]
        public void Can_Write_In_Console()
        {            
            using (var outputWritter = new StringWriter())
            {
                // Arrange
                Console.SetOut(outputWritter);

                ILogger logger = new Logger();

                string textForWrite = "Test text";

                // Act
                logger.Write(textForWrite);

                // Assert
                Assert.AreEqual(textForWrite + "\r\n", outputWritter.ToString());
            }
        }
    }
}
