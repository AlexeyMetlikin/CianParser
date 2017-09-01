using SiteParser.Infrastructure.Abstract;
using System;

namespace SiteParser.Infrastructure.Implements
{
    public class Logger : ILogger
    {
        public void Write(string outputText)
        {
            Console.WriteLine(outputText);
        }
    }
}
