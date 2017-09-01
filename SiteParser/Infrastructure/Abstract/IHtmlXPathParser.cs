using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteParser.Infrastructure.Abstract
{
    public interface IHtmlXPathParser
    {
        string GetAttributeValue(string content, string xPath, string attrName);

        List<string> GetAttributesValue(string content, string xPath, string attrName);

        string GetTagInnerText(string content, string xPath);
    }
}
