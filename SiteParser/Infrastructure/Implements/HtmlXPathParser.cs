using HtmlAgilityPack;
using SiteParser.Infrastructure.Abstract;
using System.Collections.Generic;
using System;

namespace SiteParser.Infrastructure.Implements
{
    public class HtmlXPathParser : IHtmlXPathParser
    {
        public string GetAttributeValue(string content, string xPath, string attrName)
        {
            var tag = GetNodeFromPage(content, xPath);
            if (tag != null && tag.Attributes[attrName] != null)
            {
                return tag.Attributes[attrName].Value;
            }

            return string.Empty;
        }

        public List<string> GetAttributesValue(string content, string xPath, string attrName)
        {
            List<string> attrValues = new List<string>();
            var tags = GetNodesFromPage(content, xPath);
            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    if (tag.Attributes[attrName] != null && tag.Attributes[attrName].Value != null)
                    {
                        attrValues.Add(tag.Attributes[attrName].Value);
                    }
                }
            }

            return attrValues;
        }

        public string GetTagInnerText(string content, string xPath)
        {
            string innerText = null;
            var node = GetNodeFromPage(content, xPath);

            if (node != null)
            {
                innerText = node.InnerText;
            }

            return innerText;
        }

        private HtmlNode GetNodeFromPage(string content, string xPath)
        {
            HtmlDocument page = new HtmlDocument();
            page.LoadHtml(content);

            return page.DocumentNode.SelectSingleNode(xPath);
        }

        private HtmlNodeCollection GetNodesFromPage(string content, string xPath)
        {
            HtmlDocument page = new HtmlDocument();
            page.LoadHtml(content);

            return page.DocumentNode.SelectNodes(xPath);
        }
    }
}
